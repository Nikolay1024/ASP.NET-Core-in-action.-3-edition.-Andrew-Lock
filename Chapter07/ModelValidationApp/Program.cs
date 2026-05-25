using System.ComponentModel.DataAnnotations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

// Для работы требуется NuGet MinimalApis.Extensions.
// Метод WithParameterValidation() добавляет фильтр валидации в конечную точку, который проверяет соответствие
// свойств модели указанным атрибутам DataAnnotation.
app.MapPost("/example1/user", (UserModel1 model) => model.ToString()).WithParameterValidation();

// Чтобы добавить валидацию простых типов с помощью MinimalApis.Extensions, необходимо создать тип и
// использовать атрибут [AsParameters].
app.MapPost("/example2/user/{id}", ([AsParameters] UserModel2 model) => $"Получено {model.Id}.")
    .WithParameterValidation();

app.Run();


// Тип UserModel реализует интерфейс IValidatableObject с методом Validate().
record UserModel1 : IValidatableObject
{
    [Required, StringLength(100), Display(Name = "Имя")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100), Display(Name = "Фамилия")]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress] // Проверяет, что Email соответствует формату адреса электронной почты.
    [Display(Name = "Почтовый адрес")]
    public string? Email { get; set; }

    [Phone] // Проверяет, что PhoneNumber соответствует формату номера телефона.
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }

    // Метод проверяет, чтобы было заполнено либо свойство Email, либо PhoneNumber, иначе возвращает ошибку.
    // Выполняется только в том случае, если соблюдены все условия атрибутов DataAnnotation.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(PhoneNumber))
        {
            yield return new ValidationResult(
                "Вам необходимо указать адрес электронной почты или номер телефона.",
                new[] { nameof(Email), nameof(PhoneNumber) });
        }
    }
}

struct UserModel2
{
    [Range(1, 10)]
    public int Id { get; set; }
}
