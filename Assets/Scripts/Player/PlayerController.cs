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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); // Rigidbody를 가져옴
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 중앙에 고정하고 보이지 않게 함
    }

    /// <summary>
    /// 물리 연산이 필요한 프레임마다 호출됨
    /// </summary>
    void FixedUpdate()
    {
        Move(); // 이동 함수 호출
    }

    /// <summary>
    /// 모든 Update가 끝난 후 호출되며, 카메라 회전에 사용하면 부드러움
    /// </summary>
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
        // 이동 방향 계산 (forward: 앞/뒤, right: 좌/우)
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; // 이동 속도 적용

        // Y축은 중력에 의해 자연스럽게 처리되도록 기존 속도 유지
        dir.y = _rigidbody.velocity.y;

        // Rigidbody에 최종 이동 벡터 적용
        _rigidbody.velocity = dir;
    }

    /// <summary>
    /// 카메라 회전 처리
    /// </summary>
    void CameraLook()
    {
        // 마우스 Y축 이동량을 누적하여 카메라 회전 각도에 반영 (상하 회전)
        camCurXRot += mouseDelta.y * lookSensitivity;
        // 상하 회전 각도 제한 (고개 너무 위/아래로 못 돌리게)
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, MaxXLook);

        // 카메라 컨테이너 오브젝트의 회전을 적용 (카메라만 위아래 움직임)
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        // 좌우 회전은 플레이어 자체를 회전시킴 (마우스 X축)
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    /// <summary>
    /// 이동 입력 이벤트 처리
    /// </summary>
    /// <param name="context">Input System에서 전달하는 입력 이벤트 데이터</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // 이동 키 입력 중일 때 방향값 저장
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // 이동 키에서 손을 뗐을 때 입력 초기화
            curMovementInput = Vector2.zero;
        }
    }

    /// <summary>
    /// 마우스 이동 입력 처리
    /// </summary>
    /// <param name="context">Input System에서 전달되는 마우스 입력 이벤트 데이터</param>
    public void OnLook(InputAction.CallbackContext context)
    {
        // 마우스 이동 입력 값을 저장
        mouseDelta = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 점프 입력 처리
    /// </summary>
    /// <param name="context">Input System에서 전달되는 점프 키 입력 이벤트 데이터</param>
    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프 키가 눌렸고 바닥에 있을 때만 점프
        Debug.Log($"test {context.phase} {InputActionPhase.Started} {IsGruonded()}"); // 상태 확인용 로그
        if (context.phase == InputActionPhase.Started && IsGruonded())
        {
            Debug.Log("점프");
            // Rigidbody에 위 방향으로 힘을 가함
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 바닥 체크
    /// </summary>
    /// <returns></returns>
    bool IsGruonded()
    {
        // 플레이어 주변 네 방향에서 아래로 레이저 쏘기 (살짝 위에서 시작함)
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + ( transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( -transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + ( -transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // 각 레이마다 바닥이 감지되는지 확인
        for (int i = 0; i < rays.Length; i++)
        {
            Debug.DrawRay(rays[i].origin, rays[i].direction * 1f, Color.red, 1f); // Scene에서 보이도록 디버그 레이 그리기

            // 지정된 레이어(groundLayerMask)에 0.1단위 거리까지 충돌이 감지되면 바닥으로 인식
            if (Physics.Raycast(rays[i], 1f, groundLayerMask))
            {
                return true;
            }
        }

        return false; // 4개 레이 모두 바닥을 감지하지 못했을 경우
    }

    /// <summary>
    /// 인벤토리 토글 입력 이벤트 핸들러
    /// 입력 액션이 시작될 때 인벤토리 상태를 토글하고 커서 잠금 상태를 변경
    /// </summary>
    /// <param name="context">입력 액션의 콜백 컨텍스트</param>
    public void OnInventory(InputAction.CallbackContext context)
    {
        // 인벤토리 토글 입력이 시작될 때 호출됨
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();   // 인벤토리 열기/닫기 이벤트 호출
            ToggleCursor();        // 커서 잠금 상태 토글
        }
    }

    /// <summary>
    /// 커서의 잠금 상태를 토글
    /// 잠금 상태가 Locked면 None으로, None이면 Locked로 변경하며, 커서 잠금 가능 여부(canLock) 플래그도 반전
    /// </summary>
    void ToggleCursor()
    {
        // 현재 커서가 잠긴 상태인지 확인
        bool toggle = Cursor.lockState == CursorLockMode.Locked;

        // 잠겨있으면 해제하고, 해제되어 있으면 잠금 상태로 변경
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;

        // 잠금 가능 여부 반전
        canLock = !toggle;
    }

}
