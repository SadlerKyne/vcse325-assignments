# vCSE 325 Assignments

Ongoing notes, assignment artifacts, and testing evidence for vCSE 325: .NET Software Development.

## Week 1

### Part 1: Pizza Web API — Pizzas List and CRUD Evidence

### Pizzas List (existing 3 + 1 added record)

```csharp
public static List<Pizza> Pizzas { get; } = new()
{
    new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
    new Pizza { Id = 2, Name = "Veggie", IsGlutenFree = true },
    new Pizza { Id = 3, Name = "Meat Lovers", IsGlutenFree = false },
    new Pizza { Id = 4, Name = "Hawaiian", IsGlutenFree = false }   // added record
};
```

### GET /pizza — 200 OK

Request:
```
GET http://localhost:5140/pizza
```

Response:
```
STATUS: 200
[{"id":1,"name":"Classic Italian","isGlutenFree":false},
 {"id":2,"name":"Veggie","isGlutenFree":true},
 {"id":3,"name":"Meat Lovers","isGlutenFree":false},
 {"id":4,"name":"Hawaiian","isGlutenFree":false}]
```

### POST /pizza — 201 Created

Request:
```
POST http://localhost:5140/pizza
Content-Type: application/json

{"name":"Buffalo Chicken","isGlutenFree":false}
```

Response:
```
STATUS: 201
{"id":5,"name":"Buffalo Chicken","isGlutenFree":false}
```

### PUT /pizza/5 — 204 No Content

Request:
```
PUT http://localhost:5140/pizza/5
Content-Type: application/json

{"id":5,"name":"Buffalo Chicken","isGlutenFree":true}
```

Response:
```
STATUS: 204
(no body)
```

Verified with `GET /pizza/5` afterward → `STATUS: 200`, body `{"id":5,"name":"Buffalo Chicken","isGlutenFree":true}`.

### DELETE /pizza/5 — 204 No Content

Request:
```
DELETE http://localhost:5140/pizza/5
```

Response:
```
STATUS: 204
(no body)
```

Verified with final `GET /pizza` → back to the original 4 pizzas.

---

## Part 2: File Directory App — Sales Summary Report Function

```csharp
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
```

### Sample output (`salesTotalDir/SalesSummary.txt`)

```
Sales Summary
----------------------------
 Total Sales: $46,288.97

 Details:
  202/sales.json: $14,032.10
  305/sales.json: $9,871.55
  201/sales.json: $22,385.32
```
