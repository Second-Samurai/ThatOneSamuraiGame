using System.Linq;
using ThatOneSamuraiGame.GameLogging;
using UnityEngine;
using Object = UnityEngine.Object;

public class Debug_EnemyAttackSystem : IDebugCommandRegistrater
{

    #region - - - - - - Methods - - - - - -

    public void RegisterCommand(IDebugCommandSystem debugCommandSystem)
    {
        DebugCommand _DamageCurrentEnemysGuard = new DebugCommand(
            "enemy_damageguard", 
            "Damages the target enemy's guard", 
            "enemy_damageguard", 
            this.DamageCurrentEnemysGuard);
        DebugCommand _DepleteCurrentEnemysGuard = new DebugCommand(
            "enemy_depleteguard",
            "Depletes the target enemy's guard",
            "enemy_depleteguard",
            this.DepleteCurrentEnemysGuard);

        debugCommandSystem.RegisterCommand(_DamageCurrentEnemysGuard);
        debugCommandSystem.RegisterCommand(_DepleteCurrentEnemysGuard);
    }

    private void DamageCurrentEnemysGuard()
    {
        GameObject _PlayerObject = this.GetPlayerObject();
        
        ILockOnSystem _LockOnSystem = _PlayerObject.GetComponentInChildren<LockOnSystem>();
        if (!_LockOnSystem.IsLockingOnTarget)
        {
            Debug.Log("Player is not locked onto target.");
            return;
        }

        GameObject _CurrentTargetEnemy = _LockOnSystem.GetCurrentTarget();
        IEnemyAttackSystem _EnemyAttackSystem = _CurrentTargetEnemy.GetComponent<IEnemyAttackSystem>();
        _EnemyAttackSystem.HandleAttackDeflection();
    }

    private void DepleteCurrentEnemysGuard()
    {
        
    }

    #endregion Methods

    #region - - - - - - Support Methods - - - - - -

    private GameObject GetPlayerObject()
    {
        if (Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .Any(x => x.CompareTag("Player")))
        {
            GameObject _PlayerObject = Object.FindFirstObjectByType<PlayerController>().gameObject;
            return _PlayerObject;
        }
            
        GameLogger.LogError("[DEBUG]: No Player tagged object found.");
        return null;
    }

    #endregion Support Methods
  
}
