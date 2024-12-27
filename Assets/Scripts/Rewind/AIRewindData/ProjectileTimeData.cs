using UnityEngine;

public class ProjectileTimeData 
{ 
    public Vector3 direction;
    public Vector3 velocity;
    public bool isActive;
    public bool hitEnemys;

    public ProjectileTimeData(Vector3 _direction, Vector3 _velocity, bool _isActive, bool _hitEnemys) 
    {
        direction = _direction;
        velocity = _velocity;
        isActive = _isActive;
        hitEnemys = _hitEnemys;
    
    }
    
  
}
