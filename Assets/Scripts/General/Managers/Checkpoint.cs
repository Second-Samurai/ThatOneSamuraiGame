using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager checkpointManager;
    public bool bIsActive = false;
    public Transform spawnPos;

    private PlayerAttackState m_PlayerAttackState;

    // Start is called before the first frame update
    void Awake()
    {
        checkpointManager = GameManager.instance.CheckpointManager;
        //checkpointManager.checkpoints.Add(this); 

        this.m_PlayerAttackState = GameManager.instance.PlayerController
                                        .GetComponent<IPlayerState>()
                                        .PlayerAttackState;
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
        GameManager.instance.EnemySpawnManager.SaveEnemyList();
        GameManager.instance.RewindManager.IncreaseRewindAmount();
    }

    public void LoadCheckpoint()
    {
        GameManager.instance.PlayerController.gameObject.transform.position = spawnPos.position;
        GameManager.instance.InputManager.EnableActiveInputControl();
        GameManager.instance.ThirdPersonViewCamera.GetComponent<ThirdPersonCamController>().SetPriority(11);
        GameManager.instance.RewindManager.isTravelling = false;
        PlayerFunctions _Player = GameManager.instance.PlayerController.gameObject.GetComponent<PlayerFunctions>();

        // Note: The sword manager exists on the root parent fo the player object
        PSwordManager _PlayerSwordManager = this.GetComponent<PSwordManager>();
        _PlayerSwordManager.SetWeapon(true, GameManager.instance.gameSettings.katanaPrefab);

        ICombatController playerCombatController = _Player.gameObject.GetComponent<ICombatController>();
        playerCombatController.DrawSword();

        this.m_PlayerAttackState.CanAttack = true;
    }

}
