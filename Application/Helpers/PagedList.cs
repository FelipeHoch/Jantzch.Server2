﻿using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Jantzch.Server2.Application.Helpers;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => (CurrentPage > 1);
    public bool HasNext => (CurrentPage < TotalPages);

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public static async Task<PagedList<T>> CreateAsync(
    IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }

    public static async Task<PagedList<T>> CreateAsync(
         IAggregateFluent<T> source, int pageNumber, int pageSize, int count, CancellationToken cancellationToken)
    {
        var items = await source.Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize).ToListAsync(cancellationToken);

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}

