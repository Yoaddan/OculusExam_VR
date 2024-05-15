using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShooter : MonoBehaviour
{
    public GameObject zombieParticles;  // Prefab de partículas para zombies
    public GameObject defaultParticles;  // Prefab de partículas por defecto
    public GameObject fireParticles;  // Prefab de partículas por defecto
    public float rayLength = 100f;  // Longitud del raycast
    public Transform rayOriginTransform;  // Asigna el transform de origen del rayo
    public AudioSource fireSound;

    private bool isFiring = false;  // Indica si el botón está siendo presionado

    void Start()
    {
        if (rayOriginTransform == null)
        {
            Debug.LogError("Transform de origen del rayo no asignado.");
            this.enabled = false;
            return;
        }
    }

    void Update()
    {
        // Comprueba si el botón está siendo presionado
        isFiring = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) == 1f;

        // Si el botón está siendo presionado, dispara y reproduce el sonido en bucle
        if (isFiring)
        {
            ShootRaycast();
            if (!fireSound.isPlaying)
            {
                fireSound.loop = true;
                fireSound.Play();
            }
        }
        else
        {
            // Si el botón no está siendo presionado, detiene la reproducción del sonido
            if (fireSound.isPlaying)
            {
                fireSound.loop = false;
                fireSound.Stop();
            }
        }
    }

    void ShootRaycast()
    {
        RaycastHit hit;
        Vector3 start = rayOriginTransform.position;
        Vector3 end = rayOriginTransform.TransformDirection(Vector3.forward) * rayLength;

        if (Physics.Raycast(start, end, out hit, rayLength))
        {
            // Comprueba el tag del objeto y genera las partículas correspondientes
            if (hit.transform.tag == "Zombie")
            {
                ZombieMovement zombie = hit.collider.GetComponent<ZombieMovement>();
                GameObject b = Instantiate(zombieParticles, hit.point + hit.normal * 0.2f, Quaternion.identity);
                b.transform.SetParent(hit.collider.transform); // Establece el padre del objeto b al objeto golpeado
                Destroy(b, 1);
                zombie.HitByRaycast(); // Llama a la función que maneja el impacto en el script del zombie
            }
            else
            {
                if (hit.transform.tag != "UI" && hit.transform.tag != "Wall" )
                {
                    GameObject c = Instantiate(defaultParticles, hit.point, Quaternion.identity);
                    Destroy(c, 1);
                }
            }

            GameObject a = Instantiate(fireParticles, start, Quaternion.identity);
            // Establece el objeto instanciado como hijo del objeto que tiene este script
            a.transform.SetParent(this.transform);
            Destroy(a, 0.3f);
            
        }
    }
}
