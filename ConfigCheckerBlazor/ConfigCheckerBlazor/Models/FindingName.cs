namespace ConfigCheckerBlazor.Models
{
    public enum FindingName
    {
        Invalid = 0,
        OpenRcePort,
        OpenPort,
        WeakPassword,
        PasswordStoredInConfig,
        EncryptionDisabled,
        MfaDisabled
    }
}
