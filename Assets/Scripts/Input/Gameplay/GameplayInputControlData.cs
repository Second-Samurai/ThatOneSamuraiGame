using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using ThatOneSamuraiGame.Scripts.Player.ViewOrientation;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseActionHandler;

namespace ThatOneSamuraiGame.Scripts.Input.Gameplay
{

    public class GameplayInputControlData
    {

        #region - - - - - - Properties - - - - - -
        
        public IPauseActionHandler PauseActionHandler { get; set; }

        public IPlayerAttackHandler PlayerAttackHandler { get; set; }
        
        public IPlayerMovement PlayerMovement { get; set; }
        
        public IPlayerTargetTracking PlayerTargetTracking { get; set; }
        
        public IPlayerSpecialAction PlayerSpecialAction { get; set; }
        
        public IPlayerViewOrientationHandler PlayerViewOrientationHandler { get; set; }
        
        public IDebugHandler DebugHandler { get; set; }

        #endregion Properties
        
    }

}