using Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Cache
{
    public class RedisCache : IRedisCache
    {
        private readonly IDatabase dataBase;
        public RedisCache(IConnectionMultiplexer cm)
        {
            this.dataBase = cm.GetDatabase();
        }

        public async Task<Result<T>> Get<T>(string key, int id, Func<Task<Result<T>>> func)
        {
            var res = await GetCache<T>(key, id);
            if (!res.Equals(default(KeyValuePair<int,T>))) return new Result<T>(res.Value);

            var data = await func();
            if(data.ResultCode != 200)
            {
                return data;
            }

            await SetData(key, id, data.Data, DateTimeOffset.Now.AddMinutes(5));

            return data;
        }

        /// <summary>
        /// This refresh every n minutes | -1 if default refresh(1h)
        /// This should be used only on rarely/none altering models
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<Result<IEnumerable<T>>> GetListEveryNMinutes<T>(string key, int minutes, Func<Task<Result<Dictionary<int, T>>>> func)
        {
            var obj = await dataBase.StringGetAsync(key+"offset");
            if (obj.HasValue)
            {
                var time = DateTime.Parse(obj.ToString());
                if(time.AddMinutes(minutes) > DateTime.UtcNow || minutes == -1)
                {
                    var cachedRes = await GetCacheList<T>(key);
                    return new Result<IEnumerable<T>>(cachedRes.Select(x=>x.Value).AsEnumerable());
                }
            }

            var res = await func();
            if (res.ResultCode != 200)
            {
                return new Result<IEnumerable<T>>() 
                { 
                    ResultCode = res.ResultCode, 
                    Message = res.Message 
                };
            }

            await dataBase.StringSetAndGetAsync(key, JsonConvert.SerializeObject(res.Data), minutes == -1 ? null : new TimeSpan(1, 0, 0));
            await dataBase.StringSetAndGetAsync(key + "offset", DateTime.UtcNow.ToString(), new TimeSpan(1,0,0));

            return new Result<IEnumerable<T>>()
            {
                Data = res.Data.Select(x => x.Value).AsEnumerable(),
                ResultCode = res.ResultCode,
                Message = res.Message
            };
        }

        /// <summary>
        /// This only collects untracked data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<Result<IEnumerable<T>>> GetListUntracked<T>(string key, Func<int[], Task<Result<Dictionary<int, T>>>> func)
        {
            var cachedList = await GetCacheList<T>(key);
            if(cachedList?.Count()> 0)
            {
                var res = await func(cachedList.Select(x=>x.Key).ToArray());
                if (res.ResultCode != 200)
                {
                    return new Result<IEnumerable<T>>() { ResultCode = res.ResultCode, Message = res.Message };
                }


                foreach(var item in res.Data)
                {
                    if(cachedList.ContainsKey(item.Key))
                        cachedList[item.Key] = item.Value;
                    else
                        cachedList.Add(item.Key, item.Value);
                }
            }
            else if(cachedList == null)
            {
                int[] ints = { };
                var res = await func(ints);
                if (res.ResultCode != 200)
                {
                    return new Result<IEnumerable<T>>() { ResultCode = res.ResultCode, Message = res.Message };
                }

                cachedList = new Dictionary<int, T>();
                foreach (var item in res.Data)
                {
                    cachedList.Add(item.Key, item.Value);
                }
            }


            await dataBase.StringSetAndGetAsync(key, JsonConvert.SerializeObject(cachedList), DateTimeOffset.Now.AddMinutes(5) - DateTimeOffset.Now);

            return new Result<IEnumerable<T>>()
            {
                Data = cachedList.Select(x => x.Value).AsEnumerable()
            };
        }

        public async Task<T> AddOrUpdateCache<T>(string key, int id, T data)
        {
            var exists = await GetCache<T>(key, id);
            if(!exists.Equals(default(KeyValuePair<int, T>)))
            {
                await RemoveData<T>(key,id);
            }

            var res = await SetData(key, id, data, DateTimeOffset.Now.AddMinutes(5));
            return res.Value;
        }

        public async Task<Result<bool>> Remove<T>(string key, int id, Func<Task<Result<bool>>> func)
        {
            var data = await func();
            if(data.ResultCode != 200)
            {
                return data;
            }

            await RemoveData<T>(key, id);

            return data;
        }

        private async Task<KeyValuePair<int,T>> GetCache<T>(string key, int id)
        {
            var obj = await dataBase.StringGetAsync(key);
            if(obj.HasValue)
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<int,T>>(obj);
                return dict.FirstOrDefault(x => x.Key == id);
            }

            return default(KeyValuePair<int, T>);
        }

        private async Task<Dictionary<int, T>> GetCacheList<T>(string key)
        {
            var obj = await dataBase.StringGetAsync(key);
            if (obj.HasValue)
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<int, T>>(obj);
                return dict;
            }

            return default(Dictionary<int, T>);
        }

        private async Task<object> RemoveData<T>(string key, int id)
        {
            var obj = await dataBase.StringGetAsync(key);
            if (obj.HasValue)
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<int, T>>(obj);
                var item = dict.FirstOrDefault(x => x.Key == id);
                if(!item.Equals(default(KeyValuePair<int, T>)))
                {
                    dict.Remove(item.Key);
                }

                if(dict.Count == 0)
                {
                    return await dataBase.KeyDeleteAsync(key);
                }
            }

            return true;
        }

        private async Task<KeyValuePair<int, T>> SetData<T>(string key, int id, T value, DateTimeOffset validity)
        {
            var obj = await dataBase.StringGetAsync(key);
            if (!obj.HasValue)
            {
                var dict = new Dictionary<int, T>();
                dict.Add(id, value);
                obj = await dataBase.StringSetAndGetAsync(key, JsonConvert.SerializeObject(dict), validity - DateTimeOffset.Now);
            }
            else
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<int, T>>(obj);
                if(dict.ContainsKey(id))
                {
                    dict[id] = value;
                }
                else
                {
                    dict.Add(id, value);
                }

                obj = await dataBase.StringSetAndGetAsync(key, JsonConvert.SerializeObject(dict), validity - DateTimeOffset.Now);
            }

            return new KeyValuePair<int, T>(id, value);
        }

    }
}
