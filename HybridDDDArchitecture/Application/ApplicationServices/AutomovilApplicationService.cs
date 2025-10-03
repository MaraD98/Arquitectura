using Application.Repositories;

namespace Application.ApplicationServices
{
    public class AutomovilApplicationService : IAutomovilApplicationService
    {
        private readonly IAutomovilRepository _automovilRepository;

        public AutomovilApplicationService(IAutomovilRepository automovilRepository)
        {
            _automovilRepository = automovilRepository ?? throw new ArgumentNullException(nameof(automovilRepository));
        }

        public bool AutomovilExist(string numeroChasis)
        {
            if (string.IsNullOrWhiteSpace(numeroChasis))
                throw new ArgumentNullException(nameof(numeroChasis));

            return _automovilRepository.Query()
                .Any(a => a.NumeroChasis == numeroChasis);
        }
    }
}
