using UnityEngine;

// 게임 내 캐릭터 정보를 전역으로 관리하는 싱글턴 클래스
public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;

    /// <summary>
    /// CharacterManager 싱글턴 인스턴스
    /// 없으면 자동으로 GameObject를 생성하여 추가
    /// </summary>
    public static CharacterManager Instance
    {
        get
        {
            // 인스턴스가 없으면 새로 생성
            if (_instance == null)
            {
                Debug.Log("테스트");
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;

    /// <summary>
    /// 현재 게임의 플레이어 참조
    /// </summary>
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    /// <summary>
    /// 씬 시작 시 싱글턴 인스턴스를 설정하고 유지
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // 씬이 바뀌어도 유지
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 있다면 중복 방지
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
