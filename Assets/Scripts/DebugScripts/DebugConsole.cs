using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    private bool showConsole;
    private bool showHelp;

    private string input;

    private DebugManager debugManager;

    private Vector2 scroll;

    private void Start()
    {
        debugManager = DebugManager.Instance;
    }

    public void OnConsolePressed()
    {
        showConsole = !showConsole;

        if (!showConsole )
        {
            showHelp = false;
        }

        Cursor.lockState = showConsole ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = showConsole;
    }

    public void ShowHelp(bool showHelp)
    {
        this.showHelp = showHelp;
    }

    public void OnEnterPressed()
    {
        if (showConsole)
        {
            HandleInput();
        }
    }

    private void OnGUI()
    {
        if (!showConsole)
        { 
            return; 
        }

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0,0,Screen.width - 30, 20 * debugManager.CommandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y+5f, Screen.width, 90), scroll, viewport); 

            for (int i = 0; i<debugManager.CommandList.Count; i++)
            {
                DebugCommandBase command = debugManager.CommandList[i] as DebugCommandBase;

                string label = $"{command.CommandFormat} - {command.CommandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();

            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);

        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    private void HandleInput()
    {
        if (input == null || input == "" || string.IsNullOrWhiteSpace(input))
        {
            return;
        }

        string[] inputs = input.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

        var targetCommand = debugManager.CommandList.Find(x => x is DebugCommandBase commandBase && inputs[0] == commandBase.CommandId);

        if (targetCommand != null)
        {
            if (targetCommand as DebugCommand != null)
            {
                (targetCommand as DebugCommand).Invoke();
            }
            else if (targetCommand as DebugCommand<int> != null)
            {
                (targetCommand as DebugCommand<int>).Invoke(int.Parse(inputs[1]));
            }
            else if (targetCommand as DebugCommand<string,int> != null)
            {
                (targetCommand as DebugCommand<string,int>).Invoke(inputs[1], int.Parse(inputs[2]));
            }
        }

        input = "";
    }
}
