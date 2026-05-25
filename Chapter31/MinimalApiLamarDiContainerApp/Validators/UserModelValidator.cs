using MinimalApiLamarDiContainerApp.Models;

namespace MinimalApiLamarDiContainerApp.Validators
{
    public class UserModelValidator : IValidator<UserModel>
    {
        public bool Validate(UserModel model) => model.Age > 18;
    }
}
