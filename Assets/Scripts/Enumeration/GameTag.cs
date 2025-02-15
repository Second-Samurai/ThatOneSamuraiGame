using ThatOneSamuraiGame.Scripts.Enumeration;

namespace ThatOneSamuraiGame
{

    public class GameTag : SmartEnum
    {

        #region - - - - - - Fields - - - - - -

        public static GameTag Player = new("Player", 0);

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public GameTag(string name, int value) : base(name, value) { }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        public static implicit operator int(GameTag gameTag)
            => gameTag.GetValue();

        public static implicit operator string(GameTag gameTag)
            => gameTag.ToString();

        #endregion Methods
        
    }

}