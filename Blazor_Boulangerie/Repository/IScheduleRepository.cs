using Shared_Orders.DTO;

namespace Blazor_Orders.Repository
{
    public interface IScheduleRepository
    {
        Task<ScheduleDTO> GetByDate(DateTime dt);
    }
}
