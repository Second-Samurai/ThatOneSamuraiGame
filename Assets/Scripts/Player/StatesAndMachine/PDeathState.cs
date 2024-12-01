using UnityEngine;

/// <summary>
/// This state is only called during the death of the player
/// </summary>
public class PDeathState : PlayerState
{
    public override void BeginState()
    {
        Debug.Log("Player has died");
    }
}
