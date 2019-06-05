using Microsoft.Extensions.Configuration;

namespace Punctual.Internal
{
    internal class PunctualConfiguration
    {
        public IConfiguration Configuration { get; }

        public PunctualConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
