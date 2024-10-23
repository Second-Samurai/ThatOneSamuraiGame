namespace ThatOneSamuraiGame.Scripts.Enumeration
{

    public class GameScenes : SmartEnum
    {
        
        #region - - - - - - Fields - - - - - -

        public static GameScenes PersistenceScene = new("PersistenceScene", 0);
        public static GameScenes MainGameScene = new("MainGameScene", 1);
        
        // Does not correspond to scene list, but represents all debug scenes first opened.
        public static GameScenes Debug_Scene = new("Debug_Scene", 999);
        
        #endregion Fields
  
        #region - - - - - - Constructors - - - - - -

        private GameScenes(string name, int value) : base(name, value) { }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        public static implicit operator int(GameScenes gameScenes)
            => gameScenes.GetValue();

        public static implicit operator string(GameScenes gameScenes)
            => gameScenes.ToString();

        #endregion Methods
        
    }

}