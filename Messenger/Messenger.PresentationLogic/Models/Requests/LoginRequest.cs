﻿using System.ComponentModel.DataAnnotations;

namespace Messenger.PresentationLogic.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}