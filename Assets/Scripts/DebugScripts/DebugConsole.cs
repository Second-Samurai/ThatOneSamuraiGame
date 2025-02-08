using System;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    private DebugManager m_debugManager;
    
    // Console Fields
    private bool m_showConsole;
    private bool m_showHelp;
    private string m_input;
    private Vector2 m_scroll;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start() 
        => m_debugManager = DebugManager.Instance;

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void OnConsolePressed()
    {
        m_showConsole = !m_showConsole;

        if (!m_showConsole )
        {
            m_showHelp = false;
        }

        Cursor.lockState = m_showConsole ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = m_showConsole;
    }

    public void ShowHelp(bool showHelp)
    {
        this.m_showHelp = showHelp;
    }

    public void OnEnterPressed()
    {
        if (m_showConsole)
            HandleInput();
    }

    private void OnGUI()
    {
        if (!m_showConsole)
            return; 

        float y = 0f;

        if (m_showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0,0,Screen.width - 30, 20 * m_debugManager.CommandList.Count);

            m_scroll = GUI.BeginScrollView(new Rect(0, y+5f, Screen.width, 90), m_scroll, viewport); 

            for (int i = 0; i < m_debugManager.CommandList.Count; i++)
            {
                DebugCommandBase command = m_debugManager.CommandList[i] as DebugCommandBase;

                string label = $"{command.CommandFormat} - {command.CommandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();

            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);

        m_input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), m_input);
    }

    private void HandleInput()
    {
        if (m_input == null || m_input == "" || string.IsNullOrWhiteSpace(m_input))
            return;

        string[] inputs = m_input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var targetCommand = m_debugManager.CommandList.Find(x => x is DebugCommandBase commandBase && inputs[0] == commandBase.CommandId);

        if (targetCommand != null)
        {
            if (targetCommand as DebugCommand != null)
                (targetCommand as DebugCommand).Invoke();
            else if (targetCommand as DebugCommand<int> != null)
                (targetCommand as DebugCommand<int>).Invoke(int.Parse(inputs[1]));
            else if (targetCommand as DebugCommand<string,int> != null)
                (targetCommand as DebugCommand<string,int>).Invoke(inputs[1], int.Parse(inputs[2]));
        }

        m_input = "";
    }

    #endregion Methods
  
}
