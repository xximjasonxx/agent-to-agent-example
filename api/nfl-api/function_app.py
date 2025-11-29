import azure.functions as func
import json
import logging

app = func.FunctionApp()

logger = logging.getLogger(__name__)


@app.route(
    route="schedule/season/{season_number}/week/{week_number}",
    methods=["GET"],
    auth_level=func.AuthLevel.ANONYMOUS
)
def get_schedule(req: func.HttpRequest) -> func.HttpResponse:
    """
    Echo back season and week parameters.
    
    Route: /schedule/season/{season_number}/week/{week_number}
    Method: GET
    Auth: Anonymous
    
    Path Parameters:
        season_number: Season year
        week_number: Week number
    
    Returns:
        200: JSON object with provided values
    """
    logger.info("Processing schedule request")
    
    # Extract route parameters
    season_number = req.route_params.get('season_number')
    week_number = req.route_params.get('week_number')
    
    # Return values as JSON
    response_data = {
        "season_number": season_number,
        "week_number": week_number
    }
    
    logger.info(f"Returning: {response_data}")
    return func.HttpResponse(
        json.dumps(response_data, indent=2),
        status_code=200,
        mimetype="application/json"
    )
