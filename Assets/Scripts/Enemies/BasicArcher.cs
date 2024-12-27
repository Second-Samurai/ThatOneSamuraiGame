﻿using System.Collections;
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
    public float attackRange = 20f, shotTimer = 0f, shotFrequency = 2f, aimDuration = 1.5f, aimCounter = 0f;
    public LineRenderer lineRenderer;

    public GameObject arrow;
    public Animator anim;
    public Collider col;

    public EnemySpawnCheck spawnCheck;

    public AudioPlayer source;
    public AudioClip draw, release;

    public TriggerImpulse camImpulse;

    private AudioManager audioManager;
    EnemyDeathParticleSpawn particleSpawn;

    // Since the player and enemies have their origin point at their feet we need to add an offset value
    private Vector3 _aimOffsetValue;
    private Vector3 playerPos;

    private void Start()
    {
        audioManager = GameManager.instance.audioManager;
        if(GameManager.instance.PlayerController != null)
            player = GameManager.instance.PlayerController.gameObject.transform;
        if(!draw) draw = AudioManager.instance.FindSound("Bow Draw");
        if(!release) release = AudioManager.instance.FindSound("Bow Release");
        //  lineRenderer = GetComponent<LineRenderer>();

        _aimOffsetValue = Vector3.up * transform.localScale.y;
        particleSpawn = GetComponentInChildren<EnemyDeathParticleSpawn>();
    }

    void FindPlayer()
    {
        player = GameManager.instance.PlayerController.gameObject.transform;
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
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                 

                if (Vector3.Distance(transform.position, player.position) <= attackRange && shotTimer >= shotFrequency)
                {
                    //gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    lastDirection = transform.position - player.position;
                    currentState = CurrentState.Aiming;
                    anim.SetTrigger("StartAim");
                    source.PlayOnce(draw, audioManager.SFXVol);
                    RaycastHit hit;
                    playerPos = player.transform.position + _aimOffsetValue;
                    shotDirection = playerPos - shotOrigin.position;
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
                    _arrow.GetComponent<Projectile>().Launch(shotDirection, playerPos);
                    anim.SetTrigger("Fire");
                    source.StopSource();
                    source.PlayOnce(release, audioManager.SFXVol);
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
        // Ignore attacks if the archer is already dead
        if (currentState == CurrentState.Dead) return;
        
        anim.SetTrigger("Death");
        col.enabled = false;
        currentState = CurrentState.Dead;
        camImpulse.FireImpulse();
        GameManager.instance.gameObject.GetComponent<HitstopController>().Hitstop(.15f);
        StartCoroutine(DodgeImpulseCoroutine(Vector3.back, damage * 4, .3f));
        
        LockOnTracker lockOnTracker = GameManager.instance.LockOnTracker;
        
        // Finds a new target on the enemy tracker (only if the dying enemy was the locked on enemy)
        lockOnTracker.SwitchDeathTarget(transform);
        lockOnTracker.RemoveEnemy(transform);
        GameManager.instance.EnemyTracker.RemoveEnemy(transform);

        if (particleSpawn) particleSpawn.SpawnParticles();

        lineRenderer.enabled = false;


        //Invoke("HideArcher", 2.0f);
    }

    private IEnumerator DodgeImpulseCoroutine(Vector3 lastDir, float force, float timer)
    {
        float dodgeTimer = timer;
        while (dodgeTimer > 0f)
        {
            transform.Translate(lastDir.normalized * force * Time.deltaTime);

            dodgeTimer -= Time.deltaTime;
            yield return null;
        }
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
