using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBullet : MonoBehaviour, IEnemyBullet
{
    private float damage;
    private Transform target;

    public float lifeTime = 5f;
    public float speed = 12f;

    public void Init(float damage, Transform target = null)
    {
        this.damage = damage;
        this.target = target;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target == null) return;
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
