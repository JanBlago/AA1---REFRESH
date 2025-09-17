using UnityEngine;

public class TargetDestructible : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject modeloTargetCompleto; // Solo el modelo del target completo, no el GameObject entero
    public Collider colliderTargetCompleto; // El collider marcado como trigger
    public GameObject prefabTargetRoto;

    private GameObject instanciaTargetRoto;
    private bool destruido = false;
    private float temporizador = 0f;
    public float tiempoReaparicion = 3f;

    void Update()
    {
        if (destruido)
        {
            temporizador += Time.deltaTime;

            if (temporizador >= tiempoReaparicion)
            {
                // Reactivar el modelo y collider del target completo
                modeloTargetCompleto.SetActive(true);
                colliderTargetCompleto.enabled = true;

                // Destruir la versión rota
                if (instanciaTargetRoto != null)
                {
                    Destroy(instanciaTargetRoto);
                }

                // Reiniciar estados
                destruido = false;
                temporizador = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Solo reaccionar a balas
        if (destruido) return;
        if (!other.CompareTag("Bala")) return;

        // Marcar como destruido
        destruido = true;

        // Ocultar modelo y desactivar el trigger para que no vuelva a detectar
        modeloTargetCompleto.SetActive(false);
        colliderTargetCompleto.enabled = false;

        // Instanciar la versión rota
        instanciaTargetRoto = Instantiate(prefabTargetRoto, transform.position, transform.rotation);
    }
}
