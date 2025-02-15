using System;
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Enumeration;

namespace Enumeration
{

    [Serializable]
    public class WeaponPickupType : SmartEnum
    {

        #region - - - - - - Fields - - - - - -

        public static WeaponPickupType Katana = new("Katana", 0, weaponSystem =>
        {
            weaponSystem.SetWeapon(GameManager.instance.gameSettings.katanaPrefab);
            AudioManager.instance.LightSaber = false;
        });
        public static WeaponPickupType LaserSword = new("LaserSword", 1, weaponSystem =>
        {
            weaponSystem.SetWeapon(GameManager.instance.gameSettings.laserSword);
            AudioManager.instance.LightSaber = true;
        });
        // public static WeaponPickupType MoeHammer = new("MoeHammer", 2, () => { }) 

        #endregion Fields
  
        #region - - - - - - Constructors - - - - - -

        public WeaponPickupType(string name, int value, Action<IWeaponSystem> action) : base(name, value) 
            => this.Action = action;

        #endregion Constructors
  
        #region - - - - - - Properties - - - - - -

        public Action<IWeaponSystem> Action { get; private set; }

        #endregion Properties
  
        #region - - - - - - Methods - - - - - -

        public static WeaponPickupType FromEnum(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Katana : return Katana;
                case WeaponType.Lightsaber : return LaserSword;
                default : return Katana;
            }
        }

        public static implicit operator int(WeaponPickupType weaponPickupType)
            => weaponPickupType.GetValue();

        public static implicit operator string(WeaponPickupType weaponPickupType)
            => weaponPickupType.ToString();

        #endregion Methods
    }

}