using ThatOneSamuraiGame.GameLogging;

public static class GameValidator
{

    #region - - - - - - Methods - - - - - -

    public static bool NotNull(object value, string objectName, string sourceObjectName = "")
    {
        if (value != null) return true;

        GameLogger.LogError($"'{objectName}' is found to be null. Please properly set its value. " +
                            $"{(string.IsNullOrEmpty(sourceObjectName) ? $"[{sourceObjectName}]" : string.Empty)}");
        return false;
    }

    #endregion Methods
  
}