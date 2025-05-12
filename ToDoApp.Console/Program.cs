// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoApp.Console1;

Console.WriteLine("Hello, World!");

//Create service collection for Dependency Injection
var serviceCollection = new ServiceCollection();

//Build a configuration
IConfiguration configuration;
configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

//Add configuration to the service collection
serviceCollection.AddSingleton<IConfiguration>(configuration);
serviceCollection.AddSingleton<Test>();

//Test the configuration
var serviceProvider = serviceCollection.BuildServiceProvider();
var testInstance = serviceProvider.GetService<Test>();
testInstance.TestMethod();
