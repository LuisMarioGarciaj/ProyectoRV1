using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IRangedAttack
{
    public void Shoot(GameObject projectilePrefab, Transform firePoint, Transform target, float damage, float speed)
    {
        if (projectilePrefab == null || firePoint == null || target == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 dir = (target.position - firePoint.position).normalized;
            rb.velocity = dir * speed;
        }

       // Projectile p = proj.GetComponent<Projectile>();
        //if (p != null) p.damage = damage;
    }
}