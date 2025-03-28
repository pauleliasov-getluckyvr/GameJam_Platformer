using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Editor window for managing level building settings and operations
/// </summary>
public class LevelBuilderWindow : EditorWindow
{
    private string levelName = "NewLevel";
    private Vector2 scrollPosition;
    private bool showSettings = true;
    private bool showObjects = true;
    private bool showSaveLoad = true;

    // Level settings
    private Vector2 startPoint;
    private Vector2 endPoint;

    // Prefabs
    private GameObject staticPlatformPrefab;
    private GameObject movingPlatformPrefab;
    private GameObject woodenWallPrefab;
    private GameObject destructiblePrefab;
    private GameObject coinPrefab;
    private GameObject powerUpPrefab;

    private List<GameObject> levelObjects = new List<GameObject>();

    [MenuItem("Window/Level Builder")]
    public static void ShowWindow()
    {
        GetWindow<LevelBuilderWindow>("Level Builder");
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        DrawSettingsSection();
        DrawObjectsSection();
        DrawSaveLoadSection();

        EditorGUILayout.EndScrollView();
    }

    private void DrawSettingsSection()
    {
        showSettings = EditorGUILayout.Foldout(showSettings, "Level Settings", true);
        if (showSettings)
        {
            EditorGUI.indentLevel++;

            levelName = EditorGUILayout.TextField("Level Name", levelName);
            startPoint = EditorGUILayout.Vector2Field("Start Point", startPoint);
            endPoint = EditorGUILayout.Vector2Field("End Point", endPoint);

            if (GUILayout.Button("Set Start Point"))
            {
                startPoint = SceneView.lastActiveSceneView.pivot;
            }

            if (GUILayout.Button("Set End Point"))
            {
                endPoint = SceneView.lastActiveSceneView.pivot;
            }

            EditorGUI.indentLevel--;
        }
    }

