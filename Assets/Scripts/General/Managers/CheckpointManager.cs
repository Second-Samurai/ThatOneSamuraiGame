using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// SCOPE: SCENE-ONLY
public class CheckpointManager : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public List<Checkpoint> checkpoints;
    public int activeCheckpoint;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -

    public void Initialize()
    {
        this.GetSaveDataCheckpoint();
        
        if(this.CheckIfCheckpointAvailable()) 
            GameManager.instance.UserInterfaceManager.ButtonController.EnableContinue();
    }

    #endregion Initializers
  
    #region - - - - - - Unity Methods - - - - - -

    private void Awake() 
        => SaveSystem.LoadGame();

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -
    
    public void LoadCheckpoint()
    {
        if(checkpoints[activeCheckpoint] != null && checkpoints[activeCheckpoint].IsActive) 
            this.checkpoints[activeCheckpoint].LoadCheckpoint();
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
            checkpoint.IsActive = false;
        
        activeCheckpoint = 0;
    }

    private bool CheckIfCheckpointAvailable() 
        => this.checkpoints.Any(c => c.IsActive);

    public void SaveActiveCheckpoint()
    {
        GameData.currentCheckpoint = activeCheckpoint;
        SaveSystem.SaveGame();
    }

    // Retrieves all the checkpoints from the GameData
    private void GetSaveDataCheckpoint()
    {
        this.activeCheckpoint = GameData.currentCheckpoint;
        
        foreach (Checkpoint _CheckPoint in checkpoints)
            _CheckPoint.IsActive = false;
        
        if (GameData.bLoaded)
            this.checkpoints[activeCheckpoint].IsActive = true;
    }

    #endregion Methods
  
}
