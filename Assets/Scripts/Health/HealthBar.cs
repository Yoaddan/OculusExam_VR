using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class HealthBar : MonoBehaviour
{
    public Slider slider;  // Referencia al Slider UI
    public OVRPlayerController playerController; // Referencia al OVRPlayerController que se desactivará al finalizar el tiempo
    public GameObject defeatCanvas;

    void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }

    // Método público para establecer la salud
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    // Método para decrementar la salud
    public void TakeDamage(int damage)
    {
        slider.value -= damage;
        if (slider.value < 0)
            slider.value = 0;
    }

    // Verificar si la salud es 0 y actuar en consecuencia
    void Update()
    {
        if (slider.value <= 0)
        {
            Die();
        }
    }

    // Manejar la "muerte" del jugador
    private void Die()
    {
        // Logica de muerte
        // Pausar el juego
        defeatCanvas.SetActive(true);
        Time.timeScale = 0f;
        playerController.enabled = false;
    }

}
