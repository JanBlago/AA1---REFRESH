using UnityEngine;
using UnityEngine.InputSystem;

public class TorretaController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivoteBase;
    public Transform pivoteElevado;
    public Transform boquilla; // Objeto vacío en la punta

    [Header("Proyectil")]
    public GameObject proyectilPrefab;
    public float fuerzaDisparo = 500f;

    [Header("Sensibilidad")]
    public float sensibilidadX = 1f;
    public float sensibilidadY = 1f;

    [Header("Límites de rotación vertical")]
    public float minRotY = -45f;
    public float maxRotY = 45f;

    private float rotacionXActual = 0f; // Rotación horizontal (base)
    private float rotacionYActual = 0f; // Rotación vertical (torso)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 deltaMouse = Mouse.current.delta.ReadValue();

        // Actualizar rotación horizontal y limitar
        rotacionXActual += deltaMouse.x * sensibilidadX;
        rotacionXActual = Mathf.Clamp(rotacionXActual, -90f, 90f);
        pivoteBase.localRotation = Quaternion.Euler(0f, rotacionXActual, 0f);

        // Actualizar rotación vertical y limitar (invertido para que al subir el ratón suba el cañón)
        rotacionYActual += deltaMouse.y * sensibilidadY;
        rotacionYActual = Mathf.Clamp(rotacionYActual, minRotY, maxRotY);
        pivoteElevado.localRotation = Quaternion.Euler(rotacionYActual, 0f, 0f);

        // Disparo con espacio
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Disparar();
        }

        // Liberar cursor con ESC
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Disparar()
    {
        // Instanciar proyectil con la rotación exacta de la boquilla (que debe combinar pivote base y pivote elevado)
        GameObject nuevoProyectil = Instantiate(proyectilPrefab, boquilla.position, boquilla.rotation);

        Rigidbody rb = nuevoProyectil.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplicar fuerza en la dirección forward del proyectil (misma que boquilla)
            rb.AddForce(nuevoProyectil.transform.forward * fuerzaDisparo);
        }
    }

    // Opcional: dibujar un gizmo para visualizar la dirección del disparo
    void OnDrawGizmos()
    {
        if (boquilla != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(boquilla.position, boquilla.forward * 2f);
        }
    }
}
