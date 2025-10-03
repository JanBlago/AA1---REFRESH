using UnityEngine;
using UnityEngine.InputSystem; // Importante para el nuevo Input System

public class CamaraOrbital : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensibilidadHorizontal = 100f;
    public float sensibilidadVertical = 80f;

    [Header("Distancia")]
    public float distanciaMaxima = 5f;
    public float distanciaMinima = 0.5f;
    public float suavizado = 10f;

    [Header("Referencias")]
    public Transform camara;   // Asigna aquí la Main Camera
    public InputActionReference moveAction; // Referencia al input "Move"

    private float rotX = 0f;
    private float rotY = 0f;

    void Start()
    {
        Vector3 rotacionInicial = transform.eulerAngles;
        rotY = rotacionInicial.y;
        rotX = rotacionInicial.x;
    }

    void Update()
    {
        // --- Leer el input del nuevo sistema ---
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        rotY += input.x * sensibilidadHorizontal * Time.deltaTime;
        rotX += -input.y * sensibilidadVertical * Time.deltaTime; // invertido para naturalidad

        rotX = Mathf.Clamp(rotX, -60f, 60f);

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);

        // --- Colisión de cámara ---
        Vector3 direccion = -transform.forward;
        RaycastHit hit;

        float distanciaDeseada = distanciaMaxima;

        if (Physics.Raycast(transform.position, direccion, out hit, distanciaMaxima))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                distanciaDeseada = Mathf.Clamp(hit.distance, distanciaMinima, distanciaMaxima);
            }
        }

        Vector3 posicionObjetivo = transform.position + direccion * distanciaDeseada;
        camara.position = Vector3.Lerp(camara.position, posicionObjetivo, Time.deltaTime * suavizado);

        camara.LookAt(transform);
    }
}
