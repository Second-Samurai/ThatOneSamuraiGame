using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNormalState : PlayerState
{
    IPlayerController playerController;

    public override void BeginState()
    {
        playerController = this.GetComponent<IPlayerController>();
    }
}
