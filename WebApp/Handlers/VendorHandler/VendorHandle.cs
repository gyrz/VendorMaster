using BusinessLogicService.Services;
using BusinessLogicService.Services.PhoneSvc;
using Contracts.Dto.Phone;
using Contracts.Dto.Vendor;
using DataAccess.Cache;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace VendorMaster.Handlers.VendorHandler
{
    public class VendorHandle<T, T2> : IHandle where T : class where T2 : class
    {
        private readonly int id;
        private readonly IEnumerable<T> dtos;
        private readonly IBase<T, T2> srv;
        private readonly IRedisCache redisCache;

        public VendorHandle(
            int id,
            IEnumerable<T> dtos,
            IBase<T, T2> srv,
            IRedisCache redisCache)
        {
            this.id = id;
            this.dtos = dtos;
            this.srv = srv;
            this.redisCache = redisCache;
        }

        public async Task<string> handle()
        {
            if (dtos == null || dtos.Count() == 0) return string.Empty;

            StringBuilder err = new StringBuilder();
            List<int> handledIds = new List<int>();
            foreach (var item in dtos)
            {
                if (item is not SimpleVendorDto dto) continue;

                dto.VendorId = id;
                var r = await srv.AddOrUpdate(item);
                if (r.ResultCode == 400)
                {
                    err.AppendLine($"{typeof(T2).ToString()} with id: {dto.Id} Message: " + r.Message);
                    continue;
                }

                handledIds.Add(r.Data);
                await redisCache.Get(typeof(T2).ToString(), r.Data, async () => await srv.Get(r.Data));
            }

            // remove entities that are not in the list
            if (handledIds.Count > 0) await srv.RemoveAll(handledIds, id);

            return err.ToString();
        }
    }
}
