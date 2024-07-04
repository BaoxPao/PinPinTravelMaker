﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PinPinServer.Models;

public partial class SplitExpense
{
    public int Id { get; set; }

    public int ScheduleId { get; set; }

    public int PayerId { get; set; }

    public int SplitCategoryId { get; set; }

    public string Name { get; set; }

    public int CurrencyId { get; set; }

    public decimal Amount { get; set; }

    public string Remark { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CostCurrencyCategory Currency { get; set; }

    public virtual User Payer { get; set; }

    public virtual Schedule Schedule { get; set; }

    public virtual SplitCategory SplitCategory { get; set; }

    public virtual ICollection<SplitExpenseParticipant> SplitExpenseParticipants { get; set; } = new List<SplitExpenseParticipant>();
}