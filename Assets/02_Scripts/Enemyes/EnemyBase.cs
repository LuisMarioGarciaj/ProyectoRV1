using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IEnemy
{
    [Header("Stats")]
    public float life = 10f;
    public float damage = 2f;
    public float moveSpeed = 5f;

    protected Transform player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public virtual void TakeDamage(float amount)
    {
        life -= amount;
        if (life <= 0) Die();
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
