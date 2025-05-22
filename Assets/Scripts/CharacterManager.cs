using UnityEngine;

// ���� �� ĳ���� ������ �������� �����ϴ� �̱��� Ŭ����
public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;

    /// <summary>
    /// CharacterManager �̱��� �ν��Ͻ�
    /// ������ �ڵ����� GameObject�� �����Ͽ� �߰�
    /// </summary>
    public static CharacterManager Instance
    {
        get
        {
            // �ν��Ͻ��� ������ ���� ����
            if (_instance == null)
            {
                Debug.Log("�׽�Ʈ");
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;

    /// <summary>
    /// ���� ������ �÷��̾� ����
    /// </summary>
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    /// <summary>
    /// �� ���� �� �̱��� �ν��Ͻ��� �����ϰ� ����
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // ���� �ٲ� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �̹� �ν��Ͻ��� �ִٸ� �ߺ� ����
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
