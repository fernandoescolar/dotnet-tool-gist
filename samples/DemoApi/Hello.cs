namespace DemoApi;

public record Hello() : Get("/")
{
    protected override IResult Handle()
        => Results.Ok("Hello World!");
}