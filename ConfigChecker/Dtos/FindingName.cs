namespace ConfigChecker.Dtos
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
