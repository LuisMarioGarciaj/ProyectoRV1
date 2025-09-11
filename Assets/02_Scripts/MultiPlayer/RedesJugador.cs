using UnityEngine;
using Photon.Pun;

public class RedesJugador : MonoBehaviour
{
    public MonoBehaviour[] codigosQueIgnorar;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (!photonView.IsMine)
        {
            // Desactiva los scripts de control que están en el Player
            foreach (var codigo in codigosQueIgnorar)
            {
                codigo.enabled = false;
            }

            // Busca la cámara dentro del jugador y la desactiva
            Camera camara = GetComponentInChildren<Camera>();
            if (camara != null)
                camara.gameObject.SetActive(false);

            // Busca el CameraFollow y lo desactiva también
            CameraFollow follow = GetComponentInChildren<CameraFollow>();
            if (follow != null)
                follow.enabled = false;
        }
    }
}
    