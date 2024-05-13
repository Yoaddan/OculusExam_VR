using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    // Función para cargar el menu
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Función para cargar el nivel
    public void LoadLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    // Función para salir del juego
    public void ExitGame()
    {
        Application.Quit();
    }


}
