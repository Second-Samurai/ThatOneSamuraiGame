using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager checkpointManager;
    public bool bIsActive = false;
    public Transform spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        checkpointManager = GameManager.instance.checkpointManager;
        checkpointManager.checkpoints.Add(this); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !bIsActive)
        {
            SetActiveCheckpoint();
        }
    }

    public void SetActiveCheckpoint()
    {
        foreach (Checkpoint checkpoint in checkpointManager.checkpoints)
        {
            checkpoint.bIsActive = false;
        }
        checkpointManager.activeCheckpoint = checkpointManager.checkpoints.IndexOf(this);
        bIsActive = true;
    }

    public void LoadCheckpoint()
    {
        GameManager.instance.playerController.gameObject.transform.position = spawnPos.position;
        GameManager.instance.EnableInput();
    }

}
