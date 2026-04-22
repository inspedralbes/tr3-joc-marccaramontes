using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class SceneSetupHelper : Editor
{
    [MenuItem("Tools/Setup HUD and Results UI")]
    public static void SetupScene()
    {
        CreateTag("Bullet");
        CreateTag("Enemy");
        CreateTag("Player");

        string scenePath = "Assets/Scenes/SampleScene.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        if (!scene.IsValid()) return;

        // ASEGURAR MANAGERS
        GameObject managerGo = GameObject.Find("_Managers") ?? new GameObject("_Managers");
        if (managerGo.GetComponent<UIAnimationManager>() == null) managerGo.AddComponent<UIAnimationManager>();

        // CANVAS
        GameObject canvasObj = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(go => go.name == "CanvasResultados" && go.scene == scene);
        if (canvasObj == null) return;

        // VINCULACIÓN AL REGISTERER
        ResultsUIRegisterer registerer = canvasObj.GetComponent<ResultsUIRegisterer>() ?? canvasObj.AddComponent<ResultsUIRegisterer>();
        GameObject panelObj = canvasObj.transform.Find("PanelResultados")?.gameObject;
        if (panelObj == null) return;

        registerer.resultsPanel = panelObj;
        registerer.deathFlashOverlay = canvasObj.transform.Find("DeathFlash")?.gameObject;

        // Configurar referencias según tu nueva jerarquía manual
        registerer.titleText = panelObj.transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
        registerer.winnerText = panelObj.transform.Find("WinnerText")?.GetComponent<TextMeshProUGUI>();
        registerer.p1TimeText = panelObj.transform.Find("P1TimeText")?.GetComponent<TextMeshProUGUI>();
        registerer.killsText = panelObj.transform.Find("KillsText")?.GetComponent<TextMeshProUGUI>();
        
        registerer.retryButton = panelObj.transform.Find("RetryButton")?.GetComponent<Button>();
        registerer.menuButton = panelObj.transform.Find("MenuButton")?.GetComponent<Button>();

        // HUD
        SetupHUD(canvasObj, registerer);

        EditorUtility.SetDirty(registerer);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("<color=green><b>[SceneSetup]</b> Scripts sincronizados con tu jerarquía manual.</color>");
    }

    private static void SetupHUD(GameObject canvasObj, ResultsUIRegisterer registerer)
    {
        GameObject hudGroupGo = canvasObj.transform.Find("HUDGroup")?.gameObject;
        if (hudGroupGo == null) return;
        
        registerer.hudGroup = hudGroupGo.GetComponent<CanvasGroup>();
        Transform kHUD = hudGroupGo.transform.Find("KillsHUD");
        if (kHUD != null) registerer.killsHUDText = kHUD.GetComponent<TextMeshProUGUI>();
        
        Transform tHUD = hudGroupGo.transform.Find("TimerHUD");
        if (tHUD != null) registerer.timerHUDText = tHUD.GetComponent<TextMeshProUGUI>();
    }

    private static void CreateTag(string tagName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");
        for (int i = 0; i < tagsProp.arraySize; i++)
            if (tagsProp.GetArrayElementAtIndex(i).stringValue.Equals(tagName)) return;
        tagsProp.InsertArrayElementAtIndex(0);
        tagsProp.GetArrayElementAtIndex(0).stringValue = tagName;
        tagManager.ApplyModifiedProperties();
    }
}
