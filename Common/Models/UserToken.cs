﻿using System;

namespace Common.Models
{
    public class UserToken
    {
        public string Purpose { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public bool Expired => Expires < DateTime.Now;
    }
}