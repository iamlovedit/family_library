using LibraryServices.Domain.Models.Dynamo;
using LibraryServices.Infrastructure.RedisCache;
using LibraryServices.Infrastructure.Repository;
using LibraryServices.Infrastructure.Seed;
using Newtonsoft.Json.Linq;
using Quartz;
namespace LibraryServices.PackageService.Jobs
{
    internal class FetchPackagesJob : IJob
    {
        private readonly DatabaseContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisBasketRepository _redis;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FetchPackagesJob> _logger;

        public FetchPackagesJob(DatabaseContext dbContext, IUnitOfWork unitOfWork, IRedisBasketRepository redis,
            IHttpClientFactory httpClientFactory, ILogger<FetchPackagesJob> logger)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _redis = redis;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var responseMessage = await httpClient.GetAsync("https://dynamopackages.com/packages");
                responseMessage.EnsureSuccessStatusCode();
                var json = await responseMessage.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    var jObject = JObject.Parse(json);
                    var content = jObject["content"];
                    if (content is null)
                    {
                        _logger.LogWarning("fetch packages failed,content is empty");
                        return;
                    }
                    var newPackages = content.ToObject<List<Package>>()!;
                    var packageDb = _dbContext.GetEntityDB<Package>();
                    var packageVersionDb = _dbContext.GetEntityDB<PackageVersion>();
                    var oldPackages = await packageDb.GetListAsync();
                    var oldPackageVersions = await packageVersionDb.GetListAsync();

                    _unitOfWork.BeginTransaction();
                    var addedPackages = new List<Package>();
                    var addedPackageVersions = new List<PackageVersion>();
                    var newPackageVersions = new List<PackageVersion>();
                    foreach (var package in newPackages)
                    {
                        var oldPackage = oldPackages.FirstOrDefault(p => p.Id == package.Id);
                        if (oldPackage is null)
                        {
                            addedPackages.Add(package);
                        }
                        else
                        {
                            await packageDb.UpdateAsync(package);
                        }

                        foreach (var pVersion in package.Versions!)
                        {
                            pVersion.PackageId = package.Id;
                            var oldPackageVersion = oldPackageVersions.FirstOrDefault(pv =>
                                pv.PackageId == package.Id && pv.Version == pVersion.Version);
                            if (oldPackageVersion is null)
                            {
                                addedPackageVersions.Add(pVersion);
                            }
                            else
                            {
                                await packageVersionDb.UpdateAsync(pVersion);
                            }

                            newPackageVersions.Add(pVersion);
                        }
                    }

                    await packageDb.InsertRangeAsync(addedPackages);
                    await packageVersionDb.InsertRangeAsync(addedPackageVersions);

                    foreach (var package in oldPackages)
                    {
                        var newPackage = newPackages.FirstOrDefault(p => p.Id == package.Id);
                        if (newPackage is null)
                        {
                            package.IsDeleted = true;
                            await packageDb.UpdateAsync(package);
                        }
                    }

                    foreach (var pVersion in oldPackageVersions)
                    {
                        var newVersion = newPackageVersions.FirstOrDefault(pv =>
                            pv.PackageId == pVersion.PackageId && pv.Version == pVersion.Version);
                        if (newVersion is null)
                        {
                            pVersion.IsDeleted = true;
                            await packageVersionDb.UpdateAsync(pVersion);
                        }
                    }
                    _logger.LogInformation(
                           "update succeed,added new packages count {added},added new versions count {addedverson}",
                           addedPackages.Count, addedPackageVersions.Count);
                    _unitOfWork.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTransaction();
                _logger.LogError(e, e.Message);
            }
        }
    }
}
