using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Models;

namespace ReactMaterialUIShowcaseApi.Mappers
{
    public static class UserMappers
    {
        public static UserDto ToDto(this AppUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return new UserDto
            {
                GivenName = user.GivenName,
                Surname = user.Surname,
                BusinessRoleName = user.BusinessRoleName,
                OrganizationName = user.OrganizationName,
                Language = user.Language
            };
        }

        public static AppUser ToEntity(this UserDto userDto)
        {
            if (userDto == null) throw new ArgumentNullException(nameof(userDto));

            return new AppUser
            {
                GivenName = userDto.GivenName,
                Surname = userDto.Surname,
                BusinessRoleName = userDto.BusinessRoleName,
                OrganizationName = userDto.OrganizationName,
                Language = userDto.Language
            };
        }
    }
}
