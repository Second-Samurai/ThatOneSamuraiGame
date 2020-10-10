using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicArcher : MonoBehaviour, IDamageable
{
    public Transform player, shotOrigin;
    //public enum CurrentState
    //{
    //    Idle,
    //    Aiming,
    //    Dead
    //}
    public CurrentState currentState;
    public Vector3 lastDirection, shotDirection;
    public float attackRange = 20f, shotTimer = 0f, shotFrequency = 2f, aimDuration = .5f, aimCounter = 0f;
    public LineRenderer lineRenderer;

    public GameObject arrow;
    public Animator anim;
    public Collider col;

    public EnemySpawnCheck spawnCheck;

    public AudioPlayer source;
    public AudioClip draw, release;
    

    private void Start()
    {
        if(GameManager.instance.playerController != null)
            player = GameManager.instance.playerController.gameObject.transform;
        if(!draw) draw = AudioManager.instance.FindSound("Bow Draw");
        if(!release) release = AudioManager.instance.FindSound("Bow Release");
      //  lineRenderer = GetComponent<LineRenderer>();
    }

    void FindPlayer()
    {
        player = GameManager.instance.playerController.gameObject.transform;
    }

    private void Update()
    {
         
        spawnCheck.bSpawnMe = currentState != CurrentState.Dead ? true : false;

        if (player == null)
            FindPlayer();
        else
        {
            if (currentState == CurrentState.Idle)
            {
                transform.LookAt(player);

                if (Vector3.Distance(transform.position, player.position) <= attackRange && shotTimer >= shotFrequency)
                {
                    //gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    lastDirection = transform.position - player.position;
                    currentState = CurrentState.Aiming;
                    anim.SetTrigger("StartAim");
                    source.PlayOnce(draw);
                    RaycastHit hit;
                    shotDirection = player.transform.position - shotOrigin.position + Vector3.up * transform.localScale.y;
                    if (Physics.Raycast(shotOrigin.position, shotDirection, out hit, Mathf.Infinity))
                    {
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, shotOrigin.position);
                        lineRenderer.SetPosition(1, hit.point);

                    }

                }
                else if (shotTimer < shotFrequency)
                {
                    shotTimer += Time.deltaTime;
                }
            }
            else if(currentState == CurrentState.Aiming)
            {
                if(aimCounter >= aimDuration)
                {
                    GameObject _arrow = ObjectPooler.instance.ReturnObject("Arrow");
                    //GameObject _arrow = Instantiate(arrow, shotOrigin.position, Quaternion.identity);
                    _arrow.transform.position = shotOrigin.position;
                    _arrow.GetComponent<Projectile>().Launch(shotDirection, player.transform.position);
                    anim.SetTrigger("Fire");
                    source.StopSource();
                    source.PlayOnce(release);
                    shotTimer = 0f;
                    lineRenderer.enabled = false;
                    aimCounter = 0f;
                    currentState = CurrentState.Idle;
                }
                else
                {
                    aimCounter += Time.deltaTime;
                }
            }
        }
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        anim.SetTrigger("Death");
        //col.enabled = false;
        currentState = CurrentState.Dead;
        //Debug.LogError("I Am dead");
        
        EnemyTracker enemyTracker = GameManager.instance.enemyTracker;
        enemyTracker.RemoveEnemy(transform);
        
        //Invoke("HideArcher", 2.0f);
    }

    public void DisableDamage()
    {
         
    }

    public void EnableDamage()
    {
         
    }

    private void HideArcher()
    {
        gameObject.SetActive(false);
    }

    public bool CheckCanDamage()
    {
        return true;
    }

    public EntityType GetEntityType()
    {
        return EntityType.Enemy;
    }

}
//moved down here for referencing
public enum CurrentState
{
    Idle,
    Aiming,
    Dead
}
