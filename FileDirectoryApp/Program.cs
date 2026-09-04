using Newtonsoft.Json;
using System.Text;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

GenerateSalesSummaryReport(salesFiles, storesDirectory, salesTotalDir);

Console.WriteLine($"Total sales: {salesTotal:C}");
Console.WriteLine($"Sales summary report written to {Path.Combine(salesTotalDir, "SalesSummary.txt")}");

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

void GenerateSalesSummaryReport(IEnumerable<string> salesFiles, string storesDirectory, string outputDirectory)
{
    var reportBuilder = new StringBuilder();
    double reportTotal = 0;
    var details = new List<(string FileName, double Amount)>();

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        double amount = data?.Total ?? 0;

        reportTotal += amount;
        details.Add((Path.GetRelativePath(storesDirectory, file), amount));
    }

    reportBuilder.AppendLine("Sales Summary");
    reportBuilder.AppendLine("----------------------------");
    reportBuilder.AppendLine($" Total Sales: {reportTotal:C}");
    reportBuilder.AppendLine();
    reportBuilder.AppendLine(" Details:");

    foreach (var (fileName, amount) in details)
    {
        reportBuilder.AppendLine($"  {fileName}: {amount:C}");
    }

    File.WriteAllText(Path.Combine(outputDirectory, "SalesSummary.txt"), reportBuilder.ToString());
}

record SalesData(double Total);
