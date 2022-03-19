using Blazor_Orders.Helpers;
using Shared_Orders.DTO;

namespace Blazor_Orders.Repository
{
    public class ScheduleRepository : IScheduleRepository
    {
        private IHttpService Service;
        public ScheduleRepository(IHttpService service)
        {
            Service = service;
        }
        public async Task<ScheduleDTO> GetByDate(DateTime dt)
        {
            return await Service.GetHelper<ScheduleDTO>($"{UrlServices.Schedule}/Get?dt={dt.ToString("yyyy-MM-dd")}");
        }
    }
}
