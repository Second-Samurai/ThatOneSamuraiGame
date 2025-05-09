namespace ThatOneSamuraiGame.Scripts.Enumeration
{

    /// <summary>
    /// Represents the different integer layers in unity.
    /// </summary>
    public class GameLayer : SmartEnum
    {

        #region - - - - - - Fields - - - - - -

        public static GameLayer Ignore = new GameLayer("Ignore", 7);
        public static GameLayer Player = new GameLayer("Player", 9);
        public static GameLayer Enemy = new GameLayer("Enemy", 10);

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public GameLayer(string name, int value) : base(name, value) { }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        public static implicit operator int(GameLayer gameLayer)
            => gameLayer.GetValue();

        public static implicit operator string(GameLayer gameLayer)
            => gameLayer.ToString();

        #endregion Methods
  
    }

}