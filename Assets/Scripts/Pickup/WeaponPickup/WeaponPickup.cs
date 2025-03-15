using Enumeration;
using UnityEngine;

namespace ThatOneSamuraiGame
{
    
    // C# enum included to display property drawer in space for the smart enums.
    public enum WeaponType
    {
        Katana,
        Lightsaber
    }
    
    public class WeaponPickup : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private WeaponType m_SelectedWeaponType;
        [SerializeField] private bool m_IsHiddenAfterPickup;

        #endregion Fields
  
        #region - - - - - - Unity Methods - - - - - -

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(GameTag.Player)) return;
            
            WeaponPickupType.FromEnum(this.m_SelectedWeaponType).Action
                .Invoke(other.GetComponent<IWeaponSystem>());
            this.gameObject.SetActive(!this.m_IsHiddenAfterPickup);
        }

        #endregion Unity Methods
  
    }

}