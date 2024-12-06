using System;
using UnityEngine;

public class PBufferedInputs : MonoBehaviour
{
    #region - - - - - - Fields - - - - - -
    
    // Components
    private PCombatController m_PlayerCombatController;
    
    // Attack Buffers
    private float m_attackInputBufferTimer;
    private bool m_isAttackInputCached = false;
    public float m_attackInputBufferDuration = 0.4f;
    
    // Dodge Buffers
    
    #endregion
    
    #region - - - - - - Lifecycle Methods - - - - - -

    private void Start()
    {
        m_PlayerCombatController = GetComponent<PCombatController>();
    }

    private void Update()
    {
        LightAttackBufferUpdate();
    }
    
    #endregion  - - - - - - LifeCycle Methods - - - - - - 

    #region Methods
    
    private void LightAttackBufferUpdate()
    {
        if (IsAttackInputBufferRunning())
        {
            m_attackInputBufferTimer -= Time.deltaTime;

            if (!IsAttackInputBufferRunning())
            {
                // If the user has cached an attack input (i.e. pressed attack mid-attack), then save the input
                // and attack at the next available moment
                if (m_isAttackInputCached)
                {
                    //Debug.Log("Performing cached attack");
                    m_isAttackInputCached = false;
                    m_PlayerCombatController.AttemptLightAttack();
                }
            }
        }
    }
    
    public void StartAttackBuffer()
    {
        m_attackInputBufferTimer = m_attackInputBufferDuration;
    }

    public bool IsAttackInputBufferRunning()
    {
        return m_attackInputBufferTimer > 0f;
    }

    public bool GetAttackInputCached()
    {
        return m_isAttackInputCached;
    }

    public void SetAttackInputCached(bool value)
    {
        if (value)
        {
            //Debug.Log("Caching input as buffer is running");
        }
        
        m_isAttackInputCached = value;
    }
    
    #endregion
}
