using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Probotbor.Core.Models.Communication
{
    public class CommSettingsOptions : IConfigureOptions<CommSettings>
    {
        private readonly IConfiguration _configuration;
        private const string SectionName = "Comm";

        public CommSettingsOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(CommSettings options)
        {
            _configuration.GetSection(SectionName).Bind(options);
            
        }
    }
}
