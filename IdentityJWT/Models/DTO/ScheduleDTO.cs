﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PinPinServer.Models.DTO
{
    public class ScheduleDTO
    {
        public int Id { get; set; }

        [Display(Name = "行程名稱")]
        public string? Name { get; set; }

        [Display(Name = "出發日期")]
        public DateOnly StartTime { get; set; }

        [Display(Name = "結束日期")]
        public DateOnly EndTime { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Display(Name = "主辦人")]
        public string UserName { get; set; }

        [Display(Name = "創建日期")]
        public DateTime? CreatedAt { get; set; }

        public Decimal? lng { get; set; }

        public Decimal? lat { get; set; }

        public List<int?> SharedUserIDs { get; set; } = new List<int?>();
        [Display(Name = "群組成員")]
        public List<string?> SharedUserNames { get; set; } = new List<string?>();

    }
}