using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivot;             // Objeto vacío en el centro de la pistola
    public Transform cameraTransform;   // La cámara real

    [Header("Configuración")]
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;
    public float maxDistance = 5f;

    [Header("Rotación Inicial (en grados)")]
    public float startRotationX = 0f;   // Yaw (izquierda/derecha)
    public float startRotationY = 20f;  // Pitch (arriba/abajo)

    private PlayerControls controls;
    private Vector2 moveInput;
    private float rotationX;
    private float rotationY;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        // ✅ Inicializar la cámara con los grados definidos
        rotationX = startRotationX;
        rotationY = startRotationY;
        pivot.rotation = Quaternion.Euler(rotationY, rotationX, 0f);
    }

    private void OnEnable()
    {
        controls.Camera.Enable();
        controls.Camera.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Camera.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        controls.Camera.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Camera.Move.canceled -= ctx => moveInput = Vector2.zero;
        controls.Camera.Disable();
    }

    private void Update()
    {
        // Rotación con WASD
        rotationX += moveInput.x * sensitivityX * Time.deltaTime;   // izquierda/derecha
        rotationY -= moveInput.y * sensitivityY * Time.deltaTime;   // arriba/abajo
        rotationY = Mathf.Clamp(rotationY, -80f, 80f);

        pivot.rotation = Quaternion.Euler(rotationY, rotationX, 0f);

        // Raycast para colisiones de cámara
        RaycastHit hit;
        Vector3 desiredPosition = pivot.position - pivot.forward * maxDistance;

        if (Physics.Raycast(pivot.position, -pivot.forward, out hit, maxDistance))
        {
            cameraTransform.position = hit.point;
        }
        else
        {
            cameraTransform.position = desiredPosition;
        }

        cameraTransform.LookAt(pivot);
    }
}
