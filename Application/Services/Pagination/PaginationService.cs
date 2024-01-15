using Jantzch.Server2.Application.Helpers;
using System.Text.Json;

namespace Jantzch.Server2.Application.Services.Pagination;

public interface IPaginationService
{
    void AddPaginationMetadataInResponseHeader<T>(PagedList<T> orders, HttpResponse response);
}

public class PaginationService : IPaginationService
{
    public void AddPaginationMetadataInResponseHeader<T>(PagedList<T> orders, HttpResponse response)
    {
        var paginationMetadata = new
        {
            totalCount = orders.TotalCount,
            pageSize = orders.PageSize,
            currentPage = orders.CurrentPage,
            totalPages = orders.TotalPages
        };

        response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    }
}
