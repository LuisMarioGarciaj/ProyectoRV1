using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject coinPrefab;   // Prefab de la moneda
    public int coinAmount = 5;      // Cantidad de monedas que suelta
    public float spawnRadius = 1f;  // Qué tan dispersas caen
    public float force = 5f;        // Fuerza con la que salen disparadas

    public AudioClip openSound;     // Sonido al abrir cofre (opcional)

    private bool opened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            Debug.Log("💥 El cofre fue abierto");
            opened = true;

            for (int i = 0; i < coinAmount; i++)
            {
                // Posición aleatoria cerca del cofre
                Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPos.y = transform.position.y + 0.2f;

                // Instanciar moneda
                // Instanciar moneda con la rotación y escala del prefab
                GameObject coin = Instantiate(
                    coinPrefab,
                    spawnPos,
                    coinPrefab.transform.rotation,
                    transform.parent // mismo padre que el cofre
                );
                Debug.Log("🪙 Moneda creada en " + spawnPos);

                // Si tiene Rigidbody, le aplicamos un empuje aleatorio
                Rigidbody rb = coin.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 randomDir = Random.onUnitSphere; // Dirección aleatoria
                    randomDir.y = Mathf.Abs(randomDir.y);    // Siempre hacia arriba un poco
                    rb.AddForce(randomDir * force, ForceMode.Impulse);
                }
            }

            // Sonido del cofre
            if (openSound != null)
            {
                AudioSource.PlayClipAtPoint(openSound, transform.position);
            }

            // Destruir cofre
            Destroy(gameObject);
        }
    }
}
