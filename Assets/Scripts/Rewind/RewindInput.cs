using System;
using System.Collections;
using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;

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
