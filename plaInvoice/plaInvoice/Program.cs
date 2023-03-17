using System.IO;
using System.CommandLine;

namespace plaInvoice
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
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

        static void PrintValue(decimal value, string company)
        {
            Console.WriteLine($"Congrats, {company} will pay you {value} dollars, you rich motherfucker!!!");
        }
    }
}