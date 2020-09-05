using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackChainTracker : MonoBehaviour
{
    Animator _animator;
    [SerializeField] int _inputCounter = 0;
    [SerializeField] float _lastInput = 0f;
    public float inputWindow = .9f;
    PlayerInputScript _input;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<PlayerInputScript>();
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
        //_animator.SetTrigger("AttackLight");
        if(_inputCounter == 1)
        {
            //if(!_animator.GetBool("FirstAttack"))
            //    _animator.SetBool("FirstAttack", true);
            //else
            //    _animator.SetBool("LoopAttack", true);
           // _input.bCanDodge = false;
            //Debug.LogError("DodgeOff");
        }
        _inputCounter = Mathf.Clamp(_inputCounter, 0, 3);
    }

    public void CheckCombo()
    {
        if(_inputCounter >= 2)
        {
            //_animator.SetBool("SecondAttack", true);
            _animator.SetBool("LoopAttack", false);
            _animator.SetBool("FirstAttack", false);

        }
        else
        {
            _animator.SetBool("FirstAttack", false);
            _inputCounter = 0;
           // _input.bCanDodge = true;
         //   Debug.LogError("DodgeOn1");
        }
    }
    public void CheckLoopCombo()
    {
        if (_inputCounter >= 2)
        {
           // _animator.SetBool("LoopAttack", true);
           // _animator.SetTrigger("AttackLight");
            _animator.SetBool("FirstAttack", false); 
            _animator.SetBool("SecondAttack", false);

            _inputCounter = 1;
        }
        else
        {
            _animator.SetBool("SecondAttack", false);
            _inputCounter = 0;
          //  _input.bCanDodge = true;
        //    Debug.LogError("DodgeOn2");
        }
    }

    public void ResetCombo()
    {
        _animator.SetBool("FirstAttack", false);
        _animator.SetBool("SecondAttack", false);
        _animator.SetBool("LoopAttack", false);
     //   _input.bCanDodge = true;
        //Debug.LogError("DodgeOn");
    }
}
