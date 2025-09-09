using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;              // Valor de la moneda
    public AudioClip collectSound;         // Sonido al recoger

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Llamamos al Canvas para sumar monedas
            CoinUI coinUI = FindObjectOfType<CoinUI>();
            if (coinUI != null)
            {
                coinUI.AddCoin(coinValue);
            }

            // Reproducimos el sonido de la moneda de manera independiente
            if (collectSound != null)
            {
                // Creamos un objeto temporal para reproducir el sonido
                GameObject audioTemp = new GameObject("TempAudio");
                AudioSource aSource = audioTemp.AddComponent<AudioSource>();
                aSource.clip = collectSound;
                aSource.Play();

                // Destruimos el objeto temporal después de que termine el sonido
                Destroy(audioTemp, collectSound.length);
            }

            // Destruimos la moneda
            Destroy(gameObject);
        }
    }
}
