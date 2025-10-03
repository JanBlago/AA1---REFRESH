using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivot;             // Objeto vac�o en el centro de la pistola
    public Transform cameraTransform;   // La c�mara real

    [Header("Configuraci�n")]
    public float sensitivityX = 100f;
    public float sensitivityY = 100f;
    public float maxDistance = 5f;

    private PlayerControls controls;
    private Vector2 moveInput;
    private float rotationX;
    private float rotationY;

    private void Awake()
    {
        controls = new PlayerControls();
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
        // Rotaci�n con WASD
        rotationX += moveInput.x * sensitivityX * Time.deltaTime;   // izquierda/derecha
        rotationY -= moveInput.y * sensitivityY * Time.deltaTime;   // arriba/abajo
        rotationY = Mathf.Clamp(rotationY, -80f, 80f);

        pivot.rotation = Quaternion.Euler(rotationY, rotationX, 0f);

        // Raycast para colisiones de c�mara
        RaycastHit hit;
        Vector3 desiredPosition = pivot.position - pivot.forward * maxDistance;

        if (Physics.Raycast(pivot.position, -pivot.forward, out hit, maxDistance))
        {
            // Si choca con algo, poner c�mara en el punto de impacto
            cameraTransform.position = hit.point;
        }
        else
        {
            // Si no choca, poner c�mara en distancia m�xima
            cameraTransform.position = desiredPosition;
        }

        cameraTransform.LookAt(pivot);
    }
}