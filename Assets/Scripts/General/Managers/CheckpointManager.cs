using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public List<Checkpoint> checkpoints;
    public int activeCheckpoint;

    

    public void SaveActiveCheckpoint()
    {
        GameData.currentCheckpoint = activeCheckpoint;
    }

    public void GetSaveDataCheckpoint()
    {
        activeCheckpoint = GameData.currentCheckpoint;
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.bIsActive = false;
        }
        checkpoints[activeCheckpoint].bIsActive = true;
    }

     
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            LoadCheckpoint();
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
   
}
