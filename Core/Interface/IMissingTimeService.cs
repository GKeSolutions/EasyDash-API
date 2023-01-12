using Core.Model.MissingTime;

namespace Core.Interface;

public interface IMissingTimeService
{
    Task<IEnumerable<MissingTime>> GetMissingTime(DateTime startDate, DateTime endDate);
}

