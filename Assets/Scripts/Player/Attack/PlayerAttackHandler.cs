using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    public class PlayerAttackHandler : MonoBehaviour, IPlayerAttackHandler, IPlayerAttackState
    {

        #region - - - - - - Fields - - - - - -

        private bool m_IsWeaponSheathed;

        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        bool IPlayerAttackState.IsWeaponSheathed
            => this.m_IsWeaponSheathed;
        
        #endregion Properties
        
        #region - - - - - - Methods - - - - - -

        void IPlayerAttackHandler.Attack()
        {
            throw new NotImplementedException();
        }

        void IPlayerAttackHandler.DrawSword()
        {
            throw new NotImplementedException();
        }

        void IPlayerAttackHandler.StartHeavy()
        {
            throw new NotImplementedException();
        }

        void IPlayerAttackHandler.StartHeavyAlternative()
        {
            throw new NotImplementedException();
        }

        void IPlayerAttackHandler.StartBlock()
        {
            throw new NotImplementedException();
        }

        void IPlayerAttackHandler.EndBlock()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
        
    }
    
}
