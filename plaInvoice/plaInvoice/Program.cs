using Microsoft.Extensions.Configuration;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.CommandLine;

namespace plaInvoice
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            ConfigureSyncFusion(args);
            
            var invoiceValue = new Option<decimal>(new string[] { "-a", "--amount" }, "Gross amount to be charged in this invoice");
            var companyToCharge = new Option<string>(new string[] { "-c", "--company" }, "Company that's going to be charged");

            var rootCommand = new RootCommand("Simple invoice generator");

            var generateInvoiceCommand = new Command("generate", "Generates an invoice with the provided parameters")
            {
                invoiceValue,
                companyToCharge
            };
            rootCommand.AddCommand(generateInvoiceCommand);

            generateInvoiceCommand.SetHandler((amount, company) =>
            {
                PrintValue(amount, company);
            }, invoiceValue, companyToCharge);

            return await rootCommand.InvokeAsync(args);
        }

        private static void ConfigureSyncFusion(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();

            var syncFusionKey = configuration.GetValue<string>("syncfusionApplicationKey");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncFusionKey);
        }

        static void PrintValue(decimal value, string company)
        {
            //Initialize HTML to PDF converter
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            //HTML string and Base URL 
            string htmlText = $"<html><body><h1>Congrats, {company} will pay you {value} dollars, you rich motherfucker!!!</h1></body></html>";
            string baseUrl = @"C:/Temp/HTMLFiles/";

            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert(htmlText, baseUrl);
            FileStream fileStream = new FileStream("HTML-to-PDF.pdf", FileMode.CreateNew, FileAccess.ReadWrite);
            //Save and close the PDF document.
            document.Save(fileStream);
            document.Close(true);
        }
    }
}