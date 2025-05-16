using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float MaxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    
    private Vector2 mouseDelta;
    [HideInInspector]
    public bool canLock = true;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLock)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, MaxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("움직인다");
            curMovementInput = context.ReadValue<Vector2>();
        }

        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("멈춘다");
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"test {context.phase} {InputActionPhase.Started} {IsGruonded()}"); // 디버그 확인용
        if (context.phase == InputActionPhase.Started && IsGruonded())
        {
            Debug.Log("점프");
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    bool IsGruonded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + ( transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( -transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( -transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * 10f, Color.red, 1f);
            if (Physics.Raycast(rays[i], 10f, groundLayerMask)) 
            {
                return true;
            }
        }

        return false;
    }
}
