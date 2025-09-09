using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRatePotion : MonoBehaviour
{
    public float cooldownMultiplier = 0.5f;
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCombat combat = other.GetComponent<PlayerCombat>();
        if (combat != null)
        {
            combat.StartCoroutine(ApplyFireRate(combat));
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyFireRate(PlayerCombat combat)
    {
        float originalCooldown1 = combat.slot1.cooldown;
        float originalCooldown2 = combat.slot2.cooldown;

        combat.slot1.cooldown *= cooldownMultiplier;
        combat.slot2.cooldown *= cooldownMultiplier;
        Debug.Log("Cadencia aumentada: Slot1 = " + combat.slot1.cooldown + " | Slot2 = " + combat.slot2.cooldown);
        yield return new WaitForSeconds(duration);

        combat.slot1.cooldown = originalCooldown1;
        combat.slot2.cooldown = originalCooldown2;
    }
}
