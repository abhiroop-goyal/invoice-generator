using InvoiceGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

ServiceProvider serviceProvider = new ServiceCollection()
            .AddSingleton<IExcelUtilities, ExcelUtilities>()
            .AddSingleton<IAmountToWords, DoubleToStringConverter>()
            .AddSingleton<ISettingsProvider, JSONConfigSettingsProvider>()
            .AddSingleton<IGeneratorEngine, GeneratorEngine>()
            .AddSingleton<IAppartementDetailsReader, AppartementDetailsReader>()
            .AddLogging((builder) =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            })
            .BuildServiceProvider();

InvoiceGeneratorSettings settings = serviceProvider.GetService<ISettingsProvider>().GetSettings();

IAppartementDetailsReader appartementReader = serviceProvider.GetService<IAppartementDetailsReader>();

List<Appartement> apps = appartementReader.Execute(
    settings.AppartementDetailsFilePath,
    settings.PastDuesFilePath,
    settings.DuesInterestRatePerAnnum);

IGeneratorEngine generator = serviceProvider.GetService<IGeneratorEngine>();
generator.Execute(apps);