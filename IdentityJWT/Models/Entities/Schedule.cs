﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PinPinServer.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateOnly StartTime { get; set; }

    public DateOnly EndTime { get; set; }

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public decimal? Lng { get; set; }

    public decimal? Lat { get; set; }

    public virtual ICollection<ChatroomChat> ChatroomChats { get; set; } = new List<ChatroomChat>();

    public virtual ICollection<ScheduleAuthority> ScheduleAuthorities { get; set; } = new List<ScheduleAuthority>();

    public virtual ICollection<ScheduleGroup> ScheduleGroups { get; set; } = new List<ScheduleGroup>();

    public virtual ICollection<SchedulePreview> SchedulePreviews { get; set; } = new List<SchedulePreview>();

    public virtual ICollection<SplitExpense> SplitExpenses { get; set; } = new List<SplitExpense>();

    public virtual User User { get; set; }

    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}