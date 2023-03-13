
var services  = new ServiceCollection();
services.AddSingleton<RootCommand, MainCommand>();
services.AddSingletonTypesAssignableFrom<CommandBase>();

var serviceProvider = services.BuildServiceProvider();
var rootCommand = serviceProvider.GetRequiredService<RootCommand>();

await rootCommand.InvokeAsync(args);