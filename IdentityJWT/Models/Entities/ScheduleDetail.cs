﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PinPinServer.Models;

public partial class ScheduleDetail
{
    public int Id { get; set; }

    public int ScheduleDayId { get; set; }

    public int UserId { get; set; }

    public string LocationName { get; set; }

    public string Location { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string Remark { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? ModifiedTime { get; set; }

    public virtual SchedulePreview ScheduleDay { get; set; }

    public virtual ICollection<Transportation> Transportations { get; set; } = new List<Transportation>();

    public virtual User User { get; set; }
}