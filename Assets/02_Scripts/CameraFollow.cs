using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Arrastra tu jugador aquí en el Inspector
    public float height = 20f; // Altura de la cámara sobre el jugador

    void LateUpdate()
    {
        if (player != null)
        {
            // Posición directamente sobre el jugador
            transform.position = new Vector3(player.position.x, player.position.y + height, player.position.z);

            // Mira directamente hacia abajo
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}
