using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedAttack
{
    void Shoot(GameObject projectilePrefab, Transform firePoint, Transform target, float damage, float speed);
}
