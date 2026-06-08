using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 종료 후 점수와 랭킹을 보여주는 엔딩 씬 UI
public class EndingSceneController : MonoBehaviour {
    private GUIStyle titleStyle;
    private GUIStyle bodyStyle;
    private GUIStyle rankStyle;
    private GUIStyle buttonStyle;

    private void Start() {
        Time.timeScale = 1f;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void OnGUI() {
        EnsureStyles();

        float panelWidth = Mathf.Min(620f, Screen.width - 40f);
        float panelHeight = 430f;
        Rect panelRect = new Rect(
            (Screen.width - panelWidth) * 0.5f,
            (Screen.height - panelHeight) * 0.5f,
            panelWidth,
            panelHeight);

        GUI.Box(panelRect, string.Empty);

        GUI.Label(
            new Rect(panelRect.x + 20f, panelRect.y + 25f, panelWidth - 40f, 60f),
            "GAME OVER",
            titleStyle);

        GUI.Label(
            new Rect(panelRect.x + 30f, panelRect.y + 95f, panelWidth - 60f, 40f),
            "Final Score : " + GameSession.lastScore,
            bodyStyle);

        GUI.Label(
            new Rect(panelRect.x + 30f, panelRect.y + 150f, panelWidth - 60f, 150f),
            "RANKING\n" + RankingSystem.FormatRanking(),
            rankStyle);

        if (GUI.Button(
            new Rect(panelRect.x + panelWidth * 0.5f - 220f, panelRect.y + 330f, 200f, 52f),
            "RETRY",
            buttonStyle))
        {
            RestartGame();
        }

        if (GUI.Button(
            new Rect(panelRect.x + panelWidth * 0.5f + 20f, panelRect.y + 330f, 200f, 52f),
            "INTRO",
            buttonStyle))
        {
            SceneManager.LoadScene("Intro");
        }
    }

    private void RestartGame() {
        SceneManager.LoadScene("Main");
    }

    private void EnsureStyles() {
        if (titleStyle != null)
        {
            return;
        }

        titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 40;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.white;

        bodyStyle = new GUIStyle(GUI.skin.label);
        bodyStyle.alignment = TextAnchor.MiddleCenter;
        bodyStyle.fontSize = 22;
        bodyStyle.normal.textColor = new Color(0.95f, 0.95f, 0.95f);

        rankStyle = new GUIStyle(GUI.skin.label);
        rankStyle.alignment = TextAnchor.UpperCenter;
        rankStyle.fontSize = 20;
        rankStyle.fontStyle = FontStyle.Bold;
        rankStyle.normal.textColor = new Color(1f, 0.86f, 0.35f);

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 20;
        buttonStyle.fontStyle = FontStyle.Bold;
    }
}
