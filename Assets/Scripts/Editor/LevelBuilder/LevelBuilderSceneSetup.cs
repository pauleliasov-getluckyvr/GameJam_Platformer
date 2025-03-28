using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// Handles the creation and setup of the Level Builder scene
/// </summary>
public static class LevelBuilderSceneSetup
{
    private const string SCENE_NAME = "LevelBuilder";
    private const string SCENE_PATH = "Assets/Scenes/Editor/LevelBuilder.unity";

    [MenuItem("Level Builder/Create Level Builder Scene")]
    public static void CreateLevelBuilderScene()
    {
        // Create a new scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Create main camera
        var mainCamera = new GameObject("Main Camera");
        var camera = mainCamera.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5;
        camera.transform.position = new Vector3(0, 0, -10);
        camera.backgroundColor = Color.black;
        mainCamera.tag = "MainCamera";

        // Create grid
        var grid = new GameObject("Grid");
        grid.AddComponent<Grid>();
        
        // Create level container
        var levelContainer = new GameObject("Level");
        levelContainer.transform.position = Vector3.zero;

        // Create UI canvas for editor controls
        var canvas = new GameObject("UI Canvas");
        var canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Save the scene
        EditorSceneManager.SaveScene(scene, SCENE_PATH);
        Debug.Log("Level Builder scene created successfully at: " + SCENE_PATH);
    }

    [MenuItem("Level Builder/Open Level Builder")]
    public static void OpenLevelBuilderScene()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(SCENE_PATH);
        }
    }
}

/// <summary>
/// Helper component for visualizing the grid in the Level Builder
/// </summary>
public class GridHelper : MonoBehaviour
{
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    private void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

        // Draw vertical lines
        for (float x = -10f; x <= 10f; x += gridSize)
        {
            Gizmos.DrawLine(new Vector3(x, -10f, 0f), new Vector3(x, 10f, 0f));
        }

        // Draw horizontal lines
        for (float y = -10f; y <= 10f; y += gridSize)
        {
            Gizmos.DrawLine(new Vector3(-10f, y, 0f), new Vector3(10f, y, 0f));
        }
    }
} 