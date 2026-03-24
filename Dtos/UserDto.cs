using ReactMaterialUIShowcaseApi.Enumerations;

namespace ReactMaterialUIShowcaseApi.Dtos
{
    public class UserDto
    {
        public string GivenName { get; set; }= string.Empty;
        public string Surname { get; set; }= string.Empty;
        public string? BusinessRoleName { get; set; }= string.Empty;
        public string? OrganizationName { get; set; }= string.Empty;
        public LanguageEnum Language { get; set; } = LanguageEnum.iEnglish;
    }
   }