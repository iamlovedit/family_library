﻿namespace LibraryServices.Domain.DataTransferObjects.Dynamo
{
    public class PackageVersionDTO
    {
        public string? Version { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}