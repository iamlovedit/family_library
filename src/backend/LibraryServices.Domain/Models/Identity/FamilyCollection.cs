using SqlSugar;

namespace LibraryServices.Domain.Models.Identity
{
    [SugarTable(TableName = "family_userCollections")]
    public class FamilyCollection : IDeletable
    {
        [SugarColumn(ColumnName = "collection_familyId", IsPrimaryKey = true)]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "collection_userId", IsPrimaryKey = true)]
        public long UserId { get; set; }

        [SugarColumn(ColumnName = "collection_createDate")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "collection_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
