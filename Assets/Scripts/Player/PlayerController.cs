using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// �̵� ���� ����
    /// </summary>
    [Header("Movement")]
    public float moveSpeed; // �÷��̾� �̵� �ӵ�
    public float jumpPower; // ������ �� ����Ǵ� ���� ũ��
    private float baseJumpPower; // �⺻ ������ ����� (������ ȿ�� ���� �� ����)
    private Vector2 curMovementInput; // ���� �Էµ� �̵� ���� (x: �¿�, y: �յ�)
    public LayerMask groundLayerMask; // � ���̾ �ٴ����� �������� ���� (Raycast��)

    /// <summary>
    /// ī�޶� ȸ�� ���� ����
    /// </summary>
    [Header("Look")]
    public Transform cameraContainer; // ī�޶� ���Ե� ������Ʈ
    public float minXLook; // ī�޶� �Ʒ��� ȸ���� �� �ִ� �ּ� ����
    public float MaxXLook; // ī�޶� ���� ȸ���� �� �ִ� �ִ� ����
    private float camCurXRot; // ���� ī�޶��� x�� ȸ�� ��
    public float lookSensitivity; // ���콺 ȸ�� ���� (�������� ���� ȸ��)

    private Vector2 mouseDelta; // ���콺 �̵� ����
    [HideInInspector]
    public bool canLock = true; // ���콺 Ŀ�� ��� ���� (true�� ���콺 ȸ�� ����)

    public Action inventory;
    private Rigidbody _rigidbody;

    private Coroutine jumpBoostCoroutine; // ���� �ν�Ʈ �ڷ�ƾ ������
    public Condition condition; // ���� �ν�Ʈ UI�� ������ Condition ������Ʈ

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); // Rigidbody�� ������
        baseJumpPower = jumpPower; // �⺻ �������� �����ص�
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ȭ�� �߾ӿ� �����ϰ� ������ �ʰ� ��
    }

    void FixedUpdate()
    {
        Move(); // �̵� �Լ� ȣ��
    }

    private void LateUpdate()
    {
        if (canLock) // ���콺�� ��� ������ ���� ȸ�� ó��
        {
            CameraLook();
        }
    }

    /// <summary>
    /// �̵� ó��
    /// </summary>
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;

        float currentY = _rigidbody.velocity.y;

        _rigidbody.velocity = new Vector3(dir.x, currentY, dir.z);
    }

    /// <summary>
    /// ī�޶� ȸ�� ó��
    /// </summary>
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, MaxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    /// <summary>
    /// �̵� �Է� �̺�Ʈ ó��
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// ���콺 �̵� �Է� ó��
    /// </summary>
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ���� �Է� ó��
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"test {context.phase} {InputActionPhase.Started} {IsGrounded()}");
         if (context.phase == InputActionPhase.Started && IsGrounded())
    {
        // ���¹̳� ��� �õ�: 50��ŭ ���
        if (CharacterManager.Instance.Player.condition.UseStamina(50f))
        {
            Debug.Log("����");
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("���¹̳� �������� ���� �Ұ�");
        }
    }
    }

    /// <summary>
    /// �ٴ� üũ
    /// </summary>
    bool IsGrounded()
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
            Debug.DrawRay(rays[i].origin, rays[i].direction * 1f, Color.red, 1f);
            if (Physics.Raycast(rays[i], 1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// �κ��丮 ��� �Է� �̺�Ʈ �ڵ鷯
    /// </summary>
    public void OnInventory(InputAction.CallbackContext context)
    {
        Debug.Log("�κ��丮 ���� �Ǵ� ����");
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    /// <summary>
    /// Ŀ���� ��� ���¸� ���
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLock = !toggle;
    }

    /// <summary>
    /// ���� �ν�Ʈ ������ ȿ�� ���� (���� �ð� ���� ������ ����)
    /// </summary>
    /// <param name="duration">������ ���� ȿ�� ���� �ð�</param>
    public void ApplyJumpBoost(float duration)
    {
        if (jumpBoostCoroutine != null)
        {
            StopCoroutine(jumpBoostCoroutine); // ���� ȿ�� ����
        }

        jumpBoostCoroutine = StartCoroutine(JumpBoostCoroutine(duration));
    }

    /// <summary>
    /// ���� �ð� ���� �������� ������Ű�� �ڷ�ƾ
    /// </summary>
    /// <param name="duration">ȿ�� ���� �ð�</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator JumpBoostCoroutine(float duration)
    {
        jumpPower = baseJumpPower * 1.2f; // ������ 2�� ����
        Debug.Log("Jump Boost ON");

        // UI ���� ��û
        if (condition != null)
            condition.StartJumpBoostUI(duration);

        yield return new WaitForSeconds(duration); // ������ �ð���ŭ ���
        
        jumpPower = baseJumpPower; // ���� ���������� ����
        Debug.Log("Jump Boost OFF");
    }
}
