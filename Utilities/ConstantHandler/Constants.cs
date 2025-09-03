namespace Common.ConstantHandler
{
    public class Constants
    {
        #region  Module/Entity Name
        public const string USER = "User";
        public const string CAR = "Car";
        public const string MANUFACTURER = "Manufacturer";

        public const string DEFAULT_ENTITY = "Entity";
        #endregion

        #region Configuration Strings
        public const string DATABASE_DEFAULT_CONNECTION = "DefaultConnection";
        public const string AES_ENCRYPTION_KEY = "AesEncryption:Key";
        public const string AES_ENCRYPTION_IV = "AesEncryption:IV";
        #endregion
        #region Swagger JWT Auth
        public const string SWAGGER_SECURITY_SCHEME = "Bearer";
        public const string SWAGGER_SECURITY_DESCRIPTION = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'";
        public const string SWAGGER_SECURITY_NAME = "Authorization";
        public const string SWAGGER_SECURITY_SCHEME_TYPE = "bearer";
        public const string SWAGGER_SECURITY_BEARER_FORMAT = "JWT";
        #endregion
    }
}
