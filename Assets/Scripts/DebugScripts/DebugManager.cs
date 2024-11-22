using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private DebugConsole console;

    private static DebugManager instance;
    public static DebugManager Instance => instance;

    public List<object> CommandList = new List<object>(); 

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        InitCommands();
    }

    private void InitCommands()
    {
        CommandList.Clear();

        CommandList.Add(new DebugCommand("help", "shows all commands", "help", () => 
        {
            console.ShowHelp(true);
        }));

        CommandList.Add(new DebugCommand("reset", "restarts the game", "reset", () => 
        {
            SceneManager.LoadScene(0); 
        }));
    }

    public void RegisterCommand(object command)
    {
        CommandList.Add(command);
    }

    public void UnregisterCommand(string id)
    {  
        var command = CommandList.First(x => x is DebugCommandBase baseCommand && baseCommand.CommandId == id);

        if (command != null)
        {
            CommandList.Remove(command);
        }
    }

    public void ShowConsolePressed()
    {
        console.OnConsolePressed();
    }

    public void EnterPressed()
    {
        console.OnEnterPressed();
    }
}
