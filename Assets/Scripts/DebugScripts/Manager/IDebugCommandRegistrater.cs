/// <summary>
/// Responsible for registering commands to the command system.
/// </summary>
public interface IDebugCommandRegistrater
{

    #region - - - - - - Methods - - - - - -

    void RegisterCommand(IDebugCommandSystem debugCommandSystem);

    #endregion Methods

}
