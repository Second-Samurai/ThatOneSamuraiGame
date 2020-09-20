using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTimeData 
{ 
    public Vector3 direction;
    public Rigidbody rb;

    public ProjectileTimeData(Vector3 _direction, Rigidbody _rb) 
    {
        direction = _direction;
        rb = _rb;
    
    }
    
  
}
