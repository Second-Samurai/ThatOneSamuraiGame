using System;
using Player.Animation;
using UnityEngine;

[Obsolete]
public class AttackChainTracker : MonoBehaviour
{
    private PlayerAnimationComponent m_PlayerAnimationComponent;
    [SerializeField] int _inputCounter = 0;
    [SerializeField] float _lastInput = 0f;
    public float inputWindow = .9f;

    // Start is called before the first frame update
    void Start() 
        => m_PlayerAnimationComponent = GetComponent<PlayerAnimationComponent>();
    
    void Update()
    {
        if(Time.time - _lastInput > inputWindow && _inputCounter != 0)
        {
            _inputCounter = 0;
            ResetCombo();
        } 
    }

    public void RegisterInput(bool isSprintAttack)
    {
        _inputCounter++;
        _lastInput = Time.time;
        
        if (isSprintAttack)
        {
            m_PlayerAnimationComponent.TriggerSprintAttack();
        }
        else
        {
            if (_inputCounter % 2 != 0 || _inputCounter == 0)
            {
                m_PlayerAnimationComponent.TriggerLightAttack(1);
            }
            else
            {
                m_PlayerAnimationComponent.TriggerLightAttack(2);
            }
        }
        
        //_animator.SetTrigger("AttackLight");
        // if(_inputCounter == 1)
        // {
            //if(!_animator.GetBool("FirstAttack"))
            //    _animator.SetBool("FirstAttack", true);
            //else
            //    _animator.SetBool("LoopAttack", true);
           // _input.bCanDodge = false;
        // }
        //_inputCounter = Mathf.Clamp(_inputCounter, 0, 3);
    }

    public void CheckCombo()
    {
        if(_inputCounter >= 2)
        {
            //_animator.SetBool("SecondAttack", true);
            //_animator.SetBool("LoopAttack", false);
            //_animator.SetBool("FirstAttack", false);

        }
        else
        {
            //_animator.SetBool("FirstAttack", false);
            _inputCounter = 0;
           // _input.bCanDodge = true;
        }
    }
    public void CheckLoopCombo()
    {
        if (_inputCounter >= 2)
        {
           // _animator.SetBool("LoopAttack", true);
           // _animator.SetTrigger("AttackLight");
            //_animator.SetBool("FirstAttack", false); 
            //_animator.SetBool("SecondAttack", false);

            _inputCounter = 1;
        }
        else
        {
            //_animator.SetBool("SecondAttack", false);
            _inputCounter = 0;
          //  _input.bCanDodge = true;
        }
    }

    private void ResetCombo()
    {
        //Debug.Log("Reset combo");
        m_PlayerAnimationComponent.ResetAttackParameters();
        //_input.bCanDodge = true;
    }
}
