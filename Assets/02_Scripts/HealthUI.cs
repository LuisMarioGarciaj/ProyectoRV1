using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    [Header("Referencias")]
    public Health playerHealth;  // Aquí arrastramos el Player
    public Slider healthSlider;  // Aquí arrastramos el Slider
    public Text healthText;      // Aquí arrastramos el Text (no TextMeshPro)

    void Start()
    {
        // Configuramos el slider al inicio
        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.currentHealth;
        UpdateHealthText();
    }

    void Update()
    {
        // Actualizamos cada frame la vida en la UI
        healthSlider.value = playerHealth.currentHealth;
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        healthText.text = playerHealth.currentHealth + " / " + playerHealth.maxHealth;
    }
}