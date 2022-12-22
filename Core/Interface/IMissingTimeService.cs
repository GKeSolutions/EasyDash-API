using Core.Model.MissingTime;

namespace Core.Interface;

public interface IMissingTimeService
{
    Task<IEnumerable<Time>> GetMissingTime();
}

