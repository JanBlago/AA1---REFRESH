using UnityEngine;

public class MovimientoDiana500 : MonoBehaviour
{
    [Header("Puntuaci�n")]
    public int puntos = 100;          // +100 normal, -100 obst�culo, +500 especial
    public bool destruible = true;    // Obst�culo = false, Targets = true
    public GameObject prefabRoto;     // Solo para los targets destruibles

    [Header("Respawn (solo targets destruible" +
        "s)")]
    public GameObject modeloCompleto;
    public Collider colisionador;
    public float tiempoRespawn = 3f;

    private bool destruido = false;
    private float contadorTiempo = 0f;

    void Update()
    {
        if (destruido)
        {
            contadorTiempo += Time.deltaTime;
            if (contadorTiempo >= tiempoRespawn)
            {
                modeloCompleto.SetActive(true);
                colisionador.enabled = true;
                destruido = false;
                contadorTiempo = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bala"))
        {
            // Aplicar puntuaci�n (positiva o negativa)
            if (ScoreManager.instancia != null)
            {
                ScoreManager.instancia.SumarPuntos(puntos);
            }

            if (destruible && !destruido)
            {
                DestruirEntidad();
            }
        }
    }

    void DestruirEntidad()
    {
        destruido = true;
        modeloCompleto.SetActive(false);
        colisionador.enabled = false;

        if (prefabRoto != null)
        {
            GameObject roto = Instantiate(prefabRoto, transform.position, transform.rotation);
            Destroy(roto, tiempoRespawn);
        }
    }
}
