using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.ViewOrientation
{

    public interface IPlayerViewOrientationHandler
    {

        #region - - - - - - Event Handlers - - - - - -

        void RotateViewOrientation(Vector2 mouseInputVector);

        Vector2 GetInputScreenPosition();

        #endregion Event Handlers

    }

}