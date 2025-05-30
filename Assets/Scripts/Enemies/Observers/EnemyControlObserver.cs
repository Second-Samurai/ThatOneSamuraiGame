﻿using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Responsible for observing Enemy specific events
/// </summary>
public class EnemyControlObserver : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public UnityEvent OnEnemyStart = new();
    public UnityEvent OnEnemyStop = new();
    public UnityEvent<GameObject> OnEnemyDeath = new();

    #endregion Fields

}
