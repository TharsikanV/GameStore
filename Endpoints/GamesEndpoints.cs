using GameStore.Api.Dtos;
using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Models;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
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

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        // GET /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
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
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
    }

}