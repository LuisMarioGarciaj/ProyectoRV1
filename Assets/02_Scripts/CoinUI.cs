using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;  // El TextMeshPro del contador
    public AudioSource coinSound;      // Componente AudioSource con el sonido de moneda
    public int coins = 0;

    void Start()
    {
        UpdateUI();
    }

    // Llamar esta función cada vez que se recoja una moneda
    public void AddCoin(int amount)
    {
        coins += amount;
        UpdateUI();
        PlaySound();
    }

    void UpdateUI()
    {
        coinText.text = coins.ToString();
    }

    void PlaySound()
    {
        if (coinSound != null)
        {
            coinSound.Play();
        }
    }
}
