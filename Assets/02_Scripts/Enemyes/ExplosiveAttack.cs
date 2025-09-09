using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveAttack : MonoBehaviour, IExplosive
{
    public void Explode(float damage, float radius, Transform origin)
    {
        Collider[] hits = Physics.OverlapSphere(origin.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
        Destroy(origin.gameObject);
    }

}
