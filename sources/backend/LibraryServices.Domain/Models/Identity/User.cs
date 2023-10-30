using SqlSugar;

namespace LibraryServices.Domain.Models.Identity
{
    [SugarTable("library_users")]
    public class User : IDeletable
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "user_id")]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "user_username")]
        public string? Username { get; set; }

        [SugarColumn(ColumnName = "user_nickname")]
        public string? Nickname { get; set; }

        [SugarColumn(ColumnName = "user_password")]
        public string? Password { get; set; }

        [SugarColumn(ColumnName = "user_email")]
        public string? Email { get; set; }

        [SugarColumn(ColumnName = "user_salt")]
        public string? Salt { get; set; }

        [Navigate(typeof(UserRole), nameof(UserRole.UserId), nameof(UserRole.RoleId))]
        public List<Role>? Roles { get; set; }

        [SugarColumn(ColumnName = "user_createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "user_lastLoginDate")]
        public DateTime LastLoginDate { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "user_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
