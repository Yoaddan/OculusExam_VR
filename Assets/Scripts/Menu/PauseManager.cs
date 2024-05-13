using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas; // Referencia al canvas que se mostrará al pausar la escena
    public OVRPlayerController playerController; // Referencia al OVRPlayerController que se desactivará al finalizar el tiempo

    private bool _isPaused = false;

    void Start()
    {
        // Establecer el timeScale en 1 al inicio
        Time.timeScale = 1f;
        playerController.enabled = true;

        // Asegurarse de que el canvas de pausa esté desactivado al inicio
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Verificar la entrada para pausar y despausar la escena
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                if(!IsUIActive())
                    PauseGame();
            }
        }
    }

    // Método para pausar la escena
    void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f; // Congelar el tiempo
        playerController.enabled = false;

        // Mostrar el canvas de pausa si está asignado
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(true);
        }
    }

    // Método para despausar la escena
    void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f; // Reanudar el tiempo
        playerController.enabled = true;

        // Ocultar el canvas de pausa si está asignado
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
    }

     // Método para verificar si hay algún elemento con el tag "UI" activo en la escena
    bool IsUIActive()
    {
        GameObject[] uiElements = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject uiElement in uiElements)
        {
            if (uiElement.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}
