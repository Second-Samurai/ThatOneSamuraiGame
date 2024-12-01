using UnityEngine;

public class RewindInput : MonoBehaviour
{ 
    public bool isTravelling = false;
    public GameObject rewindTut;
    public GameObject rewindBar;

    PlayerFunctions playerFunction;
    FinishingMoveController finishingMoveController;

    public GameEvent hidePopupEvent;
    public GameEvent hideLockOnPopupEvent; 
}
