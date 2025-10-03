using UnityEngine;

public class TargetController : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject modeloCompleto;   // El mesh del target completo
    public Collider colisionador;       // El collider del target completo
    public GameObject prefabRoto;       // Prefab con el target roto

    [Header("Respawn")]
    public float tiempoRespawn = 3f;

    private bool destruido = false;
    private float contadorTiempo = 0f;

    void Update()
    {
        // Si está destruido, contamos segundos
        if (destruido)
        {
            contadorTiempo += Time.deltaTime;

            if (contadorTiempo >= tiempoRespawn)
            {
                // Respawnear
                modeloCompleto.SetActive(true);
                colisionador.enabled = true;
                destruido = false;
                contadorTiempo = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bala") && !destruido)
        {
            DestruirTarget();
        }
    }

    void DestruirTarget()
    {
        destruido = true;

        // Ocultar el target completo
        modeloCompleto.SetActive(false);
        colisionador.enabled = false;

        // Spawnear el target roto en la misma posición y rotación
        GameObject roto = Instantiate(prefabRoto, transform.position, transform.rotation);

        // El roto se autodestruye a los X segundos
        Destroy(roto, tiempoRespawn);
    }
}
