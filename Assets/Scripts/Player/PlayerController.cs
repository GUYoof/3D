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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); // Rigidbody�� ������
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // ���콺 Ŀ���� ȭ�� �߾ӿ� �����ϰ� ������ �ʰ� ��
    }

    /// <summary>
    /// ���� ������ �ʿ��� �����Ӹ��� ȣ���
    /// </summary>
    void FixedUpdate()
    {
        Move(); // �̵� �Լ� ȣ��
    }

    /// <summary>
    /// ��� Update�� ���� �� ȣ��Ǹ�, ī�޶� ȸ���� ����ϸ� �ε巯��
    /// </summary>
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
        // �̵� ���� ��� (forward: ��/��, right: ��/��)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; // �̵� �ӵ� ����

        // Y���� �߷¿� ���� �ڿ������� ó���ǵ��� ���� �ӵ� ����
        dir.y = _rigidbody.velocity.y;

        // Rigidbody�� ���� �̵� ���� ����
        _rigidbody.velocity = dir;
    }

    /// <summary>
    /// ī�޶� ȸ�� ó��
    /// </summary>
    void CameraLook()
    {
        // ���콺 Y�� �̵����� �����Ͽ� ī�޶� ȸ�� ������ �ݿ� (���� ȸ��)
        camCurXRot += mouseDelta.y * lookSensitivity;
        // ���� ȸ�� ���� ���� (�� �ʹ� ��/�Ʒ��� �� ������)
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, MaxXLook);

        // ī�޶� �����̳� ������Ʈ�� ȸ���� ���� (ī�޶� ���Ʒ� ������)
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // �¿� ȸ���� �÷��̾� ��ü�� ȸ����Ŵ (���콺 X��)
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    /// <summary>
    /// �̵� �Է� �̺�Ʈ ó��
    /// </summary>
    /// <param name="context">Input System���� �����ϴ� �Է� �̺�Ʈ ������</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // �̵� Ű �Է� ���� �� ���Ⱚ ����
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // �̵� Ű���� ���� ���� �� �Է� �ʱ�ȭ
            curMovementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// ���콺 �̵� �Է� ó��
    /// </summary>
    /// <param name="context">Input System���� ���޵Ǵ� ���콺 �Է� �̺�Ʈ ������</param>
    public void OnLook(InputAction.CallbackContext context)
    {
        // ���콺 �̵� �Է� ���� ����
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// ���� �Է� ó��
    /// </summary>
    /// <param name="context">Input System���� ���޵Ǵ� ���� Ű �Է� �̺�Ʈ ������</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        // ���� Ű�� ���Ȱ� �ٴڿ� ���� ���� ����
        Debug.Log($"test {context.phase} {InputActionPhase.Started} {IsGruonded()}"); // ���� Ȯ�ο� �α�
        if (context.phase == InputActionPhase.Started && IsGruonded())
        {
            Debug.Log("����");
            // Rigidbody�� �� �������� ���� ����
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// �ٴ� üũ
    /// </summary>
    /// <returns></returns>
    bool IsGruonded()
    {
        // �÷��̾� �ֺ� �� ���⿡�� �Ʒ��� ������ ��� (��¦ ������ ������)
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + ( transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( -transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( -transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // �� ���̸��� �ٴ��� �����Ǵ��� Ȯ��
        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * 1f, Color.red, 1f); // Scene���� ���̵��� ����� ���� �׸���

            // ������ ���̾�(groundLayerMask)�� 0.1���� �Ÿ����� �浹�� �����Ǹ� �ٴ����� �ν�
            if (Physics.Raycast(rays[i], 1f, groundLayerMask))
            {
                return true;
            }
        }

        return false; // 4�� ���� ��� �ٴ��� �������� ������ ���
    }

    /// <summary>
    /// �κ��丮 ��� �Է� �̺�Ʈ �ڵ鷯
    /// �Է� �׼��� ���۵� �� �κ��丮 ���¸� ����ϰ� Ŀ�� ��� ���¸� ����
    /// </summary>
    /// <param name="context">�Է� �׼��� �ݹ� ���ؽ�Ʈ</param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        // �κ��丮 ��� �Է��� ���۵� �� ȣ���
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();   // �κ��丮 ����/�ݱ� �̺�Ʈ ȣ��
            ToggleCursor();        // Ŀ�� ��� ���� ���
        }
    }

    /// <summary>
    /// Ŀ���� ��� ���¸� ���
    /// ��� ���°� Locked�� None����, None�̸� Locked�� �����ϸ�, Ŀ�� ��� ���� ����(canLock) �÷��׵� ����
    /// </summary>
    void ToggleCursor()
    {
        // ���� Ŀ���� ��� �������� Ȯ��
        bool toggle = Cursor.lockState == CursorLockMode.Locked;

        // ��������� �����ϰ�, �����Ǿ� ������ ��� ���·� ����
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;

        // ��� ���� ���� ����
        canLock = !toggle;
    }

}
