namespace MinimalApiLamarDiContainerApp.Validators
{
    public class DefaultValidator<T> : IValidator<T>
    {
        public bool Validate(T model) => true;
    }
}
