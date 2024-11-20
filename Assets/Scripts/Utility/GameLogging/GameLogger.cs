using UnityEngine;

namespace ThatOneSamuraiGame.GameLogging
{

    public static class GameLogger
    {

        #region - - - - - - Methods - - - - - -

        public static void Log(string message)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[LOG]: {message}");
            #endif
        }

        public static void LogWarning(string message)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning($"[WARNING]: {message}");
            #endif
        }
        
        public static void LogError(string message)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError($"[ERROR]: {message}");
            #endif
        }

        #endregion Methods
  
    }

}