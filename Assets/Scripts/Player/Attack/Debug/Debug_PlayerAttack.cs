using System;
using System.Linq;
using ThatOneSamuraiGame.GameLogging;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.Player.Attack.Debug
{

    public class Debug_PlayerAttack : IDebugCommandRegistrater
    {

        #region - - - - - - Command Methods - - - - - -

        public void RegisterCommand(IDebugCommandSystem debugCommandSystem)
        {
            DebugCommand _EquipPlayerWithSword = new DebugCommand(
                "player_equipsword",
                "Equips player with sword",
                "player_equipsword",
                this.EquipPlayerWithSword);
            DebugCommand _EquipPlayerWithLaserSword = new DebugCommand(
                "player_equiplasersword",
                "Equips player with laser sword",
                "player_equiplasersword",
                this.EquipPlayerWithLaserSword);

            DebugCommand _TriggerKnockback = new DebugCommand(
                "player_triggerparry",
                "Triggers the player's parry",
                "player_triggerparry",
                this.TriggerKnockback);
            DebugCommand _TriggerBlocking = new DebugCommand(
                "player_triggerblocking",
                "Triggers the player's blocking",
                "player_triggerblocking",
                this.TriggerBlocking);
            
            debugCommandSystem.RegisterCommand(_EquipPlayerWithSword);
            debugCommandSystem.RegisterCommand(_EquipPlayerWithLaserSword);
            debugCommandSystem.RegisterCommand(_TriggerKnockback);
            debugCommandSystem.RegisterCommand(_TriggerBlocking);
        }

        private void EquipPlayerWithSword()
        {
            GameObject _PlayerObject = this.GetPlayerObject();
            if (_PlayerObject != null)
            {
                IWeaponSystem _WeaponSystem = _PlayerObject.GetComponent<IWeaponSystem>();
                _WeaponSystem.SetWeapon(GameManager.instance.gameSettings.katanaPrefab);
                AudioManager.instance.LightSaber = false;
            }
        }

        private void EquipPlayerWithLaserSword()
        {
            GameObject _PlayerObject = this.GetPlayerObject();
            if (_PlayerObject != null)
            {
                IWeaponSystem _WeaponSystem = _PlayerObject.GetComponent<IWeaponSystem>();
                _WeaponSystem.SetWeapon(GameManager.instance.gameSettings.laserSword);
                AudioManager.instance.LightSaber = true;
            }
        }

        private void TriggerKnockback()
        {
            GameObject _PlayerObject = this.GetPlayerObject();
            if (_PlayerObject != null)
            {
                ParryAttackHandler _ParryHandler = _PlayerObject.GetComponent<ParryAttackHandler>();
                GameObject _NearestEnemy = this.GetNearestEnemyObject(_PlayerObject.transform);
                
                _ParryHandler.TriggerParry(_NearestEnemy, 20);
                IDamageable _PlayerDamageable = _PlayerObject.GetComponent<IDamageable>();
                _PlayerDamageable.OnEntityDamage(20, _NearestEnemy, false);
            }
        }

        private void TriggerBlocking()
        {
            throw new NotImplementedException();
        }

        #endregion Command Methods
  
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

        public GameObject GetNearestEnemyObject(Transform playerTransform)
        {
            if (Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .Any(x => x.CompareTag("Enemy")))
            {
                GameObject _NearestEnemy = Object
                    .FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                    .Where(x => x.CompareTag("Enemy"))
                    .OrderBy(x => Vector3.Distance(x.transform.position, playerTransform.position)).First();
                return _NearestEnemy;
            }
            
            GameLogger.LogError("[DEBUG]: No Enemy tagged objects found.");
            return null;
        }

        #endregion Support Methods
  
    }

}