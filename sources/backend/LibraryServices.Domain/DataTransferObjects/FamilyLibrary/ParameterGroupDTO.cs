using Newtonsoft.Json;
using SqlSugar;

namespace LibraryServices.Domain.DataTransferObjects.FamilyParameter
{
    public class ParameterGroupDTO
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }
    }
}
