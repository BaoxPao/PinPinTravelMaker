﻿using System.ComponentModel.DataAnnotations;

namespace PinPinServer.Controllers
{
    public class SendMessageDTO
    {
        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
