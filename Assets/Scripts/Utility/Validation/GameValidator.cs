using ThatOneSamuraiGame.GameLogging;

public static class GameValidator
{

    #region - - - - - - Methods - - - - - -

    public static bool NotNull(object value, string objectName)
    {
        if (value != null) return true;
        
        GameLogger.LogError($"'{objectName}' is found to be null. Please properly set its value.");
        return false;
    }

    #endregion Methods
  
}