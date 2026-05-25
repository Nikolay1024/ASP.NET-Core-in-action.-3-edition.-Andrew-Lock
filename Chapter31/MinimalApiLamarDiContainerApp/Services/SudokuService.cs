using MinimalApiLamarDiContainerApp.Models;
using MinimalApiLamarDiContainerApp.Validators;

namespace MinimalApiLamarDiContainerApp.Services
{
    public class SudokuService : IGamingService
    {
        private readonly IValidator<AvatarModel> _validator;
        
        public SudokuService(IValidator<AvatarModel> validator) => _validator = validator;
    }
}
