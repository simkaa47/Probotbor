using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Probotbor.Core.Models.Probotbor
{
    public class ProbotborSettingsOptions : IConfigureOptions<ProbotborSettings>
    {
        private readonly IConfiguration _configuration;
        private const string SectionName = "System";
        public ProbotborSettingsOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(ProbotborSettings options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
