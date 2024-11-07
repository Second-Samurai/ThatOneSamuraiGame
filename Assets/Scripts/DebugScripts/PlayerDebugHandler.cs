using UnityEngine;

public class PlayerDebugHandler : MonoBehaviour, IDebugHandler
{
    public void SubmitDebugCommand()
    {
        if (DebugManager.Instance == null)
        {
            return;
        }

        DebugManager.Instance.EnterPressed();
    }

    public void ToggleDebugMenu()
    {
        if (DebugManager.Instance == null)
        {
            return;
        }

        DebugManager.Instance.ShowConsolePressed();
    } 
}
