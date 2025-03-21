using Shippo;
using Shippo.Models.Components;
using Shippo.Models.Requests;

var shippo = new ShippoSDK("YOUR API KEY HERE");
var shipment = await shippo.Shipments.CreateAsync(new ShipmentCreateRequest{
  AddressFrom = AddressFrom.CreateAddressCreateRequest(new AddressCreateRequest
  {
      Name = "Shawn Ippotle",
      Street1 = "215 Clayton St.",
      City = "San Francisco",
      State = "CA",
      Zip = "94117",
      Country = "US",
      Phone = "+1 555 341 9393",
      Email = "test@gmail.com"
  }),
  AddressTo = AddressTo.CreateAddressCreateRequest(new AddressCreateRequest
  {
      Name = "Mr Hippo",
      Street1 = "1092 Indian Summer Ct",
      City = "San Jose",
      State = "CA",
      Zip = "95122",
      Country = "US",
      Phone = "+1 555 341 9393",
      Email = "test@gmail.com"
  }),
  Parcels = new List<Shippo.Models.Components.Parcels>{
    Shippo.Models.Components.Parcels.CreateParcelCreateRequest(new ParcelCreateRequest {
      MassUnit = WeightUnitEnum.Kg,
      Weight = "0.2",
      DistanceUnit = DistanceUnitEnum.Cm,
      Height = "15.0",
      Width = "15.0",
      Length = "15.0"
    })
  }
});
Console.WriteLine($"Shipment {shipment.ObjectId} created.");
var rates = await shippo.Rates.ListShipmentRatesAsync(new Shippo.Models.Requests.ListShipmentRatesRequest
  {
    ShipmentId = shipment.ObjectId,
    Page = 1,
    Results = 10
  });

foreach (var rate in rates.Results) {
  Console.WriteLine($"{rate.Provider} {rate.Servicelevel.Name}, Arrives in {rate.EstimatedDays} days: {rate.AmountLocal}{rate.CurrencyLocal}");
}
var selectedRate = rates.Results.First(r => Convert.ToDouble(r.AmountLocal) < 5);
var transaction = await shippo.Transactions.CreateAsync(
  new CreateTransactionRequestBody(CreateTransactionRequestBodyType.TransactionCreateRequest)
  {
    TransactionCreateRequest = new TransactionCreateRequest
    {
      Rate = selectedRate.ObjectId,
      LabelFileType = LabelFileTypeEnum.PdfA4
    }
});
Console.WriteLine($"Transaction {transaction.ObjectId} created.");

if (transaction.Status == TransactionStatusEnum.Queued) {
  Console.WriteLine("Waiting for transaction to complete...");
  while (transaction.Status == TransactionStatusEnum.Queued) {
    transaction = await shippo.Transactions.GetAsync(transaction.ObjectId);
  }
}

if (transaction.Status == TransactionStatusEnum.Success)
{
  var fileName = $"label_{transaction.ObjectId}.pdf";
  Console.WriteLine($"Downloading label to {fileName}");
  var labelStream = await new HttpClient().GetStreamAsync(transaction.LabelUrl);
  var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
  using (var fileStream = File.Create(Path.Combine(downloadsPath, fileName)))
  {
    await labelStream.CopyToAsync(fileStream);
  }
}