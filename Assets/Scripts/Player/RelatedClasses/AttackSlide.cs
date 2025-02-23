using System.Collections;
using UnityEngine;

/// <summary>
/// Handling slide maneuvers to target enemy during attacks
/// </summary>
public class AttackSlide
{

    #region - - - - - - Fields - - - - - -

    private MonoBehaviour m_MonoBehaviour;
    private Rigidbody m_PlayerRigidbody;
    private PlayerSettings m_settings;
    private float m_TargetDistance;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -

    /// <summary>
    /// Attack slide Init function
    /// </summary>
    public void Init(MonoBehaviour monoBehaviour, Rigidbody playerRB)
    {
        this.m_MonoBehaviour = monoBehaviour;
        this.m_PlayerRigidbody = playerRB;
        this.m_settings = GameManager.instance.gameSettings.playerSettings;
    }

    #endregion Initializers

    #region - - - - - - Methods - - - - - -

    // Summary: Trigger sliding towards intended enemies
    //
    public void SlideToEnemy(Transform targetEnemy)
    {
        if (CheckWithinMinDist(targetEnemy))
            return;

        this.m_MonoBehaviour.StopCoroutine(SlideOverTime(targetEnemy));
        this.m_MonoBehaviour.StartCoroutine(SlideOverTime(targetEnemy));
    }

    /// <summary>
    /// When there are no enemies the player will just slide forward.
    /// </summary>
    public void SlideForward()
    {
        this.m_MonoBehaviour.StopCoroutine(JustSlideForward());
        this.m_MonoBehaviour.StartCoroutine(JustSlideForward());
    }

    // Summary: Checks whether the player is within the minimum dist against enemy
    //
    private bool CheckWithinMinDist(Transform targetEnemy)
    {
        this.m_TargetDistance = Vector3.Magnitude(this.m_PlayerRigidbody.transform.position - targetEnemy.position);
        return this.m_TargetDistance <= this.m_settings.minimumAttackDist;
    }

    private void FaceTowardsEnemy(Transform targetEnemy)
    {
        Vector3 direction = targetEnemy.position - this.m_PlayerRigidbody.transform.position;
        float angle = Vector3.Angle(this.m_PlayerRigidbody.transform.forward, direction);
        angle *= Vector3.Dot(this.m_PlayerRigidbody.transform.right.normalized, direction.normalized) < 0 ? -1 : 1;

        this.m_PlayerRigidbody.transform.Rotate(0, angle, 0);
    }

    // Summary: Coroutine for sliding player to forward target
    //
    private IEnumerator SlideOverTime(Transform targetEnemy)
    {
        float velocity = this.m_settings.slideSpeed;

        FaceTowardsEnemy(targetEnemy);

        while (velocity > 0)
        {
            if (CheckWithinMinDist(targetEnemy)) yield break;

            velocity -= this.m_settings.slideSpeed * this.m_settings.slideDuration;
            this.m_PlayerRigidbody.position += this.m_PlayerRigidbody.transform.forward * velocity;
            yield return null;
        }
    }

    private IEnumerator JustSlideForward()
    {
        float velocity = this.m_settings.slideSpeed;

        while (velocity > 0)
        {
            velocity -= this.m_settings.slideSpeed * this.m_settings.slideDuration;
            this.m_PlayerRigidbody.position += this.m_PlayerRigidbody.transform.forward * velocity;
            yield return null;
        }
    }

    #endregion Methods
  
}
