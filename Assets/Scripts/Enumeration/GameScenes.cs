namespace ThatOneSamuraiGame.Scripts.Enumeration
{

    public enum GameSceneEnum
    {
        Debug_AdditiveLoadingTest,
        Debug_AdditiveScene_1,
        Debug_AdditiveScene_2,
        Debug_AdditiveScene_3,
        Debug_AdditiveScene_4
    }

    public class GameScene : SmartEnum
    {
        
        #region - - - - - - Fields - - - - - -

        public static GameScene PersistenceScene = new("PersistenceScene", 0);
        public static GameScene MainGameScene = new("MainGameScene", 1);
        
        // Does not correspond to scene list, but represents all debug scenes first opened.
        public static GameScene Debug_Scene = new("Debug_Scene", 999);
        public static GameScene Debug_AdditiveLoadingTest = new("Debug_AdditiveLoadingTest", 8);
        public static GameScene Debug_AdditiveScene_1 = new("999_Additive_1", 9);
        public static GameScene Debug_AdditiveScene_2 = new("999_Additive_2", 10);
        public static GameScene Debug_AdditiveScene_3 = new("999_Additive_3", 11);
        
        #endregion Fields
  
        #region - - - - - - Constructors - - - - - -

        private GameScene(string name, int value) : base(name, value) { }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        public static implicit operator int(GameScene gameScenes)
            => gameScenes.GetValue();

        public static implicit operator string(GameScene gameScenes)
            => gameScenes.ToString();

        public static implicit operator GameScene(GameSceneEnum gameSceneEnum)
        {
            if (gameSceneEnum == GameSceneEnum.Debug_AdditiveLoadingTest)
                return Debug_AdditiveLoadingTest;
            if (gameSceneEnum == GameSceneEnum.Debug_AdditiveScene_1)
                return Debug_AdditiveScene_1;
            if (gameSceneEnum == GameSceneEnum.Debug_AdditiveScene_2)
                return Debug_AdditiveScene_2; 
            if (gameSceneEnum == GameSceneEnum.Debug_AdditiveScene_3)
                return Debug_AdditiveScene_3;

            return MainGameScene; // Default scene
        }

        #endregion Methods
        
    }

}