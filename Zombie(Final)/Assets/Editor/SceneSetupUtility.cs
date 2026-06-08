using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSetupUtility {
    public static void CreateRequiredScenes() {
        EnsureScene("Assets/Scenes/Intro.unity", "Intro Scene Controller", typeof(IntroSceneController));
        EnsureScene("Assets/Scenes/Ending.unity", "Ending Scene Controller", typeof(EndingSceneController));
        UpdateBuildSettings();
    }

    private static void EnsureScene(string path, string controllerName, System.Type controllerType) {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.02f, 0.025f, 0.03f);
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0f, 1.5f, -8f);
        cameraObject.transform.rotation = Quaternion.Euler(8f, 0f, 0f);

        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        GameObject controller = new GameObject(controllerName);
        controller.AddComponent(controllerType);

        EditorSceneManager.SaveScene(scene, path);
    }

    private static void UpdateBuildSettings() {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
        AddSceneIfExists(scenes, "Assets/Scenes/Intro.unity");
        AddSceneIfExists(scenes, "Assets/Scenes/Main.unity");
        AddSceneIfExists(scenes, "Assets/Scenes/Ending.unity");
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    private static void AddSceneIfExists(List<EditorBuildSettingsScene> scenes, string path) {
        if (System.IO.File.Exists(path))
        {
            scenes.Add(new EditorBuildSettingsScene(path, true));
        }
    }
}
