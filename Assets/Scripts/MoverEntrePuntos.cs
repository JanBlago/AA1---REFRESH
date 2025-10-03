using UnityEngine;

public class MoverEntrePuntos : MonoBehaviour
{
    [Header("Movimiento")]
    public Transform[] puntos;   // Lista de puntos por los que pasa
    public float velocidad = 3f;

    private int indiceActual = 0;

    void Update()
    {
        if (puntos.Length == 0) return;

        // Mover hacia el siguiente punto
        transform.position = Vector3.MoveTowards(
            transform.position,
            puntos[indiceActual].position,
            velocidad * Time.deltaTime
        );

        // Si llega al punto, pasar al siguiente
        if (Vector3.Distance(transform.position, puntos[indiceActual].position) < 0.05f)
        {
            indiceActual = (indiceActual + 1) % puntos.Length; // ciclo infinito
        }
    }
}
