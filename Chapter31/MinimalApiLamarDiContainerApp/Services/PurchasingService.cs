using MinimalApiLamarDiContainerApp.Models;
using MinimalApiLamarDiContainerApp.Validators;

namespace MinimalApiLamarDiContainerApp.Services
{
    public class PurchasingService : IPurchasingService
    {
        private readonly IValidator<UserModel> _validator;

        public PurchasingService(IValidator<UserModel> validator) => _validator = validator;
    }
}
