using System.ComponentModel.DataAnnotations;

namespace MinimalApiConfigureOptionsApp.Options
{
    public class SlackApiOptions
    {
        [Required, Url]
        public required string WebhookUrl { get; set; }
        [Required]
        public required string DisplayName { get; set; }
        public bool ShouldNotify { get; set; }
    }
}
