using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Legacy;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    #region - - - - - - Methods - - - - - -

    public bool IsActive;
    public Transform SpawnPosition;

    private CheckpointManager m_CheckpointManager;
    private PlayerAttackState m_PlayerAttackState;

    #endregion Methods

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        // TODO: Move this so it is handled by pipeline.
        this.m_CheckpointManager = FindFirstObjectByType<CheckpointManager>();
        this.m_PlayerAttackState = GameManager.instance.PlayerController
            .GetComponent<IPlayerState>()
            .PlayerAttackState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player") && this.IsActive)
            return;
        
        this.SetActiveCheckpoint();
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void LoadCheckpoint()
    {
        GameManager.instance.PlayerController.gameObject.transform.position = SpawnPosition.position;
        GameManager.instance.InputManager.EnableActiveInputControl();
        GameManager.instance.ThirdPersonViewCamera.GetComponent<ThirdPersonCamController>().SetPriority(11);

        // Note: The sword manager exists on the root parent fo the player object
        // PSwordManager _PlayerSwordManager = this.GetComponent<PSwordManager>();
        IWeaponSystem _WeaponSystem = this.GetComponent<IWeaponSystem>();
        _WeaponSystem.SetWeapon(GameManager.instance.gameSettings.katanaPrefab);

        PlayerFunctions _Player = GameManager.instance.PlayerController.gameObject.GetComponent<PlayerFunctions>();
        ICombatController playerCombatController = _Player.gameObject.GetComponent<ICombatController>();
        playerCombatController.DrawSword();

        this.m_PlayerAttackState.CanAttack = true;
    }

    private void SetActiveCheckpoint()
    {
        foreach (Checkpoint checkpoint in m_CheckpointManager.checkpoints) 
            checkpoint.IsActive = false;
        
        this.m_CheckpointManager.activeCheckpoint = this.m_CheckpointManager.checkpoints.IndexOf(this);
        this.IsActive = true;
        
        GameManager.instance.EnemySpawnManager.SaveEnemyList();
    }

    #endregion Methods

}
