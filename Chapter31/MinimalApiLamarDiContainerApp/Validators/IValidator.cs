namespace MinimalApiLamarDiContainerApp.Validators
{
    public interface IValidator<T>
    {
        bool Validate(T model);
    }
}
