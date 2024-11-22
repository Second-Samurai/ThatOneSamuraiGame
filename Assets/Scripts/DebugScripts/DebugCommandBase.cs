using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandBase
{
    private string m_commandId;
    private string m_commandDescription;
    private string m_commandFormat;

    public string CommandId => m_commandId;
    public string CommandDescription => m_commandDescription;
    public string CommandFormat => m_commandFormat;

    public DebugCommandBase(string commandId, string commandDescription, string commandFormat)
    {
        this.m_commandId = commandId;
        this.m_commandDescription = commandDescription;
        this.m_commandFormat = commandFormat;
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
