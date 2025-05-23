using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 이동 관련 설정
    /// </summary>
    [Header("Movement")]
    public float moveSpeed; // 플레이어 이동 속도
    public float jumpPower; // 점프할 때 적용되는 힘의 크기
    private float baseJumpPower; // 기본 점프력 저장용 (아이템 효과 종료 시 복원)
    private Vector2 curMovementInput; // 현재 입력된 이동 방향 (x: 좌우, y: 앞뒤)
    public LayerMask groundLayerMask; // 어떤 레이어를 바닥으로 간주할지 설정 (Raycast용)

    /// <summary>
    /// 카메라 회전 관련 설정
    /// </summary>
    [Header("Look")]
    public Transform cameraContainer; // 카메라가 포함된 오브젝트
    public float minXLook; // 카메라가 아래로 회전할 수 있는 최소 각도
    public float MaxXLook; // 카메라가 위로 회전할 수 있는 최대 각도
    private float camCurXRot; // 현재 카메라의 x축 회전 값
    public float lookSensitivity; // 마우스 회전 감도 (높을수록 빨리 회전)

    private Vector2 mouseDelta; // 마우스 이동 방향
    [HideInInspector]
    public bool canLock = true; // 마우스 커서 잠금 여부 (true면 마우스 회전 가능)

    public Action inventory;
    private Rigidbody _rigidbody;

    private Coroutine jumpBoostCoroutine; // 점프 부스트 코루틴 추적용
    public Condition condition; // 점프 부스트 UI를 갱신할 Condition 컴포넌트

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); // Rigidbody를 가져옴
        baseJumpPower = jumpPower; // 기본 점프력을 저장해둠
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 중앙에 고정하고 보이지 않게 함
    }

    void FixedUpdate()
    {
        Move(); // 이동 함수 호출
    }

    private void LateUpdate()
    {
        if (canLock) // 마우스가 잠금 상태일 때만 회전 처리
        {
            CameraLook();
        }
    }

    /// <summary>
    /// 이동 처리
    /// </summary>
    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;

        float currentY = _rigidbody.velocity.y;

        _rigidbody.velocity = new Vector3(dir.x, currentY, dir.z);
    }

    /// <summary>
    /// 카메라 회전 처리
    /// </summary>
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, MaxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    /// <summary>
    /// 이동 입력 이벤트 처리
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
    /// 마우스 이동 입력 처리
    /// </summary>
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 점프 입력 처리
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"test {context.phase} {InputActionPhase.Started} {IsGrounded()}");
         if (context.phase == InputActionPhase.Started && IsGrounded())
    {
        // 스태미나 사용 시도: 50만큼 사용
        if (CharacterManager.Instance.Player.condition.UseStamina(50f))
        {
            Debug.Log("점프");
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("스태미나 부족으로 점프 불가");
        }
    }
    }

    /// <summary>
    /// 바닥 체크
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
    /// 인벤토리 토글 입력 이벤트 핸들러
    /// </summary>
    public void OnInventory(InputAction.CallbackContext context)
    {
        Debug.Log("인벤토리 열림 또는 닫힘");
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    /// <summary>
    /// 커서의 잠금 상태를 토글
    /// </summary>
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLock = !toggle;
    }

    /// <summary>
    /// 점프 부스트 아이템 효과 적용 (지속 시간 동안 점프력 증가)
    /// </summary>
    /// <param name="duration">점프력 증가 효과 지속 시간</param>
    public void ApplyJumpBoost(float duration)
    {
        if (jumpBoostCoroutine != null)
        {
            StopCoroutine(jumpBoostCoroutine); // 기존 효과 중지
        }

        jumpBoostCoroutine = StartCoroutine(JumpBoostCoroutine(duration));
    }

    /// <summary>
    /// 일정 시간 동안 점프력을 증가시키는 코루틴
    /// </summary>
    /// <param name="duration">효과 지속 시간</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator JumpBoostCoroutine(float duration)
    {
        jumpPower = baseJumpPower * 1.2f; // 점프력 2배 증가
        Debug.Log("Jump Boost ON");

        // UI 갱신 요청
        if (condition != null)
            condition.StartJumpBoostUI(duration);

        yield return new WaitForSeconds(duration); // 지정된 시간만큼 대기
        
        jumpPower = baseJumpPower; // 원래 점프력으로 복구
        Debug.Log("Jump Boost OFF");
    }
}
