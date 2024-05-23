using System;
using StackExchange.Redis;

namespace WebApi.Infrastructure.Database
{
    public class RedisContext : IDisposable
    {
        private static Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect("localhost"));

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return _lazyConnection.Value;
            }
        }

        // Flag to detect redundant calls
        private bool disposed = false;

        public RedisContext()
        {
            // Ensure the lazy connection is created
            var conn = Connection;
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                if (_lazyConnection.IsValueCreated)
                {
                    _lazyConnection.Value.Dispose();
                }
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        ~RedisContext()
        {
            Dispose(false);
        }
    }
}