    private void DrawObjectsSection()
    {
        showObjects = EditorGUILayout.Foldout(showObjects, "Level Objects", true);
        if (showObjects)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField("Platforms", EditorStyles.boldLabel);
            staticPlatformPrefab = (GameObject)EditorGUILayout.ObjectField("Static Platform Prefab", staticPlatformPrefab, typeof(GameObject), false);
            movingPlatformPrefab = (GameObject)EditorGUILayout.ObjectField("Moving Platform Prefab", movingPlatformPrefab, typeof(GameObject), false);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Obstacles", EditorStyles.boldLabel);
            woodenWallPrefab = (GameObject)EditorGUILayout.ObjectField("Wooden Wall Prefab", woodenWallPrefab, typeof(GameObject), false);
            destructiblePrefab = (GameObject)EditorGUILayout.ObjectField("Destructible Prefab", destructiblePrefab, typeof(GameObject), false);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Collectibles", EditorStyles.boldLabel);
            coinPrefab = (GameObject)EditorGUILayout.ObjectField("Coin Prefab", coinPrefab, typeof(GameObject), false);
            powerUpPrefab = (GameObject)EditorGUILayout.ObjectField("Power Up Prefab", powerUpPrefab, typeof(GameObject), false);

            EditorGUILayout.Space();
            if (GUILayout.Button("Add Static Platform"))
            {
                CreateObject(staticPlatformPrefab);
            }
            if (GUILayout.Button("Add Moving Platform"))
            {
                CreateObject(movingPlatformPrefab);
            }
            if (GUILayout.Button("Add Wooden Wall"))
            {
                CreateObject(woodenWallPrefab);
            }
            if (GUILayout.Button("Add Destructible"))
            {
                CreateObject(destructiblePrefab);
            }
            if (GUILayout.Button("Add Coin"))
            {
                CreateObject(coinPrefab);
            }
            if (GUILayout.Button("Add Power Up"))
            {
                CreateObject(powerUpPrefab);
            }

            EditorGUI.indentLevel--;
        }
    }

    private void DrawSaveLoadSection()
    {
        showSaveLoad = EditorGUILayout.Foldout(showSaveLoad, "Save/Load", true);
        if (showSaveLoad)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Save Level"))
            {
                SaveLevel();
            }

            if (GUILayout.Button("Load Level"))
            {
                LoadLevel();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Clear Level"))
            {
                if (EditorUtility.DisplayDialog("Clear Level",
                    "Are you sure you want to clear the current level?",
                    "Yes", "No"))
                {
                    ClearLevel();
                }
            }

            EditorGUI.indentLevel--;
        }
    }

    private void CreateObject(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab not assigned! Please assign a prefab in the Level Builder window.");
            return;
        }

        GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        if (obj != null)
        {
            obj.transform.position = SceneView.lastActiveSceneView.pivot;
            
            // Add appropriate component based on prefab type
            if (prefab == staticPlatformPrefab || prefab == movingPlatformPrefab)
            {
                var platform = obj.AddComponent<PlatformComponent>();
                platform.isMoving = prefab == movingPlatformPrefab;
                platform.movementType = "Horizontal";
                platform.speed = 2f;
                platform.waitTime = 1f;
                Debug.Log($"Created {(platform.isMoving ? "moving" : "static")} platform at {obj.transform.position}");
            }
            else if (prefab == woodenWallPrefab || prefab == destructiblePrefab)
            {
                var obstacle = obj.AddComponent<ObstacleComponent>();
                obstacle.obstacleType = prefab == destructiblePrefab ? "Destructible" : "WoodenWall";
                obstacle.isDestructible = prefab == destructiblePrefab;
                Debug.Log($"Created {obstacle.obstacleType} at {obj.transform.position}");
            }
            else if (prefab == coinPrefab || prefab == powerUpPrefab)
            {
                var collectible = obj.AddComponent<CollectibleComponent>();
                collectible.collectibleType = prefab == coinPrefab ? "Coin" : "PowerUp";
                collectible.value = prefab == coinPrefab ? 1f : 10f;
                Debug.Log($"Created {collectible.collectibleType} at {obj.transform.position}");
            }

            levelObjects.Add(obj);
            Selection.activeObject = obj;
        }
        else
        {
            Debug.LogError($"Failed to instantiate prefab: {prefab.name}");
        }
    }

    private void SaveLevel()
    {
        string path = EditorUtility.SaveFilePanel(
            "Save Level",
            Path.Combine(Application.dataPath, "Resources/Levels"),
            levelName,
            "json"
        );

        if (string.IsNullOrEmpty(path)) return;

        // Clean up null objects from the list
        levelObjects.RemoveAll(obj => obj == null);

        LevelData levelData = new LevelData
        {
            levelName = levelName,
            startPoint = startPoint,
            endPoint = endPoint,
            objects = new LevelObject[levelObjects.Count]
        };

        for (int i = 0; i < levelObjects.Count; i++)
        {
            GameObject obj = levelObjects[i];
            LevelObject levelObj = CreateLevelObject(obj);
            if (levelObj != null)
            {
                levelData.objects[i] = levelObj;
            }
        }

        string json = JsonUtility.ToJson(levelData, true);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
        Debug.Log($"Level saved to: {path}");
    }

    private LevelObject CreateLevelObject(GameObject obj)
    {
        if (obj == null) return null;

        Transform transform = obj.transform;
        
        // Check for components instead of tags
        var platform = obj.GetComponent<PlatformComponent>();
        if (platform != null)
        {
            return new PlatformData
            {
                type = platform.isMoving ? "MovingPlatform" : "Platform",
                position = transform.position,
                scale = transform.localScale,
                rotation = transform.rotation.eulerAngles.z,
                isMoving = platform.isMoving,
                isDestructible = platform.isDestructible,
                speed = platform.speed,
                waitTime = platform.waitTime
            };
        }

        var obstacle = obj.GetComponent<ObstacleComponent>();
        if (obstacle != null)
        {
            return new ObstacleData
            {
                type = obstacle.obstacleType,
                position = transform.position,
                scale = transform.localScale,
                rotation = transform.rotation.eulerAngles.z,
                obstacleType = obstacle.obstacleType,
                damage = obstacle.damage,
                isDestructible = obstacle.isDestructible,
                health = obstacle.health
            };
        }

        var collectible = obj.GetComponent<CollectibleComponent>();
        if (collectible != null)
        {
            return new CollectibleData
            {
                type = collectible.collectibleType,
                position = transform.position,
                scale = transform.localScale,
                rotation = transform.rotation.eulerAngles.z,
                collectibleType = collectible.collectibleType,
                value = collectible.value
            };
        }

        // If no component is found, create a basic level object
        return new LevelObject
        {
            type = obj.name.Replace("(Clone)", "").Trim(),
            position = transform.position,
            scale = transform.localScale,
            rotation = transform.rotation.eulerAngles.z
        };
    }

    private void LoadLevel()
    {
        string path = EditorUtility.OpenFilePanel(
            "Load Level",
            Path.Combine(Application.dataPath, "Resources/Levels"),
            "json"
        );

        if (string.IsNullOrEmpty(path)) return;

        string json = File.ReadAllText(path);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        ClearLevel();

        levelName = levelData.levelName;
        startPoint = levelData.startPoint;
        endPoint = levelData.endPoint;

        foreach (LevelObject obj in levelData.objects)
        {
            if (obj == null) continue;

            GameObject prefab = GetPrefabForType(obj.type);
            if (prefab != null)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (instance != null)
                {
                    instance.transform.position = obj.position;
                    instance.transform.localScale = obj.scale;
                    instance.transform.rotation = Quaternion.Euler(0, 0, obj.rotation);
                    levelObjects.Add(instance);
                }
            }
        }
    }

    private GameObject GetPrefabForType(string type)
    {
        switch (type)
        {
            case "Platform": return staticPlatformPrefab;
            case "MovingPlatform": return movingPlatformPrefab;
            case "WoodenWall": return woodenWallPrefab;
            case "Destructible": return destructiblePrefab;
            case "Coin": return coinPrefab;
            case "PowerUp": return powerUpPrefab;
            default: return null;
        }
    }

    private void ClearLevel()
    {
        foreach (GameObject obj in levelObjects)
        {
            if (obj != null)
            {
                DestroyImmediate(obj);
            }
        }
        levelObjects.Clear();
    }

    private void OnEnable()
    {
        // Try to find prefabs in the project
        if (staticPlatformPrefab == null)
            staticPlatformPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/StaticPlatform.prefab");
        if (movingPlatformPrefab == null)
            movingPlatformPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MovingPlatform.prefab");
        if (woodenWallPrefab == null)
            woodenWallPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/WoodenWall.prefab");
        if (destructiblePrefab == null)
            destructiblePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Destructible.prefab");
        if (coinPrefab == null)
            coinPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Coin.prefab");
        if (powerUpPrefab == null)
            powerUpPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PowerUp.prefab");
    }
} 