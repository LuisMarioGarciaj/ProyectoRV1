using UnityEngine;

[System.Serializable]
public class WeaponSlot
{
    public string name;
    public GameObject projectilePrefab;
    public float cooldown = 0.5f;
    [HideInInspector] public float nextFireTime = 0f;
}

public class PlayerCombat : MonoBehaviour
{
    [Header("Slots")]
    public WeaponSlot slot1; // slot principal
    public WeaponSlot slot2; // slot secundario

    [Header("Fire")]
    public Transform firePoint; // punto desde donde salen los proyectiles
    public float firePointRotationSpeed = 10f; // velocidad de giro suave del FirePoint

    private int activeSlotIndex = 0; // 0 = slot1, 1 = slot2

    void Start()
    {
        // seguridad por si no se asignaron
        if (slot1 == null) slot1 = new WeaponSlot() { name = "empty", projectilePrefab = null, cooldown = 0.5f };
        if (slot2 == null) slot2 = new WeaponSlot() { name = "empty", projectilePrefab = null, cooldown = 0.5f };
        if (firePoint == null) Debug.LogWarning("Assign a firePoint transform to PlayerCombat.");
    }

    void Update()
    {
        HandleInput();
        UpdateFirePointDirection();
    }

    void HandleInput()
    {
        // disparo con click izquierdo
        if (Input.GetMouseButton(0))
        {
            TryAttack();
        }

        // rotar/cambiar slot activo con click derecho
        if (Input.GetMouseButtonDown(1))
        {
            CycleActiveSlot();
        }

        // intercambiar slots con espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwapSlotsContents();
        }
    }

    void TryAttack()
    {
        WeaponSlot current = GetActiveSlot();
        if (current == null || current.projectilePrefab == null) return;

        if (Time.time >= current.nextFireTime)
        {
            current.nextFireTime = Time.time + current.cooldown;
            Shoot(current);
        }
    }

    void Shoot(WeaponSlot slot)
    {
        if (firePoint == null) return;
        Instantiate(slot.projectilePrefab, firePoint.position, firePoint.rotation);
    }

    void UpdateFirePointDirection()
    {
        // input WSAD / flechas
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 aimDir = new Vector3(moveX, 0, moveY);

        if (aimDir.sqrMagnitude > 0.01f)
        {
            aimDir.Normalize();

            Quaternion targetRot = Quaternion.LookRotation(aimDir);

            // rotación suave
            firePoint.rotation = Quaternion.Slerp(firePoint.rotation, targetRot, firePointRotationSpeed * Time.deltaTime);
        }
    }

    void CycleActiveSlot()
    {
        if (slot2 != null && slot2.projectilePrefab != null)
        {
            activeSlotIndex = (activeSlotIndex == 0) ? 1 : 0;
            Debug.Log("Active slot: " + (activeSlotIndex + 1) + " -> " + GetActiveSlotName());
        }
    }

    void SwapSlotsContents()
    {
        WeaponSlot tmp = slot1;
        slot1 = slot2;
        slot2 = tmp;

        activeSlotIndex = (activeSlotIndex == 0) ? 1 : 0;

        Debug.Log("Swapped slots. Active slot now: " + (activeSlotIndex + 1) + " -> " + GetActiveSlotName());
    }

    public void EquipWeapon(GameObject projectilePrefab, float cooldown, string weaponName = "Weapon")
    {
        if (projectilePrefab == null) return;

        if (slot2.projectilePrefab == null)
        {
            slot2.name = weaponName;
            slot2.projectilePrefab = projectilePrefab;
            slot2.cooldown = cooldown;
            slot2.nextFireTime = 0f;
            Debug.Log("Equipped " + weaponName + " into slot 2.");
            return;
        }

        WeaponSlot target = (activeSlotIndex == 0) ? slot1 : slot2;
        target.name = weaponName;
        target.projectilePrefab = projectilePrefab;
        target.cooldown = cooldown;
        target.nextFireTime = 0f;
        Debug.Log("Replaced active slot with " + weaponName + ".");
    }

    WeaponSlot GetActiveSlot()
    {
        return (activeSlotIndex == 0) ? slot1 : slot2;
    }

    string GetActiveSlotName()
    {
        WeaponSlot s = GetActiveSlot();
        return (s != null && s.projectilePrefab != null) ? s.name : "empty";
    }
}
