using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// 점수와 게임 오버 여부를 관리하는 게임 매니저
public class GameManager : MonoBehaviour {
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public event Action<int> onGameover; // 게임 오버시 최종 점수를 전달하는 이벤트

    private int score = 0; // 현재 게임 점수
    public int Score { get { return score; } } // 현재 게임 점수 조회용 프로퍼티
    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake() {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
            return;
        }

        m_instance = this;
    }

    private void Start() {
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.onDeath += EndGame;
        }
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore) {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateScoreText(score);
            }
        }
    }

    // 게임 오버 처리
    public void EndGame() {
        if (isGameover)
        {
            return;
        }

        // 게임 오버 상태를 참으로 변경
        isGameover = true;

        GameSession.FinishRun(score);

        if (onGameover != null)
        {
            onGameover(score);
        }

        // 엔딩 씬이 빌드 설정에 있으면 엔딩 씬으로 전환한다.
        if (Application.CanStreamedLevelBeLoaded("Ending"))
        {
            SceneManager.LoadScene("Ending");
            return;
        }

        // 엔딩 씬을 아직 생성하지 않은 상태에서는 기존 게임 오버 UI로 대체한다.
        if (UIManager.instance != null)
        {
            UIManager.instance.SetActiveGameoverUI(true);
        }
    }
}
