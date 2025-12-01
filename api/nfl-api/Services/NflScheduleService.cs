using Npgsql;
using nfl_api.Models;

namespace nfl_api.Services;

public class NflScheduleService : INflScheduleService
{
    private readonly string _connectionString;

    public NflScheduleService(string host, string user, string password, string database)
    {
        _connectionString = $"Host={host};Username={user};Password={password};Database={database}";
    }

    public async Task<IEnumerable<NflGame>> GetGamesBySeasonAndWeekAsync(int season, int week, CancellationToken cancellationToken = default)
    {
        var games = new List<NflGame>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string query = @"
            SELECT id, title, venue, season, week_no, date, created_at, updated_at
            FROM nfl_schedules
            WHERE season = @season AND week_no = @week
            ORDER BY date";

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@season", (short)season);
        command.Parameters.AddWithValue("@week", (short)week);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            games.Add(new NflGame
            {
                Id = reader.GetGuid(0),
                Title = reader.GetString(1),
                Venue = reader.GetString(2),
                Season = reader.GetInt16(3),
                WeekNo = reader.GetInt16(4),
                Date = DateOnly.FromDateTime(reader.GetDateTime(5)),
                CreatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(7)
            });
        }

        return games;
    }
}
