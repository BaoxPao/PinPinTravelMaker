﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PinPinServer.Models;

public partial class ScheduleGroup
{
    public int Id { get; set; }

    public int ScheduleId { get; set; }

    public int UserId { get; set; }

    public DateTime? JoinedDate { get; set; }

    public DateTime? LeftDate { get; set; }

    public bool IsHoster { get; set; }

    public virtual Schedule Schedule { get; set; }

    public virtual User User { get; set; }
}