using UnityEngine;
using UnityEngine.SceneManagement;

// 게임 시작 전 인트로 씬 UI
public class IntroSceneController : MonoBehaviour {
    private GUIStyle titleStyle;
    private GUIStyle bodyStyle;
    private GUIStyle buttonStyle;

    private void Start() {
        Time.timeScale = 1f;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    private void OnGUI() {
        EnsureStyles();

        float panelWidth = Mathf.Min(560f, Screen.width - 40f);
        float panelHeight = 330f;
        Rect panelRect = new Rect(
            (Screen.width - panelWidth) * 0.5f,
            (Screen.height - panelHeight) * 0.5f,
            panelWidth,
            panelHeight);

        GUI.Box(panelRect, string.Empty);

        GUI.Label(
            new Rect(panelRect.x + 20f, panelRect.y + 30f, panelWidth - 40f, 70f),
            "ZOMBIE SURVIVAL",
            titleStyle);

        GUI.Label(
            new Rect(panelRect.x + 30f, panelRect.y + 115f, panelWidth - 60f, 90f),
            "Move with W/S, aim with the mouse, survive the waves, and collect bonus weapons.",
            bodyStyle);

        if (GUI.Button(
            new Rect(panelRect.x + panelWidth * 0.5f - 100f, panelRect.y + 230f, 200f, 52f),
            "START",
            buttonStyle))
        {
            StartGame();
        }
    }

    private void StartGame() {
        SceneManager.LoadScene("Main");
    }

    private void EnsureStyles() {
        if (titleStyle != null)
        {
            return;
        }

        titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 38;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.white;

        bodyStyle = new GUIStyle(GUI.skin.label);
        bodyStyle.alignment = TextAnchor.MiddleCenter;
        bodyStyle.fontSize = 18;
        bodyStyle.wordWrap = true;
        bodyStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 22;
        buttonStyle.fontStyle = FontStyle.Bold;
    }
}
