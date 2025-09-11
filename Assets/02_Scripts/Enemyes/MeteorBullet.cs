using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBullet : MonoBehaviour, IEnemyBullet
{
    private float damage;

    public float fallSpeed = 15f;
    public float explosionRadius = 4f;

    public void Init(float damage, Transform target = null)
    {
        this.damage = damage;
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision col)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
        Destroy(gameObject);
    }
}
