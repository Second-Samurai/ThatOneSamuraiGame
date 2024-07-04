using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Containers
{
    
    public interface IPlayerState
    {

        #region - - - - - - Properties - - - - - -

        GameObject AttackTarket { get; }
        
        bool CanOverrideMovement { get; set; }

        #endregion Properties
        
    }
    
}