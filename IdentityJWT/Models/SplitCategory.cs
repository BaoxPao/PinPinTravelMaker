﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PinPinServer.Models;

public partial class SplitCategory
{
    public int Id { get; set; }

    public string Category { get; set; }

    public string Color { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<SplitExpense> SplitExpenses { get; set; } = new List<SplitExpense>();
}