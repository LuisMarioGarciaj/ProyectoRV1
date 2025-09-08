using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaKing : EnemyBase
{
    [Header("References")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public GameObject meteorPrefab;
    public GameObject minionDogPrefab;
    public GameObject minionFlamePrefab;

    private IRangedAttack ranged;
    private IMeleeAttack melee;
    private IMovement2 movement;

    private int phase = 1;
    private float attackTimer = 0f;

    protected override void Start()
    {
        base.Start();
        ranged = GetComponent<IRangedAttack>();
        melee = GetComponent<IMeleeAttack>();
        movement = GetComponent<IMovement2>();
    }

    void Update()
    {
        if (player == null) return;

        if (life < 70) phase = 2;
        if (life < 40) phase = 3;

        attackTimer -= Time.deltaTime;

        switch (phase)
        {
            case 1: PhaseOne(); break;
            case 2: PhaseTwo(); break;
            case 3: PhaseThree(); break;
        }
    }

    void PhaseOne()
    {
        if (attackTimer <= 0)
        {
            //revisar
            ranged.Shoot(fireballPrefab, firePoint, player, damage, 10f);
            attackTimer = 1.5f;
        }
    }

    void PhaseTwo()
    {
        movement.MoveTowards(player, moveSpeed, transform);

        if (Vector3.Distance(transform.position, player.position) < 2f && attackTimer <= 0)
        {
            //reivsar
            melee.Melee(transform, player, damage, 2f);
            attackTimer = 2f;
        }
    }

    void PhaseThree()
    {
        if (attackTimer <= 0)
        {
            // lluvia de meteoritos
            for (int i = 0; i < 5; i++)
            {
                Vector3 pos = player.position + new Vector3(Random.Range(-5, 5), 10, Random.Range(-5, 5));
                Instantiate(meteorPrefab, pos, Quaternion.identity);
            }

            // invocar minions
            Instantiate(minionDogPrefab, transform.position + Vector3.right * 3, Quaternion.identity);
            Instantiate(minionFlamePrefab, transform.position + Vector3.left * 3, Quaternion.identity);

            attackTimer = 5f;
        }
    }
}
