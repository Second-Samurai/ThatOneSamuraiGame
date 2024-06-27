using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    public class PlayerAttackHandler : MonoBehaviour, IPlayerAttackHandler
    {
        
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

        void IPlayerAttackHandler.OnStartHeavyAlternative()
        {
            throw new NotImplementedException();
        }

        void IPlayerAttackHandler.OnStartBlock()
        {
            throw new NotImplementedException();
        }

        void IPlayerAttackHandler.OnEndBlock()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
        
    }
    
}
