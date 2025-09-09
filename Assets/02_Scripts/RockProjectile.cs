using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    private Collider playerCollider;

    void Start()
    {
        // Buscar al Player por tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider>();
            Collider myCollider = GetComponent<Collider>();

            // Ignorar colisión entre proyectil y Player
            if (playerCollider != null && myCollider != null)
            {
                Physics.IgnoreCollision(myCollider, playerCollider);
            }
        }

        // Destruir proyectil después del tiempo de vida
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mover proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Aquí el proyectil solo colisionará con otros objetos, no con el Player
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Impacto en: " + other.name);
            Destroy(gameObject);
        }
    }
}
