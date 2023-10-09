using BusinessLogicService.Services;
using BusinessLogicService.Services.PhoneSvc;
using Contracts.Dto.Phone;
using Contracts.Dto.Vendor;
using DataAccess.Cache;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace VendorMaster.Handlers.VendorHandler
{
    public class VendorTranzaction<T, T2> : IHandle where T : class where T2 : class
    {
        private readonly int id;
        private readonly IEnumerable<T> dtos;
        private readonly IBaseService<T, T2> srv;
        private readonly IRedisCache redisCache;
        private List<int> handledIds;

        public VendorTranzaction(
            int id,
            IEnumerable<T> dtos,
            IBaseService<T, T2> srv,
            IRedisCache redisCache)
        {
            this.id = id;
            this.dtos = dtos;
            this.srv = srv;
            this.redisCache = redisCache;
            handledIds = new List<int>();
        }

        public async Task<string> Handle()
        {
            if (dtos == null || dtos.Count() == 0) return string.Empty;

            StringBuilder err = new StringBuilder();
            
            foreach (var item in dtos)
            {
                if (item is not BaseVendorRelationDto dto) continue;

                dto.VendorId = id;
                var r = await srv.AddOrUpdate(item);
                if (r.ResultCode == 400)
                {
                    err.AppendLine($"{typeof(T2).ToString()} with id: {dto.Id} Message: " + r.Message);
                    continue;
                }

                handledIds.Add(r.Data);
                await redisCache.Get<T2>(typeof(T2).ToString(), r.Data, async () => await srv.Get(r.Data));
            }

            // remove entities that are not in the list
            if (handledIds.Count > 0) await srv.RemoveAll(handledIds, id);

            return err.ToString();
        }

        public async Task<string> RollBack()
        {
            StringBuilder err = new StringBuilder();
            for (int i = handledIds.Count - 1; i >= 0; i--)
            {
                int id = handledIds[i];
                var r = await srv.Remove(id);
                if (r.ResultCode == 400)
                {
                    err.AppendLine($"{typeof(T2).ToString()} with id: {id} cannot be deleted! Message: " + r.Message);
                    continue;
                }
                await redisCache.Remove<T2>(typeof(T2).ToString(), id, async () => await srv.Remove(id));
            }

            return err.ToString();
        }
    }
}
