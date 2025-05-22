using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어 주변 상호작용 가능한 오브젝트를 탐지하고 처리하는 클래스
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;          // 상호작용 체크 주기 (초)
    private float lastCheckTime;             // 마지막 체크 시간
    public float maxCheckDistance;           // 탐지 가능한 최대 거리
    public LayerMask layerMask;              // 상호작용 레이어 마스크

    public GameObject curIntercatGameObject; // 현재 바라보고 있는 오브젝트
    private IInteractable curInteractable;   // 현재 상호작용 가능한 컴포넌트

    public TextMeshProUGUI promptText;       // 화면 중앙에 띄울 상호작용 텍스트
    private Camera camera;                   // 메인 카메라 참조

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // 화면 중앙에서 앞으로 Ray 발사
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // 새롭게 감지된 오브젝트일 경우
                if (hit.collider.gameObject != curIntercatGameObject)
                {
                    Debug.Log("물체 감지");
                    curIntercatGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();

                    // 프롬프트 텍스트 표시
                    SetpromptText();
                }
            }
            else
            {
                // 감지된 오브젝트가 없으면 초기화
                curIntercatGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 상호작용 프롬프트 UI 텍스트 설정 및 표시
    /// </summary>
    private void SetpromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    /// <summary>
    /// 플레이어가 상호작용 키를 눌렀을 때 실행
    /// </summary>
    /// <param name="context">입력 이벤트 컨텍스트</param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // 상호작용 실행
            curInteractable.OnInteract();

            // 상태 초기화
            curIntercatGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
