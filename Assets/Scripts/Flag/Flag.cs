using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject victoryCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Activar el Canvas de victoria cuando el jugador colisiona con la bandera
            if (victoryCanvas != null)
            {
                victoryCanvas.SetActive(true);
                Time.timeScale = 0f; // Pausar el juego al activar el Canvas de victoria
                // Desactivar el script OVRPlayerController
                DesactivarScript(other.GetComponent<OVRPlayerController>());
            }
        }
    }

    // Método para desactivar un script en un componente MonoBehaviour
    private void DesactivarScript(MonoBehaviour script)
    {
        if (script != null)
        {
            script.enabled = false;
        }
    }
}
