namespace ThatOneSamuraiGame.Scripts.Enumeration
{

    public class GameScenes : SmartEnum
    {
        
        #region - - - - - - Fields - - - - - -

        public static GameScenes PersistenceScene = new("PersistenceScene", 0);
        public static GameScenes MainGameScene = new("MainGameScene", 3);
        
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