namespace ReactMaterialUIShowcaseApi.Models
{
    public class AppSettings
    {
        public string RpmsWebDbConnection { get; set; } = string.Empty;
        public string RpmsWebEnvironment { get; set; } = string.Empty;
        public string OracleTnsNamesAndSqlNetLocationShort { get; set; } = string.Empty;
        public string OracleTnsNamesAndSqlNetLocation { get; set; } = string.Empty;
        public string XorEncryptionSalt { get; set; } = string.Empty;
    }
}
