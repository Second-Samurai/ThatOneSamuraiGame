using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBreak : MonoBehaviour, IDamageable
{
    public List<Rigidbody> boards = new List<Rigidbody>();
    public BoxCollider thisCol;
    public StatHandler statHandler; //objects with idamageable require this

    private void Start()
    {
        Rigidbody[] children = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody child in children) boards.Add(child);
        thisCol = GetComponent<BoxCollider>();
    }

    public void DisableDamage()
    {
       
    }

    public void EnableDamage()
    {
       
    }

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        thisCol.enabled = false;
        foreach (Rigidbody board in boards)
        {
            board.isKinematic = false;
            board.AddForce((board.transform.position - attacker.transform.position) * 2f, ForceMode.Impulse);
        }
    }

    public bool CheckCanDamage()
    {
        return true;
    }

    public EntityType GetEntityType()
    {
        return EntityType.Destructible;
    }
}
