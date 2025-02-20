using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
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
        private PlayerAttackState m_PlayerAttackState;

        private bool m_IsWeaponDrawn;
        
        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public GameObject EquippedWeapon
            => this.m_EquippedWeapon;
        
        public IWeaponEffectHandler WeaponEffectHandler
            => this.m_WeaponEffectHandler;

        public bool IsWeaponDrawn => this.m_IsWeaponDrawn;

        #endregion Properties

        #region - - - - - - Unity Methods - - - - - -

        private void Start()
        {
            this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;

            IWeaponAnimationEvents _AnimationEvents = this.GetComponent<IWeaponAnimationEvents>();
            _AnimationEvents.OnRevealWeapons.AddListener(this.RevealWeapon);
            _AnimationEvents.OnHideWeapons.AddListener(this.HideWeapon);
            _AnimationEvents.OnPlayWeaponEffect.AddListener(this.StartWeaponEffect);
        }

        #endregion Unity Methods
  
        #region - - - - - - Methods - - - - - -

        public void SetWeapon(GameObject weaponPrefab)
        {
            if (this.IsWeaponEquipped())
                this.RemoveWeapon();

            this.m_EquippedWeapon = Instantiate(weaponPrefab, this.m_WeaponHolder);
            this.m_EquippedWeapon.GetComponent<IInitialize<WeaponEffectInitializerData>>()
                .Initialize(new WeaponEffectInitializerData { ParentTransform = this.m_WeaponHolder });
            this.m_WeaponEffectHandler = this.m_EquippedWeapon.GetComponent<IWeaponEffectHandler>();

            ICombatController playerCombatController = this.GetComponent<ICombatController>();
            playerCombatController.DrawSword();
            
            this.m_PlayerAttackState.CanAttack = true;
        }
        
        private void RevealWeapon() 
            => this.m_EquippedWeapon.SetActive(true);

        private void HideWeapon() 
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