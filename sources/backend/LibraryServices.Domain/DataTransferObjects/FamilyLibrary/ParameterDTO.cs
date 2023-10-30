﻿using SqlSugar;
using System.Text.Json.Serialization;

namespace LibraryServices.Domain.DataTransferObjects.FamilyLibrary
{
    public class ParameterDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }

        public DisplayUnitTypeDTO? DisplayUnitType { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public StorageType StorageType { get; set; }

        public bool UserModifiable { get; set; }

        public long DefinitionId { get; set; }

    }
}
