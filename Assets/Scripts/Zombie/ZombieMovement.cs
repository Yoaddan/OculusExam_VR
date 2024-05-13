using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{

    public Transform user;
    private NavMeshAgent enemyAgent;
    public bool userDetect;
    public float delay; //Segundos a esperar para destruir tras animación de muerte.
    public float stoppingDistance = 1.5f; // Distancia de tolerancia para detenerse frente al jugador.
    public float detectionRadius = 10f; // Radio de detección
    private Animator enemyAnimator;
    private bool disparado = false;
    private Vector3 originalPosition; // Posición original del enemigo
    public int health = 500;
    public GameObject zombiePrefab; // Prefab del zombie para respawning
    private HealthBar playerHealthBar; // Referencia a la barra de salud del jugador
    private int lastAttackTime = -1; // Rastrea la última vez que se aplicó el daño
    private Collider[] colliders; // Arreglo para todos los colliders asociados


    public void OnTriggerEnter(Collider other)
    {
        userDetect = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Obtiene los colliders
        colliders = colliders = GetComponents<Collider>();

        // Activa los colliders
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }

        // Busca el transform de la cámara principal usando el tag 'MainCamera'
        if (user == null)
        {
            GameObject cameraGameObject = GameObject.FindGameObjectWithTag("MainCamera");
            if (cameraGameObject != null)
            {
                user = cameraGameObject.transform;
            }
            else
            {
                Debug.LogError("No se encontró ningún objeto con el tag 'MainCamera'");
            }
        }

        // Encuentra la HealthBar por nombre
        GameObject healthBarGameObject = GameObject.Find("Health Bar");
        if (healthBarGameObject != null)
        {
            playerHealthBar = healthBarGameObject.GetComponent<HealthBar>();
            if (playerHealthBar == null)
                Debug.LogError("El componente HealthBar no se encontró en el objeto 'Health Bar'");
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto llamado 'Health Bar'");
        }

        health = 500;
        disparado = false;
        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();  
        originalPosition = transform.position; // Almacena la posición original al inicio
    }

    void Update()
    {
        // Calcula la distancia entre el enemigo y el jugador
        float distanceToUser = Vector3.Distance(transform.position, user.position);
        // Calcula la distancia entre la posición actual y la posición original
        float distanceToOriginal = Vector3.Distance(transform.position, originalPosition);

        if(disparado)
        {
            enemyAnimator.SetInteger("action",3);
            return;
        }

        // Verifica si el jugador está dentro del radio de detección
        if(distanceToUser <= detectionRadius)
        {
            userDetect = true;
        }
        else
        {
            userDetect = false;
        }

        // Si el jugador ha sido detectado, persigue al jugador
        if(userDetect)
        {
            // Si el enemigo está lo suficientemente cerca del jugador, detenerse y reproducir la animación de ataque
            if(distanceToUser <= stoppingDistance)
            {
                enemyAnimator.SetInteger("action", 2);
                AnimatorStateInfo stateInfo = enemyAnimator.GetCurrentAnimatorStateInfo(0);
                if(stateInfo.IsName("Attack"))
                {
                    int currentAttackTime = (int)(stateInfo.normalizedTime * 10); // Multiplique por 10 para obtener un índice más amplio
                    if (currentAttackTime != lastAttackTime && stateInfo.normalizedTime >= 0.5f)
                    {
                        playerHealthBar.TakeDamage(1);
                        lastAttackTime = currentAttackTime; // Actualiza el ciclo de ataque
                    }
                }
            }
            else
            {
                enemyAnimator.SetInteger("action", 1);
                enemyAgent.destination = user.position;
            }  

        }
        else
        {
            // Si el jugador no ha sido detectado, regresa a su posición original
            if (distanceToOriginal <= 0.5f) // Ajusta la tolerancia según sea necesario
            {
                // Si el enemigo está en su posición original, cambia a la animación Idle
                enemyAnimator.SetInteger("action",0);
                //Debug.Log("Cambio a IDLE");
            }
            else
            {
                enemyAgent.destination = originalPosition;
            }
        }
    }

    public void HitByRaycast()
    {
        if (health > 0) // Solo reacciona si aún tiene vida
        {
            health -= 1; // Supongamos que cada golpe quita 50 de vida
            if (health <= 0)
            {
                if(!disparado)
                    TriggerDeath();
            }
        }
    }

    private void TriggerDeath()
    {
        Debug.Log("Zombie muere.");
        enemyAnimator.SetInteger("action", 3); // Asume que 3 es la animación de muerte
        float animTime = enemyAnimator.GetCurrentAnimatorStateInfo(0).length;

        // Desactiva los colliders.
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        Invoke(nameof(Respawn), animTime + delay);
        Destroy(gameObject, animTime + delay); // Asegura destruir después de la animación
        disparado = true;
    }

    private void Respawn()
    {
        Instantiate(zombiePrefab, originalPosition, Quaternion.identity);
    }

}
