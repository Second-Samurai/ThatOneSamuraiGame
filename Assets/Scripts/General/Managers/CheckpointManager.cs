using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints;
    public int activeCheckpoint;

    private void Awake()
    { 
        SaveSystem.LoadGame();
    }

    private void Start()
    {
        GetSaveDataCheckpoint();
        if(CheckIfCheckpointAvailable()) GameManager.instance.UserInterfaceManager.ButtonController.EnableContinue();
    }

    public bool CheckIfCheckpointAvailable()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.bIsActive) return true;
        }
        return false;
    }

    public void SaveActiveCheckpoint()
    {
        GameData.currentCheckpoint = activeCheckpoint;
        SaveSystem.SaveGame();
    }

    public void GetSaveDataCheckpoint()
    {
        activeCheckpoint = GameData.currentCheckpoint;
        //Debug.Log(activeCheckpoint);
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.bIsActive = false;
        }
        if (GameData.bLoaded)
        {

            checkpoints[activeCheckpoint].bIsActive = true;
        }
    }

      
    public void LoadCheckpoint()
    {
        if(checkpoints[activeCheckpoint] != null && checkpoints[activeCheckpoint].bIsActive) checkpoints[activeCheckpoint].LoadCheckpoint();
        else
        {
            Debug.LogError("No active Checkpoint! Restarting");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ResetCheckpoints()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.bIsActive = false;
        }
        activeCheckpoint = 0;
    }
   
}
