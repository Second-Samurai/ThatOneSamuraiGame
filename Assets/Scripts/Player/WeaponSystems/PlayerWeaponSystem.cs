using UnityEngine;

namespace ThatOneSamuraiGame
{

    public class PlayerWeaponSystem : MonoBehaviour, IWeaponSystem
    {

        #region - - - - - - Fields - - - - - -

        [RequiredField]
        [SerializeField] 
        private Transform m_WeaponHolder;
        
        private GameObject m_EquippedWeapon;
        private IWeaponEffectHandler m_WeaponEffectHandler;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public GameObject EquippedWeapon
            => this.m_EquippedWeapon;
        
        public IWeaponEffectHandler WeaponEffectHandler
            => this.m_WeaponEffectHandler;

        #endregion Properties
  
        #region - - - - - - Methods - - - - - -

        public void SetWeapon(GameObject weaponPrefab)
        {
            if (this.IsWeaponEquipped())
                this.RemoveWeapon();

            this.m_EquippedWeapon = Instantiate(weaponPrefab, this.m_WeaponHolder);
            this.m_EquippedWeapon.GetComponent<IInitialize<WeaponEffectInitializerData>>()
                .Initialize(new WeaponEffectInitializerData { ParentTransform = this.m_WeaponHolder });
            this.m_WeaponEffectHandler = this.m_EquippedWeapon.GetComponent<IWeaponEffectHandler>();
        }
        
        public void RevealWeapon() 
            => this.m_EquippedWeapon.SetActive(true);

        public void HideWeapon() 
            => this.m_EquippedWeapon.SetActive(false);

        public void StartWeaponEffect(float slashAngle) 
            => this.m_WeaponEffectHandler.CreateSlashEffect(slashAngle);

        public bool IsWeaponEquipped() 
            => this.m_EquippedWeapon != null;

        private void RemoveWeapon()
        {
            Destroy(this.m_EquippedWeapon);
            this.m_EquippedWeapon = null;
        }

        #endregion Methods
  
    }

}