using InvoiceGenerator;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
            .AddSingleton<ILogger, ConsoleLogger>()
            .AddSingleton<IExcelUtilities, ExcelUtilities>()
            .AddSingleton<IAmountToWords, DoubleToStringConverter>()
            .AddSingleton<ISettingsProvider, JSONConfigSettingsProvider>()
            .AddSingleton<IGeneratorEngine, GeneratorEngine>()
            .AddSingleton<IAppartementDetailsReader, AppartementDetailsReader>()
            .BuildServiceProvider();

var settings = serviceProvider.GetService<ISettingsProvider>().GetSettings();
var appartementReader = serviceProvider.GetService<IAppartementDetailsReader>();
var apps = appartementReader.Execute(settings.DetailsFilePath);
var generator = serviceProvider.GetService<IGeneratorEngine>();
generator.Execute(apps);