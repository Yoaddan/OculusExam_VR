using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float initialTime = 300f; // 5 minutos en segundos
    private float currentTime;
    public OVRPlayerController playerController; // Referencia al OVRPlayerController que se desactivará al finalizar el tiempo
    public TMPro.TextMeshProUGUI timerText;
    public GameObject defeatCanvas;

    void Start()
    {
        currentTime = initialTime;
    }

    void Update()
    {
        // Reducir el tiempo restante
        currentTime -= Time.deltaTime;

        // Actualizar el texto del temporizador en la interfaz de usuario
        DisplayTime(currentTime);

        // Verificar si el tiempo restante ha llegado a cero
        if (currentTime <= 0)
        {
            EndGame(); // Aquí deberías implementar tu lógica de derrota
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        // Convertir el tiempo a minutos y segundos
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // Actualizar el texto en el formato MM:SS
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame()
    {
        // Pausar el juego
        defeatCanvas.SetActive(true);
        Time.timeScale = 0f;
        playerController.enabled = false;
    }
}
