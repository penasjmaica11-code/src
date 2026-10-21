var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot")
});
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/api/sort/validate", (SortValidationRequest? request) =>
{
    if (request is null ||
        string.IsNullOrWhiteSpace(request.ExpectedLabel) ||
        string.IsNullOrWhiteSpace(request.SelectedLabel))
    {
        return Results.BadRequest(new { error = "Both answer labels are required." });
    }

    var isCorrect = string.Equals(
        request.ExpectedLabel.Trim(),
        request.SelectedLabel.Trim(),
        StringComparison.OrdinalIgnoreCase);

    return Results.Ok(new SortValidationResponse(isCorrect));
});

app.Run();

public sealed record SortValidationRequest(string? ExpectedLabel, string? SelectedLabel);

public sealed record SortValidationResponse(bool IsCorrect);
