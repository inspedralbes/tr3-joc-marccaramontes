using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;
using System.Linq;
using UnityEngine.EventSystems;

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

        // 1. ASEGURAR EVENTSYSTEM (CRÍTICO PARA CLICS)
        EventSystem es = Object.FindFirstObjectByType<EventSystem>();
        if (es == null)
        {
            GameObject esGo = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Undo.RegisterCreatedObjectUndo(esGo, "Create EventSystem");
            Debug.Log("<b>[SceneSetup]</b> EventSystem creado.");
        }

        // 2. BUSCAR O CREAR CANVAS
        GameObject canvasObj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "CanvasResultados" && go.scene == scene);
        if (canvasObj == null)
        {
            canvasObj = new GameObject("CanvasResultados", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObj.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            Undo.RegisterCreatedObjectUndo(canvasObj, "Create Canvas");
        }
        canvasObj.SetActive(true);

        // 3. BUSCAR O CREAR PANEL DE FONDO (CON LAYOUT)
        GameObject panelObj = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name == "PanelResultados" && go.scene == scene);
        if (panelObj == null)
        {
            panelObj = new GameObject("PanelResultados", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
            panelObj.transform.SetParent(canvasObj.transform, false);
            Undo.RegisterCreatedObjectUndo(panelObj, "Create Panel");
        }
        
        panelObj.SetActive(false); // Empieza oculto

        // Configurar Layout del Panel
        var rect = panelObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(450, 550);
        
        var img = panelObj.GetComponent<Image>();
        img.color = new Color(0.12f, 0.15f, 0.18f, 0.95f); // Gris azulado oscuro pro
        
        var vlg = panelObj.GetComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(40, 40, 40, 40);
        vlg.spacing = 25;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlHeight = false;
        vlg.childForceExpandHeight = false;

        var csf = panelObj.GetComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // 4. VINCULACIÓN AL CANVAS
        ResultsUIRegisterer registerer = canvasObj.GetComponent<ResultsUIRegisterer>();
        if (registerer == null) registerer = canvasObj.AddComponent<ResultsUIRegisterer>();
        registerer.resultsPanel = panelObj;

        // 5. CREAR ELEMENTOS (ORDENADOS POR EL LAYOUT)
        CreateTextIfMissing("Title", panelObj.transform, "PARTIDA FINALIZADA", 38, Color.white, true);
        
        // Separador visual pequeño
        CreateTextIfMissing("Spacer", panelObj.transform, "________________", 10, new Color(1,1,1,0.3f), false);

        registerer.p1TimeText = CreateTextIfMissing("P1TimeText", panelObj.transform, "Resultado: 0.00s", 26, Color.yellow, false);
        registerer.bestTimeText = CreateTextIfMissing("BestTimeText", panelObj.transform, "Mejor: 0.00s", 20, new Color(1, 1, 1, 0.7f), false);
        registerer.killsText = CreateTextIfMissing("KillsText", panelObj.transform, "Bajas: 0", 24, Color.white, true);
        
        GameObject badge = CreateTextIfMissing("NewRecordBadge", panelObj.transform, "¡NUEVO RÉCORD!", 22, new Color(1, 0.8f, 0), true).gameObject;
        registerer.newRecordBadge = badge;
        badge.SetActive(false);

        registerer.p2TimeText = CreateTextIfMissing("P2TimeText", panelObj.transform, "", 22, Color.white, false);
        registerer.winnerText = CreateTextIfMissing("WinnerText", panelObj.transform, "", 28, Color.green, true);

        // 6. CREAR BOTONES (CON ESTILO)
        GameObject btnRetry = CreateStyledButton("RetryButton", panelObj.transform, "REINTENTAR", new Color(0.15f, 0.65f, 0.15f));
        registerer.retryButton = btnRetry.GetComponent<Button>();

        GameObject btnMenu = CreateStyledButton("MenuButton", panelObj.transform, "MENÚ PRINCIPAL", new Color(0.75f, 0.2f, 0.2f));
        registerer.menuButton = btnMenu.GetComponent<Button>();

        // 7. HUD DE TIEMPO (FUERA DEL PANEL)
        GameObject timerHUD = CreateTextIfMissing("TimerHUD", canvasObj.transform, "Tiempo: 00.00s", 22, Color.white, true).gameObject;
        var timerRect = timerHUD.GetComponent<RectTransform>();
        timerRect.anchorMin = timerRect.anchorMax = timerRect.pivot = new Vector2(1, 1);
        timerRect.anchoredPosition = new Vector2(-30, -30);
        registerer.timerHUDText = timerHUD.GetComponent<TextMeshProUGUI>();

        EditorUtility.SetDirty(registerer);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("<color=cyan><b>[SceneSetup]</b> ¡Nueva UI Profesional creada! EventSystem habilitado.</color>");
    }

    private static TextMeshProUGUI CreateTextIfMissing(string name, Transform parent, string content, float size, Color col, bool isBold)
    {
        GameObject go = parent.Find(name)?.gameObject;
        if (go == null)
        {
            go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            go.transform.SetParent(parent, false);
        }
        
        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = size;
        tmp.color = col;
        tmp.alignment = TextAlignmentOptions.Center;
        if (isBold) tmp.fontStyle = FontStyles.Bold;
        
        // Sombra para mejor legibilidad
        var shadow = go.GetComponent<UnityEngine.UI.Shadow>() ?? go.AddComponent<UnityEngine.UI.Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.6f);
        shadow.effectDistance = new Vector2(2, -2);
        
        return tmp;
    }

    private static GameObject CreateStyledButton(string name, Transform parent, string label, Color baseColor)
    {
        GameObject btnGo = parent.Find(name)?.gameObject;
        if (btnGo == null)
        {
            btnGo = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            btnGo.transform.SetParent(parent, false);
            
            GameObject textGo = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            textGo.transform.SetParent(btnGo.transform, false);
            var textRect = textGo.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero; textRect.anchorMax = Vector2.one; textRect.sizeDelta = Vector2.zero;
        }

        btnGo.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 55);
        
        var img = btnGo.GetComponent<Image>();
        img.color = baseColor;
        
        var btn = btnGo.GetComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.highlightedColor = baseColor * 1.2f;
        cb.pressedColor = baseColor * 0.8f;
        btn.colors = cb;

        var tmp = btnGo.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 22;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        return btnGo;
    }
}
