namespace LibraryServices.Infrastructure.Consul
{
    internal class ConsulOption
    {
        /// <summary>
        /// service ip
        /// </summary>
        public string? IP { get; set; }

        /// <summary>
        /// service port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// service name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// consule client address
        /// </summary>
        public string? ConsulAddress { get; set; }

        /// <summary>
        /// consule client prot
        /// </summary>
        public int ConsulPort { get; set; }

    }
}
