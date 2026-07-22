namespace GameStore.API.Dtos;

// A DTo is a contract between the client and server since it represents
// a shares agrement about the data will be transferred and used.
public record GameSummaryDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);