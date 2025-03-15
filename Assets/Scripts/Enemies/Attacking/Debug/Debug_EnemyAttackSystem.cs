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
        GameObject _CurrentTargetEnemy = (((IDebuggable)_LockOnSystem).GetDebugInfo() as DebuggingLockOnSystemInfo)?.TargetEnemy;
        if (!_LockOnSystem.IsLockingOnTarget || _CurrentTargetEnemy == null)
        {
            Debug.Log("Player is not locked onto target.");
            return;
        }

        IEnemyAttackSystem _EnemyAttackSystem = _CurrentTargetEnemy.GetComponent<IEnemyAttackSystem>();
        _EnemyAttackSystem.HandleAttackDeflection();
    }

    private void DepleteCurrentEnemysGuard()
    {
        GameObject _PlayerObject = this.GetPlayerObject();
        
        ILockOnSystem _LockOnSystem = _PlayerObject.GetComponentInChildren<LockOnSystem>();
        GameObject _CurrentTargetEnemy = (((IDebuggable)_LockOnSystem).GetDebugInfo() as DebuggingLockOnSystemInfo)?.TargetEnemy;
        if (!_LockOnSystem.IsLockingOnTarget || _CurrentTargetEnemy == null)
        {
            Debug.Log("Player is not locked onto target.");
            return;
        }

        DebuggingAttackDeflectionInfo _EnemyGuardInfo = _CurrentTargetEnemy
            .GetComponent<IDebuggable<DebuggingAttackDeflectionInfo>>()
            .GetDebugInfo();
        IEnemyAttackSystem _EnemyAttackSystem = _CurrentTargetEnemy.GetComponent<IEnemyAttackSystem>();
        for (float _ProjectedGuardAmount = _EnemyGuardInfo.MaxGuard;
             _ProjectedGuardAmount > 0;
             _ProjectedGuardAmount -= _EnemyGuardInfo.GuardDamage)
            _EnemyAttackSystem.HandleAttackDeflection();
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
