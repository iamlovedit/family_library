using LibraryServices.Domain.DataTransferObjects.FamilyParameter;

namespace LibraryServices.Domain.DataTransferObjects.FamilyLibrary
{
    public class FamilySymbolDTO
    {
        public string? Name { get; set; }

        public List<ParameterDTO>? Parameters { get; set; }
    }
}
