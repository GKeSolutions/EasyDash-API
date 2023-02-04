using Core.Model.MissingTime;

namespace Core.Interface;

public interface IMissingTimeService
{
    Task<IEnumerable<MissingTime>> GetMissingTime(DateTime startDate, DateTime endDate);
    Task<Time> GetMissingTimePerUserPerWeek(Guid userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Time>> GetUsersWithMissingTimePerWeek(DateTime startDate, DateTime endDate);
}

