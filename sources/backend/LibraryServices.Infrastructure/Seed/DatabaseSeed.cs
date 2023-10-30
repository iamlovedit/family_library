using Newtonsoft.Json;
using SqlSugar;
using System.Reflection;
using System.Text;

namespace LibraryServices.Infrastructure.Seed
{
    public class DatabaseSeed
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseSeed(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void InitTablesByClass<T>() where T : class, new()
        {
            if (_databaseContext.DbType == DbType.Oracle)
            {
                throw new InvalidOperationException("暂不支持Oracle数据库");
            }
            else
            {
                _databaseContext.Database.DbMaintenance.CreateDatabase();
            }
            try
            {
                var modelType = typeof(T);
                var types = modelType.Assembly.DefinedTypes.
                 Where(ti => ti.Namespace == modelType.Namespace && ti.IsClass && ti.GetCustomAttribute<SugarTable>() != null).
                 Select(ti => ti.AsType());

                foreach (var type in types)
                {
                    var tableName = type.GetCustomAttribute<SugarTable>()?.TableName ?? type.Name;
                    Console.WriteLine($"table is initializing: {tableName}");
                    _databaseContext.Database.CodeFirst.InitTables(type);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }


        public async void InitSeed<T>(string seedFile) where T : class, new()
        {
            var setting = new JsonSerializerSettings();
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                setting.NullValueHandling = NullValueHandling.Ignore;
                return setting;
            });
            if (string.IsNullOrEmpty(seedFile))
            {

                throw new ArgumentException("Value cannot be null or empty.", nameof(seedFile));
            }
            try
            {
                if (await _databaseContext.Database.Queryable<T>().AnyAsync())
                {
                    return;
                }

                var json = await File.ReadAllTextAsync(seedFile, Encoding.UTF8);
                if (string.IsNullOrEmpty(json))
                {
                    return;
                }

                var data = JsonConvert.DeserializeObject<List<T>>(json);
                await _databaseContext.GetEntityDB<T>().InsertRangeAsync(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}