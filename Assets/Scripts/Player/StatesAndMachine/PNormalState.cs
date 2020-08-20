using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Summary:
/* 
    This class is a child state of Player State and is responsible
    for any related functions or processes during the lifetime of
    the player. This state ONLY exists when player isn't dead or
    in rewind.
*/

public class PNormalState : PlayerState
{
    IPlayerController playerController;

    public override void BeginState()
    {
        playerController = this.GetComponent<IPlayerController>();
    }
}
