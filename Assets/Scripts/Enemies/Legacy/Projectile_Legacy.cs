using System;
using System.Collections;
using UnityEngine;

[Obsolete]
public class Projectile_Legacy : MonoBehaviour
{ //script for basic projectile

    TrailRenderer trail;
    GameManager manager;

    int damageAmount = 1;

    float speed = 60f;

    GameObject player;
    public Vector3 direction;
    public Rigidbody rb;
    public bool active = true;
    public bool hitEnemies = false;
    public GameObject arrowModel;
    public Collider collider;

    //initialize
    private void Awake()
    {
        //GameObject managerObject;
        //managerObject = GameObject.FindGameObjectWithTag("Manager");
        //manager = managerObject.GetComponent<GameManager>();
       // player = GameManager.instance.playerController.gameObject;

        rb = GetComponent<Rigidbody>();

        collider = GetComponent<Collider>();
        trail = GetComponentInChildren<TrailRenderer>();
    }

    //launches projectile in target direction
    public void Launch(Vector3 target, Vector3 playerPos)
    {
        EnableMethod();
        transform.LookAt(playerPos);
        direction = target.normalized * speed;
        //direction.y = 0f;
        rb.linearVelocity = direction;
        trail.emitting = true;
        StartCoroutine(Die(3f));
    }

    //sees if the object is damagable
    private void OnTriggerEnter(Collider other)
    {
        DamageCheck(other);
    }

    //if the object is damagable, apply damage, if the player parries the projectile it is reflected
    private void DamageCheck(Collider other)
    {
         
        IDamageable damagable = other.gameObject.GetComponent<IDamageable>();
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerFunctions>().bIsParrying)
            {
                rb.linearVelocity = Vector3.zero;
                rb.linearVelocity = -direction;
                hitEnemies = true; 
                // manager.StartCoroutine("HitPause");
            }
            else if (!hitEnemies)
            {
 
                damagable.OnEntityDamage(damageAmount, this.gameObject, false);
                StartCoroutine(Die(0f));
            }
        }
        else if (other.gameObject.tag == "Attack" && !hitEnemies)
        {
            rb.linearVelocity = Vector3.zero;
            rb.linearVelocity = -direction;
            hitEnemies = true;
            //manager.StartCoroutine("HitPause");
        }
        else if (damagable != null && hitEnemies)
        {
            
            damagable.OnEntityDamage(damageAmount, this.gameObject, false);
            StartCoroutine(Die(0f));

        }
        else if (!other.gameObject.CompareTag("LOD") && 
                 !other.gameObject.CompareTag("Level") && 
                 !other.gameObject.CompareTag("LockOnTracker") &&
                 !hitEnemies)
        {
            StartCoroutine(Die(0f));
        }
    }

    //returns object to pool
    IEnumerator Die(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }
        DisableMethod();
        trail.emitting = false;
        ObjectPooler.instance.AddObject("Arrow", gameObject);
       // Destroy(this.gameObject);
    }

    //disable
    public void DisableMethod()
    {
        arrowModel.SetActive(false);
        collider.enabled = false;
        rb.linearVelocity = Vector3.zero;
        hitEnemies = false;
    }

    //enable
    public void EnableMethod()
    {
        arrowModel.SetActive(true);
        trail.Clear();
        collider.enabled = true;
    }

}
