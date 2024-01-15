namespace Jantzch.Server2.Application.Services.PropertyChecker;

public interface IPropertyCheckerService
{
    bool TypeHasProperties<T>(string? fields);
}
