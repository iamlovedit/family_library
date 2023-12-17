namespace LibraryServices.Infrastructure.Consul
{
    internal class ConsulOption
    {
        /// <summary>
        /// service ip
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// service name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// consule client address
        /// </summary>
        public string? ConsulAddress { get; set; }

        /// <summary>
        /// health check route
        /// </summary>
        public string? HealthRoute { get; set; }
    }
}
