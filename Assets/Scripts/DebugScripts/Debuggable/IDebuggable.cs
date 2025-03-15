public interface IDebuggable
{

    #region - - - - - - Methods - - - - - -

    void DebugInvoke();

    object GetDebugInfo();

    #endregion Methods

}

public interface IDebuggable<TDebugInfo>
{

    #region - - - - - - Methods - - - - - -

    void DebugInvoke();

    TDebugInfo GetDebugInfo();

    #endregion Methods

}
