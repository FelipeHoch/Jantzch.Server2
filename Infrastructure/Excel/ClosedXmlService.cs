using ClosedXML.Excel;
using Jantzch.Server2.Application.Abstractions.Excel;
using Jantzch.Server2.Domain.Entities.Clients;

namespace Jantzch.Server2.Infrastructure.Excel;

public class ClosedXmlService : IExcelService
{
    public IEnumerable <ImportDeal> ReadExcel(Stream stream)
    {
        var deals = new List<ImportDeal>();

        using var workbook = new XLWorkbook(stream);

        var worksheet = workbook.Worksheet(1);

        // skip first row
        var rows = worksheet.RowsUsed().Skip(1);

        foreach (var row in rows)
        {

            var deal = new ImportDeal
            {
                Type = row.Cell("J").Value.ToString().Trim(),
                Description = row.Cell("K").Value.ToString().Trim(),
                Value = row.Cell("L").GetDouble(),
                InstalationType = row.Cell("M").Value.ToString().Trim(),
                StructureType = row.Cell("N").Value.ToString().Trim(),
                Address = new Address
                {
                    Street = row.Cell("O").Value.ToString().Trim(),
                    City = row.Cell("S").Value.ToString().Trim(),
                    State = row.Cell("T").Value.ToString().Trim(),
                    District = row.Cell("Q").Value.ToString().Trim(),
                    StreetNumber = int.TryParse(row.Cell("P").Value.ToString(), out int streetNumber) ? streetNumber : 0
                },
                Phase = row.Cell("V").Value.ToString().Trim(),
                IntegrationId = row.Cell("A").Value.ToString(),
                CreatedBy = row.Cell("G").Value.ToString().Trim(),
                CEP = row.Cell("U").Value.ToString(),
                Email = row.Cell("F").Value.ToString().Trim(),
                PhoneNumber = row.Cell("D").Value.ToString().Trim(),
                ClientName = row.Cell("C").Value.ToString().Trim(),
                CreatedAt = row.Cell("H").GetDateTime(),
                DealConfirmedAt = row.Cell("I").GetDateTime()
            };

            deal.PhoneNumber = PhoneNumberWithNoMask(deal.PhoneNumber);

            deals.Add(deal);
        }

        return deals;
    }

    private static string PhoneNumberWithNoMask(string phoneNumber)
    {
        return phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
    }
}
