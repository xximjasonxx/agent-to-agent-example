using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using nfl_api.Services;

namespace nfl_api.Functions;

public class GetScheduleFunction
{
    private readonly ILogger<GetScheduleFunction> _logger;
    private readonly INflScheduleService _scheduleService;

    public GetScheduleFunction(ILogger<GetScheduleFunction> logger, INflScheduleService scheduleService)
    {
        _logger = logger;
        _scheduleService = scheduleService;
    }

    [Function("GetSchedule")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "schedule/{season_number}/week/{week_number}")] HttpRequestData req,
        int season_number,
        int week_number,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing request for season {Season}, week {Week}", season_number, week_number);

        // Validate season_number
        if (season_number < 2020 || season_number > 2030)
        {
            _logger.LogWarning("Invalid season number: {Season}", season_number);
            var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequestResponse.WriteAsJsonAsync(new
            {
                error = "Invalid season_number. Must be between 2020 and 2030."
            }, cancellationToken);
            return badRequestResponse;
        }

        // Validate week_number
        if (week_number < 1 || week_number > 17)
        {
            _logger.LogWarning("Invalid week number: {Week}", week_number);
            var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequestResponse.WriteAsJsonAsync(new
            {
                error = "Invalid week_number. Must be between 1 and 17 inclusive."
            }, cancellationToken);
            return badRequestResponse;
        }

        try
        {
            var games = await _scheduleService.GetGamesBySeasonAndWeekAsync(season_number, week_number, cancellationToken);

            // Map to response DTO without id
            var gameResponses = games.Select(g => new Models.NflGameResponse
            {
                Title = g.Title,
                Venue = g.Venue,
                Season = g.Season,
                WeekNo = g.WeekNo,
                Date = g.Date
            });

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(gameResponses, cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving schedule for season {Season}, week {Week}", season_number, week_number);
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(new
            {
                error = "An error occurred while retrieving the schedule."
            }, cancellationToken);
            return errorResponse;
        }
    }
}
