using System;
using System.Collections;
using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/ArcherDeath")]
public class ArcherDeath : Leaf
{
    // Needs to:
    // - De-Activate any further collision
    // - Remove body applied weights (excluding weapon)
    // - Deactivate all unecessary objects in hierachy
    // - Trigger the death animation
    // - (by event) destroy the evnemy object

    #region - - - - - - Enumerations - - - - - -

    private enum DeathDirection
    {
        Forward = 0,
        Backward = 1,
        Left = 2, 
        Right = 3
    }

    #endregion Enumerations
  
    #region - - - - - - Fields - - - - - -

    [Header("Required Dependencies")]
    [SerializeField, RequiredField] private Animator m_Animator;
    [SerializeField, RequiredField] private ArcherAnimationReciever m_AnimationReceiver;
    [SerializeField, RequiredField] private AnimationRagdollController m_RagdollController;
    [SerializeField, RequiredField] private Transform m_ArcherTransform;
    
    [Space]
    [SerializeField] private TransformReference m_PlayerTransform = new();
    [SerializeField] private float m_TimeTillDestroy = 1f;
    
    private bool m_HasPlayedAnimation;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        GameValidator.NotNull(this.m_Animator, nameof(m_Animator));
        GameValidator.NotNull(this.m_AnimationReceiver, nameof(m_AnimationReceiver));
        GameValidator.NotNull(this.m_RagdollController, nameof(m_RagdollController));
        GameValidator.NotNull(this.m_ArcherTransform, nameof(m_ArcherTransform));
        GameValidator.NotNull(this.m_PlayerTransform, nameof(m_PlayerTransform));
        
        this.m_AnimationReceiver.OnDeath.AddListener(() => this.StartCoroutine(this.KillArcher()));
    }

    #endregion Unity Methods
  
    #region - - - - - - MBT Methods - - - - - -

    public override NodeResult Execute()
    {
        if (this.m_HasPlayedAnimation) return NodeResult.success;

        Vector3 _Direction = this.m_ArcherTransform.position - this.m_PlayerTransform.Value.position;
        ArcherAnimationEvents.ArcherDeath.Run(this.m_Animator);
        ArcherAnimationEvents.DeathDirection.Run(this.m_Animator, intValue: (int)this.GetDirection(_Direction));
        this.m_HasPlayedAnimation = true;
        
        return NodeResult.success;
    }

    #endregion MBT Methods
  
    #region - - - - - - Methods - - - - - -

    private DeathDirection GetDirection(Vector3 direction)
    {
        int _DirectionX = (int)Mathf.Clamp((float)Math.Round(direction.x), -1, 1);
        int _DirectionY = (int)Mathf.Clamp((float)Math.Round(direction.y), -1, 1);

        return _DirectionX switch
        {
            0 when _DirectionY == 1 => DeathDirection.Forward,
            0 when _DirectionY == -1 => DeathDirection.Backward,
            -1 when _DirectionY == 0 => DeathDirection.Left,
            1 when _DirectionY == 0 => DeathDirection.Right,
            _ => DeathDirection.Right
        };
    }

    private IEnumerator KillArcher()
    {
        yield return new WaitForSeconds(this.m_TimeTillDestroy);
        
        GameObject _Archer = this.transform.root.gameObject;
        Destroy(_Archer);
    }

    #endregion Methods
  
}
