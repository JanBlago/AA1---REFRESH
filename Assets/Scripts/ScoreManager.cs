using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instancia; // Singleton

    [Header("UI")]
    public TextMeshProUGUI textoPuntos;

    private int puntosTotales = 0;

    void Awake()
    {
        // Implementación de Singleton
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarPuntos(int cantidad)
    {
        puntosTotales += cantidad;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (textoPuntos != null)
        {
            textoPuntos.text = "Puntos: " + puntosTotales;
        }
    }
}
