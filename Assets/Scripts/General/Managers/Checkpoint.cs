using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager checkpointManager;
    public bool bIsActive = false;
    public Transform spawnPos;

    // Start is called before the first frame update
    void Awake()
    {
        checkpointManager = GameManager.instance.checkpointManager;
        checkpointManager.checkpoints.Add(this); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !bIsActive)
        {
            Debug.Log("Checkpoint Set");
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
        GameManager.instance.enemySpawnManager.SaveEnemyList();
        GameManager.instance.rewindManager.IncreaseRewindAmount();
    }

    public void LoadCheckpoint()
    {
        GameManager.instance.playerController.gameObject.transform.position = spawnPos.position;
        GameManager.instance.EnableInput();
        GameManager.instance.thirdPersonViewCamera.GetComponent<ThirdPersonCamController>().SetPriority(11);
        GameManager.instance.rewindManager.isTravelling = false;
        PlayerFunctions player = GameManager.instance.playerController.gameObject.GetComponent<PlayerFunctions>();
        
        player.rSword.SetActive(true);
        player.gameObject.GetComponent<PlayerInputScript>().bCanAttack = true; 
    }

}
