namespace Jantzch.Server2.Application.Abstractions.Excel;

public interface IExcelService
{
    IEnumerable<ImportDeal> ReadExcel(Stream stream);
}
