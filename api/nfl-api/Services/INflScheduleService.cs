using nfl_api.Models;

namespace nfl_api.Services;

public interface INflScheduleService
{
    Task<IEnumerable<NflGame>> GetGamesBySeasonAndWeekAsync(int season, int week, CancellationToken cancellationToken = default);
}
