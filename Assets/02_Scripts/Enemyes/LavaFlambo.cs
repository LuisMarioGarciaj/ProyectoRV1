using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFlambo : EnemyBase
{
    public float explosionRadius = 3f;

    private IMovement2 movement;
    private IExplosive explosive;

    protected override void Start()
    {
        base.Start();
        movement = GetComponent<IMovement2>();
        explosive = GetComponent<IExplosive>();
    }

    void Update()
    {
        movement.MoveTowards(player, moveSpeed, transform);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            explosive.Explode(damage, explosionRadius, transform);
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        if (life <= 0)
        {
            explosive.Explode(damage, explosionRadius, transform);
        }
    }
}
