using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    Vector2 _inputVector;
    public bool bCanMove = true;
    public float rotationSpeed = 4f, smoothingValue = .1f;
    Animator _animator;
     
    float _turnSmoothVelocity;
     
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void OnMovement(InputValue dir) 
    {
        _inputVector = dir.Get<Vector2>(); 
    }
  
    private void FixedUpdate()
    {
        if (bCanMove)
        {
            Vector3 _direction = new Vector3(_inputVector.x, 0, _inputVector.y).normalized;
            if (_direction != Vector3.zero)
            {
                float _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, .1f);
                 
                transform.rotation = Quaternion.Euler(0f, _angle, 0f);
            }
        }
        _animator.SetFloat("InputSpeed", _inputVector.magnitude, smoothingValue, Time.deltaTime);
    }
     
}
