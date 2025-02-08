using System;

public class DebugCommandBase
{

    #region - - - - - - Fields - - - - - -

    private string m_commandId;
    private string m_commandDescription;
    private string m_commandFormat;

    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public string CommandId => m_commandId;
    public string CommandDescription => m_commandDescription;
    public string CommandFormat => m_commandFormat;

    #endregion Properties

    #region - - - - - - Constructors - - - - - -

    public DebugCommandBase(string commandId, string commandDescription, string commandFormat)
    {
        this.m_commandId = commandId;
        this.m_commandDescription = commandDescription;
        this.m_commandFormat = commandFormat;
    }

    #endregion Constructors
  
}

public class DebugCommand : DebugCommandBase
{

    #region - - - - - - Fields - - - - - -

    private Action command;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public DebugCommand(
        string commandId, 
        string commandDescription, 
        string commandFormat, 
        Action command) 
        : base(commandId, commandDescription, commandFormat) =>
        this.command = command;

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public void Invoke() => command?.Invoke();

    #endregion Methods
  
}

public class DebugCommand<T> : DebugCommandBase
{

    #region - - - - - - Fields - - - - - -

    private Action<T> command;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public DebugCommand(
        string commandId, 
        string commandDescription, 
        string commandFormat, 
        Action<T> command) 
        : base(commandId, commandDescription, commandFormat) 
        => this.command = command;

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public void Invoke(T value) => command?.Invoke(value);

    #endregion Methods
  
}

public class DebugCommand<T1,T2> : DebugCommandBase
{

    #region - - - - - - Fields - - - - - -

    private Action<T1,T2> command;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public DebugCommand(
        string commandId, 
        string commandDescription, 
        string commandFormat, 
        Action<T1,T2> command) 
        : base(commandId, commandDescription, commandFormat) 
        => this.command = command;

    #endregion Constructors

    #region - - - - - - Methods - - - - - -

    public void Invoke(T1 value, T2 value2) => command?.Invoke(value,value2);

    #endregion Methods
  
}
