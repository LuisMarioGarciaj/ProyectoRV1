using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDog : EnemyBase
{
    public float chargeSpeed = 15f;
    public float detectionRange = 10f;
    private bool isCharging = false;

    private IMovement2 movement;
    private IMeleeAttack melee;

    protected override void Start()
    {
        base.Start();
        movement = GetComponent<IMovement2>();
        melee = GetComponent<IMeleeAttack>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < detectionRange && !isCharging)
        {
            StartCoroutine(ChargeAttack());
        }
        else if (!isCharging)
        {
            movement.MoveTowards(player, moveSpeed, transform);
        }
    }

    System.Collections.IEnumerator ChargeAttack()
    {
        isCharging = true;
        Vector3 dir = (player.position - transform.position).normalized;

        float chargeTime = 1f;
        float elapsed = 0f;
        while (elapsed < chargeTime)
        {
            transform.position += dir * chargeSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        isCharging = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            melee.Melee(transform, player, damage, 2f);
            Die();
        }
    }
}
