using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 1.0f; 

    private InputAction lookAction;
    private InputAction rotateAction;

    private bool isRotating = false;

    void Awake()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        rotateAction = playerInput.actions["Rotate"];
    }

    void OnEnable()
    {
        rotateAction.performed += _ => { isRotating = true; };
        rotateAction.canceled += _ => { isRotating = false; };
    }

    void OnDisable()
    {
        rotateAction.performed -= _ => { isRotating = true; };
        rotateAction.canceled -= _ => { isRotating = false; };
    }

    void Update()
    {
        if (!isRotating)
        {
            return;
        }
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

        transform.RotateAround(transform.position, -Vector3.up, rotationSpeed * mouseDelta.x * Time.deltaTime);
        transform.RotateAround(transform.position, -transform.right, -rotationSpeed * mouseDelta.y * Time.deltaTime);
    }
}
