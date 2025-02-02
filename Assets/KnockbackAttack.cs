using UnityEngine;

public class KnockbackAttack : MonoBehaviour
{
    public Collider col;
    public float knockbackAmount = 10f, duration = .4f;
    public Transform enemy;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    public void KBColOn()
    {
        col.enabled = true;
    }

    public void KBColOff()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 dir = enemy.position - other.transform.position;
            dir.y = 0;
            other.GetComponent<PlayerFunctions>().Knockback(knockbackAmount, -dir, duration, enemy.gameObject);
        }
    }
}
