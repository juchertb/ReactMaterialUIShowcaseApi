using System;
using System.ComponentModel.DataAnnotations;
using ReactMaterialUIShowcaseApi.Enumerations;

public class ValidLanguageAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return false; // Not set
        if (value is LanguageEnum lang)
        {
            // Only allow defined enum values (excluding iNone if desired)
            return Enum.IsDefined(typeof(LanguageEnum), lang);
        }
        return false;
    }
}