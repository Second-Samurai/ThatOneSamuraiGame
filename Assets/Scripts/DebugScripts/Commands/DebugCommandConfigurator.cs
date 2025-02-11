
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Player.Attack.Debug;

public class DebugCommandConfigurator
{

    #region - - - - - - Fields - - - - - -

    private DebugManager m_DebugManager;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public DebugCommandConfigurator() 
        => this.m_DebugManager = DebugManager.Instance;

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public void ConfigureCommands()
    {
        // Camera system
        new Debug_CameraController().RegisterCommand(this.m_DebugManager);
        
        // Player
        new Debug_PlayerAttack().RegisterCommand(this.m_DebugManager);
    }

    #endregion Methods
  
}
