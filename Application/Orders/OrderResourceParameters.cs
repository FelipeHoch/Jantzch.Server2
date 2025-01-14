﻿using Jantzch.Server2.Application.Shared;

namespace Jantzch.Server2.Application.Orders;

public class OrderResourceParameters : ResourceParameters
{
    public string? CreatedBy { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
    public string? Client { get; set; }
    public string? Types { get; set; }
    public string? DealId { get; set; }
}
