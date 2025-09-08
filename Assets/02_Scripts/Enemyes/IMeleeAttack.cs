using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeleeAttack 
{
    void Melee(Transform attacker, Transform target, float damage, float range);
}
