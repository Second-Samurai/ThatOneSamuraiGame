using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour, IDebugCommandSystem
{

    #region - - - - - - Fields - - - - - -

    public static DebugManager Instance;

    public List<object> CommandList = new(); 
    
    [SerializeField]
    private DebugConsole console;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start() 
        => InitCommands();

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    private void InitCommands()
    { 
        CommandList.Add(new DebugCommand("help", "shows all commands", "help", () => { console.ShowHelp(true); }));
        CommandList.Add(new DebugCommand("reset", "restarts the game", "reset", () => { SceneManager.LoadScene(0); }));
        
        // Register all commands
        DebugCommandConfigurator _CommandConfigurator = new DebugCommandConfigurator();
        _CommandConfigurator.ConfigureCommands();
    }

    public void RegisterCommand(object command) 
        => CommandList.Add(command);

    public void UnregisterCommand(string id)
    {  
        var command = CommandList.First(x => x is DebugCommandBase baseCommand && baseCommand.CommandId == id);

        if (command != null)
        {
            CommandList.Remove(command);
        }
    }

    public void ShowConsolePressed() 
        => console.OnConsolePressed();

    public void EnterPressed() 
        => console.OnEnterPressed();

    #endregion Methods
  
}
