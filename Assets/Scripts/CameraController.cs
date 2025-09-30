using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(PlayerInput))]
public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float maxVerticalAngle = 85.0f;

    public bool isDollhouse = false;
    public Transform minePivot;
    private bool isChangingNode =  false;

    private InputAction lookAction;
    private InputAction rotateAction;

    private bool isRotating = false;
    private float currentPitch = 0.0f;

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
        if (isChangingNode)
        {
            return;
        }
        
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
        
        Vector3 pivotPoint = isDollhouse && minePivot != null 
                             ? minePivot.position 
                             : transform.position;

        float horizontalRotation = -rotationSpeed * mouseDelta.x * Time.deltaTime;
        transform.RotateAround(pivotPoint, Vector3.up, horizontalRotation);

        float pitchRotation = rotationSpeed * mouseDelta.y * Time.deltaTime;

        if (isDollhouse)
        {
            transform.RotateAround(pivotPoint, transform.right, pitchRotation);
        }
        else
        {
            currentPitch += pitchRotation;
            currentPitch = Mathf.Clamp(currentPitch, -maxVerticalAngle, maxVerticalAngle);

            Quaternion targetRotation = Quaternion.Euler(currentPitch, transform.localEulerAngles.y, 0);
            transform.localRotation = targetRotation;
        }
    }

    public void ToggleIsDollHouse(bool boolean)
    {
        isDollhouse = boolean;
        if (isDollhouse)
        {
            transform.localRotation = Quaternion.Euler(41.0f, 162.8f, 0);
            //this.transform.LookAt(minePivot.transform);
        }
    }

        public void ToggleIsChangingNode(bool boolean)
    {
        isChangingNode = boolean;
    }
}