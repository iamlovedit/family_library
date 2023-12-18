using Newtonsoft.Json;
using SqlSugar;

namespace LibraryServices.Domain.DataTransferObjects.FamilyParameter
{
    public class UnitTypeDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }
    }
}

