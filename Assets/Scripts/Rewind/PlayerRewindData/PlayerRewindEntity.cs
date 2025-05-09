using System;
using System.Collections.Generic;
using ThatOneSamuraiGame.Legacy;
using UnityEngine;

public class PlayerRewindEntity : AnimationRewindEntity
{
    
    public List<PlayerTimeData> playerDataList;
    public Collider swordCollider;
    public Rigidbody gameObjectRigidbody;

    private ICameraController m_CameraController;
    private PlayerFunctions m_PlayerFunctions;
     
       
    [Obsolete("Deprecated", false)]
    public override void StepBack()
    {
         
    }

    [Obsolete("Deprecated", false)]
    public override void StepForward()
    {
        
    }
       
    [Obsolete("Deprecated", false)]
    public override void ApplyData()
    {
         
    }
    
}
