using GameStore.Api.Dtos;
using GameStore.API.Dtos;

const string GetGameEndpointName = "GetGame";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<GameDto> games =[
    new (
        1, 
        "The Legend of Zelda: Breath of the Wild", 
        "Action-adventure",
        19.99M, 
        new DateOnly(2017, 3, 3)),
    new (
        2, 
        "Super Mario Odyssey", 
        "Platformer", 
        59.99M, 
        new DateOnly(2017, 10, 27)),
    new (
        3, 
        "Red Dead Redemption 2", 
        "Action-adventure", 
        39.99M, 
        new DateOnly(2018, 10, 26)),
    new (
        4, 
        "The Witcher 3: Wild Hunt", 
        "Action RPG", 
        39.99M, 
        new DateOnly(2015, 5, 19)),
    new (
        5, 
        "God of War", 
        "Action-adventure", 
        39.99M, 
        new DateOnly(2018, 4, 20))
];
// GET /games
app.MapGet("/games", () => games);

// GET /games/1
app.MapGet("/games/{id}",(int id) => 
{
    var game = games.Find(game => game.Id == id);

    return game is null ? Results.NotFound() : Results.Ok(game);
}).WithName(GetGameEndpointName);

// POST /games
app.MapPost("/games",(CreateGameDto newGame) =>
{
    GameDto game = new (
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

// PUT /games/1
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    var index = games.FindIndex(game => game.Id == id);

    if (index == -1)
    {
        return Results.NotFound();
    }

    games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );

    return Results.NoContent();
});

// DELETE /games/1
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();
