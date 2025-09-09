# dotnet-tool-gist sample

This sample shows how to use dotnet-tool-gist to add a gist reference to a project.

## Prerequisites

First of all, you need to install dotnet-tool-gist. You can do it by running the following command:

```bash
dotnet tool install dotnet-tool-gist --global
```

This will install the latest version of dotnet-tool-gist globally on your machine, so you can use it from anywhere in your terminal.

To verify that dotnet-tool-gist was insdotnettalled correctly, you can run the following command:

```bash
dotnet gist --version
```

If everything was installed correctly, you should see the version number of dotnet-tool-gist printed to the terminal.

## Sample

The sample project is a simple Asp.Net Core Minimal API project. It is going to use a simple library you can find [here](https://gist.github.com/fernandoescolar/fd0f87915264038fa463966428da2986), to create endpoints using record types.

First, you need to create a new project. You can do it by running the following command:

```bash
dotnet new web -o DemoApi
```

Then, you need to add the gist reference to the project. You can do it by running the following command:

```bash
cd DemoApi
dotnet gist add fd0f87915264038fa463966428da2986 --out ./ --namespace DemoApi
```

It will add a file called `RecordEndpoints.cs` to root directory of the project.

Then you can add your custom endpoint:

```csharp
// hello.cs
namespace DemoApi;

public record Hello() : Get("/")
{
    protected override IResult Handle()
        => Results.Ok("Hello World!");
}
```

To serve this kind of endpoints, you need to add the following line to the `Program.cs` file:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapEndpoints(); // <-- Add this line
app.Run();
```

Finally, you can run the project by running the following command:

```bash
dotnet run
```

And you can test the endpoint by running the following command:

```bash
$ curl http://localhost:5000/
"Hello World!"
```

## Final notes

dotnet-tool-gist is a very simple tool, but it can be very useful when you need to add a gist reference to a project. It is also very easy to use, and it is very easy to install.
