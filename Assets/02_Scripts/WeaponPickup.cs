using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponProjectilePrefab; // el prefab que se disparará (Rock, CubitoHielo, etc)
    public float weaponCooldown = 0.5f;       // cooldown del arma
    public string weaponName = "Weapon";      // nombre para debug

    void OnTriggerEnter(Collider other)
    {
        PlayerCombat pc = other.GetComponent<PlayerCombat>();
        if (pc == null)
        {
            pc = other.GetComponentInParent<PlayerCombat>();
        }

        if (pc != null)
        {
            pc.EquipWeapon(weaponProjectilePrefab, weaponCooldown, weaponName);
            Destroy(gameObject); // recogido
        }
    }
}
