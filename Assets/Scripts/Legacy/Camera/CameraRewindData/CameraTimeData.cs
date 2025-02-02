using UnityEngine;

namespace ThatOneSamuraiGame.Legacy
{

    public class CameraTimeData
    {
        public bool bIsLockedOn;
        public Transform target, player;
        public int priority;

        //targeted enemy needs to be added
        public CameraTimeData(bool _bIsLockedOn, Transform _target, Transform _player, int _priority)
        {
            bIsLockedOn = _bIsLockedOn;
            target = _target;
            player = _player;
            priority = _priority;
        }
    }


}