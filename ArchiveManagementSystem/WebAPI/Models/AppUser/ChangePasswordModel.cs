﻿namespace WebAPI.Models.AppUser
{
    public class ChangePasswordModel
    {
        public int Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
