using ThatOneSamuraiGame.Scripts.Enumeration;

public class PlayerMovementStates : SmartEnum
{

    #region - - - - - - Fields - - - - - -

    public static PlayerMovementStates Normal = new("Normal", 0);
    public static PlayerMovementStates LockOn = new("LockOn", 1);
    public static PlayerMovementStates Finisher = new("Finisher", 2);

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public PlayerMovementStates(string name, int value) : base(name, value) { }

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public static implicit operator int(PlayerMovementStates gameScenes)
        => gameScenes.GetValue();

    public static implicit operator string(PlayerMovementStates gameScenes)
        => gameScenes.ToString();

    #endregion Methods
    
}
