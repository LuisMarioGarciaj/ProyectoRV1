using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int extraHealth = 50;
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        Health hp = other.GetComponent<Health>();
        if (hp != null)
        {
            StartCoroutine(ApplyExtraHealth(hp)); 
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyExtraHealth(Health hp)
    {
        hp.maxHealth += extraHealth;
        hp.currentHealth += extraHealth;

        yield return new WaitForSeconds(duration);

        hp.maxHealth -= extraHealth;
        if (hp.currentHealth > hp.maxHealth)
            hp.currentHealth = hp.maxHealth;
    }
}
