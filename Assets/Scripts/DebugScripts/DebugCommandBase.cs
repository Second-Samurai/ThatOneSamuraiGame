using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    private string commandId;
    private string commandDescription;
    private string commandFormat;

    public string CommandId => commandId;
    public string CommandDescription => commandDescription;
    public string CommandFormat => commandFormat;

    public DebugCommandBase(string commandId, string commandDescription, string commandFormat)
    {
        this.commandId = commandId;
        this.commandDescription = commandDescription;
        this.commandFormat = commandFormat;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command?.Invoke();
    }
}

public class DebugCommand<T> : DebugCommandBase
{
    private Action<T> command;

    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T> command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke(T value)
    {
        command?.Invoke(value);
    }
}

public class DebugCommand<T1,T2> : DebugCommandBase
{
    private Action<T1,T2> command;

    public DebugCommand(string commandId, string commandDescription, string commandFormat, Action<T1,T2> command) : base(commandId, commandDescription, commandFormat)
    {
        this.command = command;
    }

    public void Invoke(T1 value, T2 value2)
    {
        command?.Invoke(value,value2);
    }
}
