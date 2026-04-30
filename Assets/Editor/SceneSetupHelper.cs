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
        
        // RemotePlayerManager
        if (managerGo.GetComponent<RemotePlayerManager>() == null) 
        {
            var rpm = managerGo.AddComponent<RemotePlayerManager>();
            // Intentar asignar el prefab del jugador automáticamente
            GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Player.prefab") ?? 
                                     AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Enemy.prefab"); // Fallback si no hay player prefab obvio
            if (playerPrefab != null) rpm.playerPrefab = playerPrefab;
        }

        // LIMPIEZA DE DUPLICADOS (Sanación de escena)
        var allRegisterers = Resources.FindObjectsOfTypeAll<ResultsUIRegisterer>()
            .Where(r => r.gameObject.scene == scene).ToList();
        
        foreach (var reg in allRegisterers)
        {
            if (reg.gameObject.name != "CanvasResultados")
            {
                Debug.Log($"<b>[SceneSetup]</b> Eliminando ResultsUIRegisterer duplicado en: {reg.gameObject.name}");
                DestroyImmediate(reg, true);
            }
        }

        // VINCULACIÓN AL REGISTERER
        GameObject canvasObj = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(go => go.name == "CanvasResultados" && go.scene == scene);
        
        if (canvasObj == null) 
        {
            Debug.LogError("<b>[SceneSetup]</b> No se encontró 'CanvasResultados'.");
            return;
        }

        ResultsUIRegisterer registerer = canvasObj.GetComponent<ResultsUIRegisterer>() ?? canvasObj.AddComponent<ResultsUIRegisterer>();
        GameObject panelObj = canvasObj.transform.Find("PanelResultados")?.gameObject;
        if (panelObj == null) return;

        registerer.resultsPanel = panelObj;
        registerer.deathFlashOverlay = canvasObj.transform.Find("DeathFlash")?.gameObject;

        // Configurar referencias según tu nueva jerarquía manual
        registerer.titleText = panelObj.transform.Find("TitleText")?.GetComponent<TextMeshProUGUI>();
        registerer.winnerText = panelObj.transform.Find("WinnerText")?.GetComponent<TextMeshProUGUI>();
        registerer.p1TimeText = panelObj.transform.Find("P1TimeText")?.GetComponent<TextMeshProUGUI>();
        registerer.p2TimeText = panelObj.transform.Find("P2TimeText")?.GetComponent<TextMeshProUGUI>(); // Nuevo
        registerer.killsText = panelObj.transform.Find("KillsText")?.GetComponent<TextMeshProUGUI>();
        
        registerer.retryButton = panelObj.transform.Find("RetryButton")?.GetComponent<Button>();
        registerer.menuButton = panelObj.transform.Find("MenuButton")?.GetComponent<Button>();

        // HUD
        SetupHUD(canvasObj, registerer);

        // ATMÓSFERA Y LUCES
        SetupAtmosphere(scene);
        SetupLighting(scene);

        EditorUtility.SetDirty(registerer);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("<color=green><b>[SceneSetup]</b> Scripts sincronizados con tu jerarquía manual y atmósfera configurada.</color>");
    }

    private static void SetupAtmosphere(UnityEngine.SceneManagement.Scene scene)
    {
        // 1. CONFIGURAR CÁMARA
        GameObject camGo = scene.GetRootGameObjects().FirstOrDefault(go => go.name == "Main Camera");
        if (camGo != null)
        {
            Camera cam = camGo.GetComponent<Camera>();
            if (cam != null)
            {
                cam.backgroundColor = Color.black;
                cam.clearFlags = CameraClearFlags.SolidColor;
            }
            // Activar Post-processing en el componente URP si existe
            var additionalCamData = camGo.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
            if (additionalCamData != null) additionalCamData.renderPostProcessing = true;
        }

        // 2. CONFIGURAR PLATAFORMA
        GameObject platformGo = GameObject.FindGameObjectWithTag("Platform") ?? scene.GetRootGameObjects().FirstOrDefault(go => go.name.Contains("Plat"));
        if (platformGo != null)
        {
            var renderer = platformGo.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Color charcoal = new Color(0.1f, 0.1f, 0.1f, 1f); // #1A1A1A aproximado
                renderer.color = charcoal;
            }
        }
    }

    private static void SetupLighting(UnityEngine.SceneManagement.Scene scene)
    {
        // 1. GLOBAL LIGHT 2D
        var globalLight = Object.FindObjectsByType<UnityEngine.Rendering.Universal.Light2D>(FindObjectsSortMode.None)
            .FirstOrDefault(l => l.lightType == UnityEngine.Rendering.Universal.Light2D.LightType.Global);
        
        if (globalLight == null)
        {
            GameObject glGo = new GameObject("Global Light 2D", typeof(UnityEngine.Rendering.Universal.Light2D));
            globalLight = glGo.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            globalLight.lightType = UnityEngine.Rendering.Universal.Light2D.LightType.Global;
        }
        globalLight.intensity = 0.15f;

        // 2. PLAYER POINT LIGHT (TORCH)
        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        if (playerGo != null)
        {
            var playerLight = playerGo.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
            if (playerLight == null || playerLight.lightType == UnityEngine.Rendering.Universal.Light2D.LightType.Global)
            {
                GameObject lightGo = new GameObject("TorchLight", typeof(UnityEngine.Rendering.Universal.Light2D));
                lightGo.transform.SetParent(playerGo.transform);
                lightGo.transform.localPosition = Vector3.zero;
                playerLight = lightGo.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            }
            
            playerLight.lightType = UnityEngine.Rendering.Universal.Light2D.LightType.Point;
            playerLight.pointLightOuterRadius = 8f;
            playerLight.intensity = 1.0f;
            
            // Color #FFCC88 (Amber)
            Color amber = new Color(1f, 0.8f, 0.53f, 1f); 
            playerLight.color = amber;
        }
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

    [MenuItem("Tools/Setup Lobby UI")]
    public static void SetupLobby()
    {
        string scenePath = "Assets/Scenes/Lobby.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        if (!scene.IsValid()) return;

        // 1. ASEGURAR MANAGERS Y POST-PROCESS
        GameObject managerGo = GameObject.Find("_Managers") ?? new GameObject("_Managers");
        if (managerGo.GetComponent<UIAnimationManager>() == null) managerGo.AddComponent<UIAnimationManager>();

        // 2. CARGAR TEMA
        UIThemeSO theme = AssetDatabase.LoadAssetAtPath<UIThemeSO>("Assets/InfernalTheme.asset");
        if (theme == null) Debug.LogWarning("<b>[LobbySetup]</b> No se encontró 'InfernalTheme.asset'.");

        // 3. CONFIGURAR CÁMARA (Igual que el Menú)
        GameObject camGo = GameObject.Find("Main Camera");
        if (camGo != null)
        {
            Camera cam = camGo.GetComponent<Camera>();
            if (cam != null)
            {
                cam.backgroundColor = Color.black;
                cam.clearFlags = CameraClearFlags.SolidColor;
            }
            // Activar Post-processing en el componente URP si existe
            var additionalCamData = camGo.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
            if (additionalCamData != null) additionalCamData.renderPostProcessing = true;
        }

        // 4. ENCONTRAR LOBBY MANAGER
        LobbyController lobby = Object.FindAnyObjectByType<LobbyController>();
        if (lobby == null)
        {
            Debug.LogError("<b>[LobbySetup]</b> No se encontró 'LobbyController'.");
            return;
        }

        // 5. CONFIGURAR FONDO Y CÍRCULOS
        GameObject canvasGo = GameObject.Find("Canvas");
        if (canvasGo != null)
        {
            // Asegurar que el fondo sea oscuro
            GameObject bgObj = GameObject.Find("LobbyBackground");
            if (bgObj == null)
            {
                bgObj = new GameObject("LobbyBackground", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            }
            
            bgObj.transform.SetParent(canvasGo.transform);
            bgObj.transform.SetAsFirstSibling(); // Detrás de todo

            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgRect.anchoredPosition = Vector2.zero;
            bgRect.localScale = Vector3.one;

            Image bgImg = bgObj.GetComponent<Image>();
            bgImg.color = new Color(0, 0, 0, 0.7f); // Capa oscura para profundidad

            // Círculos (Igual que el Menú, pero uno azul)
            SetupPulsatingCircle(bgObj.transform, "Circle_Left", new Vector2(-350, -100), new Vector2(700, 700), new Color(0f, 0.4f, 1f, 0.15f)); // AZUL
            SetupPulsatingCircle(bgObj.transform, "Circle_Right", new Vector2(350, 100), new Vector2(700, 700), new Color(1f, 0f, 0f, 0.1f));   // ROJO
        }

        // 6. CONFIGURAR PANELES (Semi-transparentes para ver los círculos)
        GameObject mainPanel = GameObject.Find("MainPanel");
        GameObject waitingPanel = GameObject.Find("WaitingPanel");

        if (mainPanel != null)
        {
            lobby.mainPanelGroup = mainPanel.GetComponent<CanvasGroup>() ?? mainPanel.AddComponent<CanvasGroup>();
            Image pImg = mainPanel.GetComponent<Image>();
            if (pImg != null) pImg.color = new Color(0, 0, 0, 0.4f); // Más transparente para ver el fondo
        }
        if (waitingPanel != null)
        {
            lobby.waitingPanelGroup = waitingPanel.GetComponent<CanvasGroup>() ?? waitingPanel.AddComponent<CanvasGroup>();
            Image pImg = waitingPanel.GetComponent<Image>();
            if (pImg == null) pImg = waitingPanel.AddComponent<Image>();
            pImg.color = new Color(0, 0, 0, 0.8f); // Fondo más oscuro para el panel de espera

            // Centrar el panel de espera y ajustar tamaño
            RectTransform waitRect = waitingPanel.GetComponent<RectTransform>();
            waitRect.anchorMin = new Vector2(0.5f, 0.5f);
            waitRect.anchorMax = new Vector2(0.5f, 0.5f);
            waitRect.pivot = new Vector2(0.5f, 0.5f);
            waitRect.anchoredPosition = Vector2.zero;
            waitRect.sizeDelta = new Vector2(500, 350);

            // Asegurar que el objeto esté activo para que el script pueda encontrar sus hijos
            waitingPanel.SetActive(true);
        }

        // 7. VINCULAR BOTONES Y ESTILO INFERNAL
        SetupInfernalButton(GameObject.Find("CreateBtn"), theme);
        SetupInfernalButton(GameObject.Find("JoinBtn"), theme);
        SetupInfernalButton(GameObject.Find("BackBtn"), theme);
        SetupInfernalButton(GameObject.Find("StartMatchBtn"), theme);
        
        if (waitingPanel != null)
        {
            foreach (var btn in waitingPanel.GetComponentsInChildren<Button>(true))
            {
                SetupInfernalButton(btn.gameObject, theme);
            }

            Button backToMain = waitingPanel.GetComponentsInChildren<Button>(true).FirstOrDefault(b => b.name.Contains("Back") || b.name.Contains("Return"));
            if (backToMain != null) lobby.backToMainBtn = backToMain;
        }

        // 8. ESTILIZAR TEXTOS
        var allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>().Where(t => t.gameObject.scene == scene);
        foreach (var text in allTexts)
        {
            if (theme != null) text.color = theme.bloodRed;
            text.alignment = TextAlignmentOptions.Center;
        }

        // 9. VINCULAR REFERENCIAS AL LOBBY MANAGER
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.scene == scene).ToList();

        lobby.statusText = allObjects.FirstOrDefault(go => go.name == "StatusText")?.GetComponent<TextMeshProUGUI>();
        lobby.roomCodeText = allObjects.FirstOrDefault(go => go.name == "RoomCodeText")?.GetComponent<TextMeshProUGUI>();
        lobby.nameInputField = allObjects.FirstOrDefault(go => go.name == "NameInput")?.GetComponent<TMP_InputField>();
        lobby.roomInputField = allObjects.FirstOrDefault(go => go.name == "RoomInput")?.GetComponent<TMP_InputField>();

        // Forzar el estado inicial: Main activo, Waiting oculto (por alpha)
        if (mainPanel != null) { mainPanel.SetActive(true); lobby.mainPanelGroup.alpha = 1; }
        if (waitingPanel != null) { waitingPanel.SetActive(true); lobby.waitingPanelGroup.alpha = 0; }

        // Forzar guardado para persistir las referencias en el Inspector
        EditorUtility.SetDirty(lobby);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("<color=green><b>[LobbySetup]</b> Escena del Lobby configurada EXACTAMENTE como el Menú.</color>");
    }

    private static void SetupPulsatingCircle(Transform parent, string name, Vector2 pos, Vector2 size, Color color)
    {
        Transform existing = parent.Find(name);
        if (existing != null) DestroyImmediate(existing.gameObject);

        GameObject circleGo = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        circleGo.transform.SetParent(parent);
        
        RectTransform rect = circleGo.GetComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;
        rect.localScale = Vector3.one;

        Image img = circleGo.GetComponent<Image>();
        img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        img.color = color;

        InfernalMenuCircle breathe = circleGo.AddComponent<InfernalMenuCircle>();
        breathe.breatheSpeed = 2f;
        breathe.breatheAmount = 0.08f;
    }

    private static void SetupInfernalButton(GameObject go, UIThemeSO theme)
    {
        if (go == null) return;
        
        // Quitar hover antiguo si existe
        ButtonHoverEffect oldHover = go.GetComponent<ButtonHoverEffect>();
        if (oldHover != null) DestroyImmediate(oldHover, true);

        // Añadir InfernalButton
        InfernalButton infernal = go.GetComponent<InfernalButton>() ?? go.AddComponent<InfernalButton>();
        infernal.theme = theme;
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
