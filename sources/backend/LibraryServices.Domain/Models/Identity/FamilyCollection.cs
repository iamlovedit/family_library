using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryServices.Domain.Models.Identity
{
    [SugarTable(TableName = "family_userCollections")]
    public class FamilyCollection : IDeletable
    {
        [SugarColumn(ColumnName = "collection_familyId", IsPrimaryKey = true)]
        public long FamilyId { get; set; }

        [SugarColumn(ColumnName = "collection_userId", IsPrimaryKey = true)]
        public long UserId { get; set; }

        [SugarColumn(ColumnName = "collection_createTime")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "collection_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
