using ReactMaterialUIShowcaseApi.Enumerations;
using ReactMaterialUIShowcaseApi.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class LoginDto
    {
        [Required]
        [DefaultValue("")]
        [MinLength(8, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "UsernameMinLength")]
        [MaxLength(30, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "UsernameMaxLength")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DefaultValue("")]
        [MinLength(8, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "PasswordMinLength")]
        [MaxLength(50, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "PasswordMaxLength")]
        public string Password { get; set; } = string.Empty;

        [ValidLanguage(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "LanguageInvalid")]
        public LanguageEnum Language { get; set; } = LanguageEnum.iEnglish;
    }
}