using UnityEngine;

namespace ThatOneSamuraiGame
{

    public interface IWeaponSystem
    {

        #region - - - - - - Properties - - - - - -

        GameObject EquippedWeapon { get; }
        
        IWeaponEffectHandler WeaponEffectHandler { get; }
        
        #endregion Properties
  
        #region - - - - - - Methods - - - - - -

        void SetWeapon(GameObject weaponPrefab);
        
        void StartWeaponEffect(float slashAngle);

        bool IsWeaponEquipped();

        #endregion Methods
  
    }

}