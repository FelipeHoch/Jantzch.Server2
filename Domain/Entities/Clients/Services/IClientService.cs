namespace Jantzch.Server2.Domain.Entities.Clients.Services;

public interface IClientService
{
    Task<Localization> GetLocalization(Address address, bool isPrimary, string description);
}
