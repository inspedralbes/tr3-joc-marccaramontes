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
        // 0. ASEGURAR TAGS (Evita el error de "Tag Bullet not defined")
        CreateTag("Bullet");
        CreateTag("Enemy");
        CreateTag("Player");

        string scenePath = "Assets/Scenes/SampleScene.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        if (!scene.IsValid())
        {
            Debug.LogError("<b>[SceneSetup]</b> No se pudo abrir la escena: " + scenePath);
            return;
        }

        // 1. LIMPIEZA DE EVENT SYSTEM (Para Unity 6)
        EventSystem es = Object.FindFirstObjectByType<EventSystem>();
        if (es == null)
        {
            GameObject esGo = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            Undo.RegisterCreatedObjectUndo(esGo, "Create EventSystem");
        }
        else
        {
            // ELIMINAR EL MÓDULO VIEJO SI EXISTE
            var oldModule = es.GetComponent<StandaloneInputModule>();
            if (oldModule != null) DestroyImmediate(oldModule);
            
            // AÑADIR EL NUEVO SI FALTA
            if (es.GetComponent<InputSystemUIInputModule>() == null)
                es.gameObject.AddComponent<InputSystemUIInputModule>();
            
            Debug.Log("<b>[SceneSetup]</b> EventSystem modernizado para Unity 6.");
        }

        // 1.5 ASEGURAR MANAGERS
        GameObject managerGo = GameObject.Find("_Managers");
        if (managerGo == null)
        {
            managerGo = new GameObject("_Managers");
            Undo.RegisterCreatedObjectUndo(managerGo, "Create Managers");
        }
        if (managerGo.GetComponent<UIAnimationManager>() == null)
            managerGo.AddComponent<UIAnimationManager>();

        // 2. CANVAS
        GameObject canvasObj = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(go => go.name == "CanvasResultados" && go.scene == scene);

        if (canvasObj == null)
        {
            canvasObj = new GameObject("CanvasResultados", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObj.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObj.GetComponent<Canvas>().sortingOrder = 100;
        }
        canvasObj.SetActive(true);

        // 3. FLASH DE MUERTE
        GameObject flashObj = canvasObj.transform.Find("DeathFlash")?.gameObject;
        if (flashObj == null)
        {
            flashObj = new GameObject("DeathFlash", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(CanvasGroup));
            flashObj.transform.SetParent(canvasObj.transform, false);
            var fRect = flashObj.GetComponent<RectTransform>();
            fRect.anchorMin = Vector2.zero; fRect.anchorMax = Vector2.one; fRect.sizeDelta = Vector2.zero;
            flashObj.GetComponent<Image>().color = Color.white;
            flashObj.GetComponent<CanvasGroup>().alpha = 0;
        }
        flashObj.SetActive(false);

        // 4. PANEL DE RESULTADOS
        GameObject panelObj = canvasObj.transform.Find("PanelResultados")?.gameObject;
        if (panelObj == null)
        {
            panelObj = new GameObject("PanelResultados", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter), typeof(CanvasGroup));
            panelObj.transform.SetParent(canvasObj.transform, false);
        }
        
        panelObj.SetActive(false); 
        var rect = panelObj.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(500, 600);
        panelObj.GetComponent<Image>().color = new Color(0, 0, 0, 0.95f);

        // 5. VINCULACIÓN AL REGISTERER
        ResultsUIRegisterer registerer = canvasObj.GetComponent<ResultsUIRegisterer>() ?? canvasObj.AddComponent<ResultsUIRegisterer>();
        registerer.resultsPanel = panelObj;
        registerer.deathFlashOverlay = flashObj;

        // Crear/Actualizar elementos hijos
        registerer.p1TimeText = CreateTextIfMissing("P1TimeText", panelObj.transform, "0.00s", 60, Color.white, true);
        registerer.killsText = CreateTextIfMissing("KillsText", panelObj.transform, "Bajas: 0", 28, Color.white, false);
        registerer.bestTimeText = CreateTextIfMissing("BestTimeText", panelObj.transform, "Mejor: 0.00s", 22, new Color(1, 1, 1, 0.6f), false);
        
        GameObject badge = CreateTextIfMissing("NewRecordBadge", panelObj.transform, "¡NUEVO RÉCORD!", 24, Color.yellow, true).gameObject;
        registerer.newRecordBadge = badge;
        badge.SetActive(false);

        registerer.retryButton = CreateStyledButton("RetryButton", panelObj.transform, "REINTENTAR", new Color(0.2f, 0.2f, 0.2f)).GetComponent<Button>();
        registerer.menuButton = CreateStyledButton("MenuButton", panelObj.transform, "MENÚ", new Color(0.2f, 0.2f, 0.2f)).GetComponent<Button>();

        // 5.5 HUD EN TIEMPO REAL
        SetupHUD(canvasObj, registerer);

        // 6. ATMÓSFERA Y LUCES
        SetupAtmosphere(scene);
        SetupPlayerLight(scene);

        EditorUtility.SetDirty(registerer);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("<color=cyan><b>[SceneSetup]</b> Escena reparada para Unity 6. Managers, Tags, HUD y Atmósfera configurados.</color>");
    }

    private static void SetupHUD(GameObject canvasObj, ResultsUIRegisterer registerer)
    {
        GameObject hudGroupGo = canvasObj.transform.Find("HUDGroup")?.gameObject;
        if (hudGroupGo == null)
        {
            hudGroupGo = new GameObject("HUDGroup", typeof(RectTransform), typeof(CanvasGroup));
            hudGroupGo.transform.SetParent(canvasObj.transform, false);
        }
        
        RectTransform hudRect = hudGroupGo.GetComponent<RectTransform>();
        hudRect.anchorMin = Vector2.zero;
        hudRect.anchorMax = Vector2.one;
        hudRect.sizeDelta = Vector2.zero;
        
        registerer.hudGroup = hudGroupGo.GetComponent<CanvasGroup>();

        // 1. Kills (Top-Left)
        registerer.killsHUDText = CreateHUDText("KillsHUD", hudGroupGo.transform, "BAJAS: 0", 32, Color.white, new Vector2(0, 1), new Vector2(0.05f, 0.95f));
        
        // 2. Timer (Top-Center)
        registerer.timerHUDText = CreateHUDText("TimerHUD", hudGroupGo.transform, "0.00s", 48, Color.white, new Vector2(0.5f, 1), new Vector2(0.5f, 0.95f));
        registerer.timerHUDText.fontStyle = FontStyles.Bold;

        // 3. Best Time (Top-Right)
        registerer.bestTimeHUDText = CreateHUDText("BestTimeHUD", hudGroupGo.transform, "RECORD: 0.00s", 24, new Color(1, 1, 1, 0.6f), new Vector2(1, 1), new Vector2(0.95f, 0.95f));
        registerer.bestTimeHUDText.alignment = TextAlignmentOptions.Right;

        Debug.Log("<b>[SceneSetup]</b> HUD Group configurado y vinculado.");
    }

    private static TextMeshProUGUI CreateHUDText(string name, Transform parent, string content, float size, Color col, Vector2 anchor, Vector2 pivotPos)
    {
        Transform tr = parent.Find(name);
        GameObject go = (tr != null) ? tr.gameObject : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = Vector2.zero; // Se ajustará con márgenes si es necesario, pero por ahora centrado en el anchor
        
        // Ajuste manual de posición según el anchor para dar margen
        float margin = 40f;
        if (anchor.x == 0) rect.anchoredPosition = new Vector2(margin, -margin);
        else if (anchor.x == 1) rect.anchoredPosition = new Vector2(-margin, -margin);
        else rect.anchoredPosition = new Vector2(0, -margin);

        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = content; tmp.fontSize = size; tmp.color = col;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.raycastTarget = false;
        
        return tmp;
    }

    private static void SetupAtmosphere(UnityEngine.SceneManagement.Scene scene)
    {
        // Cámara
        Camera cam = Object.FindFirstObjectByType<Camera>();
        if (cam != null)
        {
            cam.backgroundColor = Color.black;
            cam.clearFlags = CameraClearFlags.SolidColor;
            Debug.Log("<b>[SceneSetup]</b> Cámara configurada (Fondo Negro).");
        }

        // Luz Global 2D
        var lights = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(m => m.GetType().Name == "Light2D").ToList();
        
        foreach (var light in lights)
        {
            var typeProp = light.GetType().GetProperty("lightType");
            if (typeProp != null && (int)typeProp.GetValue(light) == 4) // 4 = Global en URP 2D
            {
                var intensityProp = light.GetType().GetProperty("intensity");
                if (intensityProp != null) intensityProp.SetValue(light, 0.15f);
                Debug.Log("<b>[SceneSetup]</b> Luz Global ajustada a 0.15.");
            }
        }

        // Plataforma
        GameObject platform = GameObject.FindGameObjectWithTag("Platform");
        if (platform != null)
        {
            SpriteRenderer sr = platform.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = new Color(0.1f, 0.1f, 0.1f, 1f); // #1A1A1A aprox
            Debug.Log("<b>[SceneSetup]</b> Plataforma oscurecida.");
        }
    }

    private static void SetupPlayerLight(UnityEngine.SceneManagement.Scene scene)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Intentar encontrar o añadir Light2D
        System.Type light2DType = System.Type.GetType("UnityEngine.Rendering.Universal.Light2D, Unity.RenderPipelines.Universal.Runtime");
        if (light2DType == null) return;

        Component lightComp = player.GetComponent(light2DType);
        if (lightComp == null)
        {
            lightComp = player.AddComponent(light2DType);
        }

        // Configurar vía Reflexión (para evitar errores si el ensamblado no está bien referenciado en el script del editor)
        var typeProp = light2DType.GetProperty("lightType");
        if (typeProp != null) typeProp.SetValue(lightComp, 0); // 0 = Point

        var radiusProp = light2DType.GetProperty("pointLightOuterRadius");
        if (radiusProp != null) radiusProp.SetValue(lightComp, 8f);

        var colorProp = light2DType.GetProperty("color");
        if (colorProp != null) colorProp.SetValue(lightComp, new Color(1f, 0.8f, 0.53f)); // #FFCC88 aprox

        var intensityProp = light2DType.GetProperty("intensity");
        if (intensityProp != null) intensityProp.SetValue(lightComp, 1.2f);

        Debug.Log("<b>[SceneSetup]</b> Luz de antorcha añadida al Jugador.");
    }

    private static void CreateTag(string tagName)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            if (tagsProp.GetArrayElementAtIndex(i).stringValue.Equals(tagName)) { found = true; break; }
        }

        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            tagsProp.GetArrayElementAtIndex(0).stringValue = tagName;
            tagManager.ApplyModifiedProperties();
            Debug.Log($"<b>[SceneSetup]</b> Tag '{tagName}' creado.");
        }
    }

    private static TextMeshProUGUI CreateTextIfMissing(string name, Transform parent, string content, float size, Color col, bool isBold)
    {
        Transform tr = parent.Find(name);
        GameObject go = (tr != null) ? tr.gameObject : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        
        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = content; tmp.fontSize = size; tmp.color = col;
        tmp.alignment = TextAlignmentOptions.Center;
        if (isBold) tmp.fontStyle = FontStyles.Bold;
        return tmp;
    }

    private static GameObject CreateStyledButton(string name, Transform parent, string label, Color baseColor)
    {
        Transform tr = parent.Find(name);
        GameObject btnGo = (tr != null) ? tr.gameObject : new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btnGo.transform.SetParent(parent, false);
        btnGo.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 55);
        btnGo.GetComponent<Image>().color = baseColor;
        CreateTextIfMissing("Text", btnGo.transform, label, 22, Color.white, true);
        return btnGo;
    }
}
