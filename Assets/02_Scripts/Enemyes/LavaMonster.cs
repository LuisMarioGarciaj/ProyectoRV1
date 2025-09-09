using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMonster : EnemyBase
{
    public float attackRange = 10f;
    public float meleeRange = 2f;
    public float shootCooldown = 2f;

    [Header("References")]
    public GameObject fireballPrefab;
    public Transform firePoint;

    private IMovement2 movement;
    private IMeleeAttack melee;
    private IRangedAttack ranged;

    private float shootTimer = 0f;

    protected override void Start()
    {
        base.Start();
        movement = GetComponent<IMovement2>();
        melee = GetComponent<IMeleeAttack>();
        ranged = GetComponent<IRangedAttack>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= meleeRange)
        {
            //revisar
            melee.Melee(transform, player, damage, meleeRange);
        }
        else if (dist <= attackRange)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                ranged.Shoot(fireballPrefab, firePoint, player, damage, 12f);
                shootTimer = shootCooldown;
            }
        }
        else
        {
            movement.MoveTowards(player, moveSpeed, transform);
        }
    }
}
