using InvoiceGenerator;
var settings = SettingsProvider.Instance.GetSettings();
var appartementReader = new AppartementDetailsReader(Logger.Instance);
var apps = appartementReader.Execute(settings.DetailsFilePath);
var generator = new GeneratorEngine(settings, apps, Logger.Instance);
generator.Execute();