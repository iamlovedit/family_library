﻿namespace LibraryServices.Domain.DataTransferObjects.Identity
{
    public class UserCreationDTO
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public int? VaildCode { get; set; }
    }
}
