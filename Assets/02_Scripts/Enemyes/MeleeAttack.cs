using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeAttack : MonoBehaviour, IMeleeAttack
{
    public void Melee(Transform attacker, Transform target, float damage, float range)
    {
        if (target == null) return;
        float dist = Vector3.Distance(attacker.position, target.position);
        if (dist <= range)
        {
            target.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

}
