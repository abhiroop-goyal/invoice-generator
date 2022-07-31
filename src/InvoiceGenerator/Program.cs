using InvoiceGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


string configFileName = args[0];

// Dependency builder.
var configurationBuild = new ConfigurationBuilder()
    .AddJsonFile(configFileName, optional: false, reloadOnChange: true)
    .Build();

ServiceProvider serviceProvider = new ServiceCollection()
            .AddSingleton<IExcelUtilities, ExcelUtilities>()
            .AddSingleton<IAmountToWords, DoubleToStringConverter>()
            .AddSingleton<IGeneratorEngine, GeneratorEngine>()
            .AddSingleton<IAppartementDetailsReader, AppartementDetailsReader>()
            .AddLogging((builder) =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            })
            .AddOptions()
            .Configure<InvoiceGeneratorSettings>(configurationBuild)
            .BuildServiceProvider();

// Code execution.
serviceProvider.GetService<IGeneratorEngine>().Execute(
    serviceProvider.GetService<IAppartementDetailsReader>().Execute());