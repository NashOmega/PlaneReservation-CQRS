using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class ServiceBase<T>
    {
        protected readonly IMapper _mapper;
        public readonly ILogger<T> _logger;

        public ServiceBase(IMapper mapper, ILoggerFactory factory)
        {
            _mapper = mapper;
            _logger = factory.CreateLogger<T>();
        }
    }
}
