using Newtonsoft.Json;
using SqlSugar;

namespace LibraryServices.Domain.Models.Package
{
    [SugarTable("library_packages")]
    public class Package : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "package_id")]
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [SugarColumn(IsNullable = false, ColumnName = "package_name")]
        public string? Name { get; set; }

        [JsonProperty("created")]
        [SugarColumn(ColumnName = "package_createdDate")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("latest_version_update")]
        [SugarColumn(ColumnName = "package_updateDate")]
        public DateTime UpdateTime { get; set; }

        [SugarColumn(IsNullable = true, Length = 65535, ColumnName = "package_description")]
        public string? Description { get; set; }

        [SugarColumn(IsNullable = true, Length = 10000, ColumnName = "package_license")]
        public string? License { get; set; }

        [SugarColumn(IsNullable = true, ColumnName = "package_engine")]
        public string? Engine { get; set; }

        [SugarColumn(ColumnName = "package_downloads")]
        public long Downloads { get; set; }

        [SugarColumn(ColumnName = "package_votes")]
        public long Votes { get; set; }

        [SugarColumn(IsNullable = true, ColumnName = "package_group")]
        public string? Group { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(PackageVersion.PackageId))]
        public List<PackageVersion>? Versions { get; set; }

        [SugarColumn(ColumnName = "package_isDeleted")]
        public bool IsDeleted { get; set; }
    }

    [SugarTable("library_package_versions")]
    public class PackageVersion : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, IsNullable = false, ColumnName = "version_version")]
        public string? Version { get; set; }

        [SugarColumn(IsPrimaryKey = true, IsNullable = false, ColumnName = "version_packageId")]
        public string? PackageId { get; set; }

        [SugarColumn(ColumnName = "package_url")]
        public string? Url { get; set; }

        [JsonProperty("created")]
        [SugarColumn(ColumnName = "package_createdDate")]
        public DateTime CreateTime { get; set; }

        [JsonProperty("scan_status")]
        [SugarColumn(IsNullable = true, ColumnName = "package_scanStatus")]
        public string? ScanStatus { get; set; }

        [SugarColumn(ColumnName = "version_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
