using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using System.Linq;

public class SceneSetupHelper : Editor
{
    [MenuItem("Tools/Setup HUD and Results UI")]
    public static void SetupScene()
    {
        string scenePath = "Assets/Scenes/SampleScene.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        if (scene == null)
        {
            Debug.LogError("No se pudo abrir la escena SampleScene.");
            return;
        }

        // Buscamos los objetos de forma más robusta (incluyendo desactivados)
        GameObject canvasResultados = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "CanvasResultados" && go.scene == scene);
        GameObject panelResultados = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "PanelResultados" && go.scene == scene);

        if (canvasResultados == null || panelResultados == null)
        {
            Debug.LogError($"No se encontraron los objetos necesarios. Canvas: {canvasResultados != null}, Panel: {panelResultados != null}");
            return;
        }

        // 0. Corrección de Escala y Render Mode
        canvasResultados.transform.localScale = Vector3.one;
        Canvas canvasComp = canvasResultados.GetComponent<Canvas>();
        if (canvasComp != null) canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;

        ResultsUIRegisterer registerer = panelResultados.GetComponent<ResultsUIRegisterer>();
        if (registerer == null)
        {
            registerer = panelResultados.AddComponent<ResultsUIRegisterer>();
        }

        Undo.RecordObject(registerer, "Update ResultsUIRegisterer");
        registerer.resultsPanel = panelResultados;

        // 1. TimerHUD (Esquina Superior Derecha)
        GameObject timerHUDObj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "TimerHUD" && go.scene == scene);
        if (timerHUDObj == null)
        {
            timerHUDObj = new GameObject("TimerHUD", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            timerHUDObj.transform.SetParent(canvasResultados.transform, false);
            Undo.RegisterCreatedObjectUndo(timerHUDObj, "Create TimerHUD");
            
            var rect = timerHUDObj.GetComponent<RectTransform>();
            // Anclaje a la esquina superior derecha
            rect.anchorMin = new Vector2(1f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(-20, -20); // Margen de 20px
            rect.sizeDelta = new Vector2(250, 50);

            var tmp = timerHUDObj.GetComponent<TextMeshProUGUI>();
            tmp.text = "Tiempo: 00.00s";
            tmp.alignment = TextAlignmentOptions.Right;
            tmp.fontSize = 28;
            tmp.color = Color.white;
            
            // Añadir un componente de sombra suave para legibilidad
            timerHUDObj.AddComponent<UnityEngine.UI.Shadow>().effectColor = new Color(0, 0, 0, 0.5f);
        }
        else
        {
            // Si ya existe, solo lo movemos a la esquina por si acaso
            var rect = timerHUDObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 1f);
            rect.anchoredPosition = new Vector2(-20, -20);
            timerHUDObj.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        }
        registerer.timerHUDText = timerHUDObj.GetComponent<TextMeshProUGUI>();

        // 2. RetryButton
        GameObject retryButtonObj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "RetryButton" && go.scene == scene);
        if (retryButtonObj == null)
        {
            retryButtonObj = CreateButton("RetryButton", panelResultados.transform, new Vector2(0, -100), "REINTENTAR");
            Undo.RegisterCreatedObjectUndo(retryButtonObj, "Create RetryButton");
        }
        registerer.retryButton = retryButtonObj.GetComponent<Button>();

        // 3. MenuButton
        GameObject menuButtonObj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "MenuButton" && go.scene == scene);
        if (menuButtonObj == null)
        {
            menuButtonObj = CreateButton("MenuButton", panelResultados.transform, new Vector2(0, -160), "MENÚ PRINCIPAL");
            Undo.RegisterCreatedObjectUndo(menuButtonObj, "Create MenuButton");
        }
        registerer.menuButton = menuButtonObj.GetComponent<Button>();

        // 4. Buscar y asignar textos de resultados si existen
        GameObject p1TextObj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "P1TimeText" && go.scene == scene);
        if (p1TextObj != null) registerer.p1TimeText = p1TextObj.GetComponent<TextMeshProUGUI>();

        GameObject p2TextObj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "P2TimeText" && go.scene == scene);
        if (p2TextObj != null) registerer.p2TimeText = p2TextObj.GetComponent<TextMeshProUGUI>();

        GameObject winnerTextObj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "WinnerText" && go.scene == scene);
        if (winnerTextObj != null) registerer.winnerText = winnerTextObj.GetComponent<TextMeshProUGUI>();

        EditorUtility.SetDirty(registerer);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("<color=green><b>[SceneSetup]</b> Configuración completada. Temporizador movido a la esquina derecha.</color>");
    }

    private static GameObject CreateButton(string name, Transform parent, Vector2 position, string label)
    {
        GameObject buttonObj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        buttonObj.transform.SetParent(parent, false);
        
        var rect = buttonObj.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(200, 45);

        GameObject textObj = new GameObject("Text (TMP)", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(buttonObj.transform, false);
        
        var textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        var tmp = textObj.GetComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = 20;
        tmp.color = Color.black;

        return buttonObj;
    }
}
