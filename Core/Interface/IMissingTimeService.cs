using Core.Model.MissingTime;

namespace Core.Interface;

public interface IMissingTimeService
{
    Task<IEnumerable<MissingTime>> GetMissingTime(DateTime startDate, DateTime endDate);
    Task<Time> GetTimePerUserPerWeek(Guid userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Time>> GetUsersTimePerWeek(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Time>> GetWeeksTimePerUser(Guid userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Time>> GetMissingTimeUsersPerTemplate(int templateId);
    Task<string> GetCcContactEmailAddress(Guid ccContact);
}

