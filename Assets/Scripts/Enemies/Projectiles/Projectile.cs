using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Enumeration;
using UnityEngine;

public class Projectile : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public Rigidbody m_Rigidbody;
    public float m_ProjectileSpeed;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_Rigidbody = this.GetComponent<Rigidbody>();
        this.m_Rigidbody.linearVelocity = this.transform.forward * this.m_ProjectileSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == GameLayer.Enemy) return;

        if (other.gameObject.layer == GameLayer.Player)
        {
            GameLogger.Log("Has hit player");
            this.DestroySelf();
        }
        else if (other.gameObject.layer == GameLayer.Enemy)
        {
            this.DestroySelf();
        }
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    private void DestroySelf()
        => Destroy(this.gameObject);

    #endregion Methods

}
