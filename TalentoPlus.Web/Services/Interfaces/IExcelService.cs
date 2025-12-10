namespace TalentoPlus.Web.Services.Interfaces;

public interface IExcelService
{
    Task ProcessEmployeeFileAsync(Stream fileStream);
}
