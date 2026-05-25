using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesTagHelpers2App.BindingModels
{
    public class BindingModel
    {
        public bool Boolean { get; set; } = true;
        public byte Byte { get; set; } = byte.MaxValue;
        public int Int { get; set; } = int.MaxValue;
        public float Float { get; set; } = 1.23f;
        public double Double { get; set; } = 1.23;
        public decimal Decimal { get; set; } = 1.23m;
        public decimal FormattedInViewDecimal { get; set; } = 1.23m;
        [DisplayFormat(DataFormatString = "{0:f4}", ApplyFormatInEditMode = true)]
        public decimal FormattedInModelDecimal { get; set; } = 1.23m;
        [StringLength(60, MinimumLength = 3)]
        public string String { get; set; } = "Какой-то текст.";
        [StringLength(60, MinimumLength = 3)]
        public string Multiline { get; set; } = "Это многострочный,\nтекст\nдля тега <textarea/>.";


        [Phone, Display(Name = "Номер телефона")]
        [RegularExpression(@"^\+7\([0-9]{3}\)[0-9]{3}\-[0-9]{2}\-[0-9]{2}$",
            ErrorMessage = "Номер телефона должен соответствовать формату +7(999)999-99-99")]
        public string Phone { get; set; } = "+7(999)999-99-99";
        [EmailAddress, Display(Name = "Почтовый адрес")]
        public string Email { get; set; } = "test@example.com";
        [Url, Display(Name = "URL-адрес")]
        public string Url { get; set; } = "https://andrewlock.net";
        [HiddenInput, Display(Name = "Скрытое поле")]
        public string HiddenInput { get; set; } = "HiddenInput";


        [DataType(DataType.Password), Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.DateTime), Display(Name = "Дата время")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; } = DateTime.Now;
        [DataType(DataType.Date), Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.Now;
        [DataType(DataType.Time), Display(Name = "Время")]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; } = DateTime.Now;

        [DataType(DataType.Currency), Display(Name = "Валюта")]
        [Range(0, 9_999_999_999.99), Column(TypeName = "decimal(12, 2)")]
        [RegularExpression(@"^\d+\,\d{2}$", ErrorMessage = "Валюта должна соответствовать формату 0,00")]
        public decimal Currency { get; set; } = 9_999_999_999.99m;
    }
}
