using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChainTracker : MonoBehaviour
{
    Animator _animator;
    [SerializeField] int _inputCounter = 0;
    [SerializeField] float _lastInput = 0f;
    public float inputWindow = .9f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - _lastInput > inputWindow)
        {
            _inputCounter = 0;
            ResetCombo();
        } 
    }

    public void RegisterInput()
    {
        _inputCounter++;
        _lastInput = Time.time;
        if(_inputCounter == 1)
        {
            _animator.SetBool("FirstAttack", true);
        }
        _inputCounter = Mathf.Clamp(_inputCounter, 0, 3);
    }

    public void CheckCombo()
    {
        if(_inputCounter >= 2)
        {
            _animator.SetBool("SecondAttack", true);
            _animator.SetBool("LoopAttack", false);
            _animator.SetBool("FirstAttack", false);
        }
        else
        {
            _animator.SetBool("FirstAttack", false);
            _inputCounter = 0;
        }
    }
    public void CheckLoopCombo()
    {
        if (_inputCounter == 3)
        {
            _animator.SetBool("LoopAttack", true);
            _animator.SetBool("FirstAttack", false); 
            _animator.SetBool("SecondAttack", false);

            _inputCounter = 1;
        }
        else
        {
            _animator.SetBool("SecondAttack", false);
            _inputCounter = 0;
        }
    }

    public void ResetCombo()
    {
        _animator.SetBool("FirstAttack", false);
        _animator.SetBool("SecondAttack", false);
        _animator.SetBool("LoopAttack", false);
    }
}
