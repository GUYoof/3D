using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// �÷��̾� �ֺ� ��ȣ�ۿ� ������ ������Ʈ�� Ž���ϰ� ó���ϴ� Ŭ����
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;          // ��ȣ�ۿ� üũ �ֱ� (��)
    private float lastCheckTime;             // ������ üũ �ð�
    public float maxCheckDistance;           // Ž�� ������ �ִ� �Ÿ�
    public LayerMask layerMask;              // ��ȣ�ۿ� ���̾� ����ũ

    public GameObject curIntercatGameObject; // ���� �ٶ󺸰� �ִ� ������Ʈ
    private IInteractable curInteractable;   // ���� ��ȣ�ۿ� ������ ������Ʈ

    public TextMeshProUGUI promptText;       // ȭ�� �߾ӿ� ��� ��ȣ�ۿ� �ؽ�Ʈ
    private Camera camera;                   // ���� ī�޶� ����

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            // ȭ�� �߾ӿ��� ������ Ray �߻�
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                // ���Ӱ� ������ ������Ʈ�� ���
                if (hit.collider.gameObject != curIntercatGameObject)
                {
                    Debug.Log("��ü ����");
                    curIntercatGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();

                    // ������Ʈ �ؽ�Ʈ ǥ��
                    SetpromptText();
                }
            }
            else
            {
                // ������ ������Ʈ�� ������ �ʱ�ȭ
                curIntercatGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ��ȣ�ۿ� ������Ʈ UI �ؽ�Ʈ ���� �� ǥ��
    /// </summary>
    private void SetpromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    /// <summary>
    /// �÷��̾ ��ȣ�ۿ� Ű�� ������ �� ����
    /// </summary>
    /// <param name="context">�Է� �̺�Ʈ ���ؽ�Ʈ</param>
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // ��ȣ�ۿ� ����
            curInteractable.OnInteract();

            // ���� �ʱ�ȭ
            curIntercatGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
