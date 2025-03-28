using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

/// <summary>
/// Custom editor tool for placing and manipulating level objects
/// </summary>
[EditorTool("Level Object Placement Tool")]
public class ObjectPlacementTool : EditorTool
{
    private GameObject _selectedPrefab;
    private bool _isPlacingObject;
    private Vector2 _mousePosition;
    private GameObject _previewObject;
    private bool _isMovingPlatform;
    private Vector2 _movementStartPoint;

    public override void OnToolGUI(EditorWindow window)
    {
        Event evt = Event.current;
        _mousePosition = HandleUtility.GUIPointToWorldRay(evt.mousePosition).origin;

        // Draw toolbar
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 200, 300));
        DrawToolbar();
        GUILayout.EndArea();
        Handles.EndGUI();

        // Handle input
        switch (evt.type)
        {
            case EventType.MouseDown:
                if (evt.button == 0) // Left click
                {
                    if (_selectedPrefab != null)
                    {
                        if (!_isPlacingObject)
                        {
                            StartPlacingObject();
                        }
                        else
                        {
                            FinishPlacingObject();
                        }
                        evt.Use();
                    }
                }
                break;

            case EventType.MouseMove:
                if (_isPlacingObject && _previewObject != null)
                {
                    UpdatePreviewPosition();
                    window.Repaint();
                }
                break;

            case EventType.KeyDown:
                if (evt.keyCode == KeyCode.Escape)
                {
                    CancelPlacement();
                    evt.Use();
                }
                break;
        }

        // Draw preview
        if (_isPlacingObject && _previewObject != null)
        {
            DrawObjectPreview();
        }
    }

    private void DrawToolbar()
    {
        GUILayout.Label("Level Builder Tools", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Static Platform"))
        {
            SelectPrefab("StaticPlatform");
            _isMovingPlatform = false;
        }
        
        if (GUILayout.Button("Moving Platform"))
        {
            SelectPrefab("MovingPlatform");
            _isMovingPlatform = true;
        }
        
        if (GUILayout.Button("Wooden Wall"))
        {
            SelectPrefab("WoodenWall");
            _isMovingPlatform = false;
        }
        
        if (GUILayout.Button("Destructible"))
        {
            SelectPrefab("Destructible");
            _isMovingPlatform = false;
        }
        
        if (GUILayout.Button("Coin"))
        {
            SelectPrefab("Coin");
            _isMovingPlatform = false;
        }
        
        if (GUILayout.Button("Double Jump Powerup"))
        {
            SelectPrefab("DoubleJumpPowerup");
            _isMovingPlatform = false;
        }

        GUILayout.Space(10);
        
        if (GUILayout.Button("Clear Selection"))
        {
            ClearSelection();
        }
    }

    private void SelectPrefab(string prefabName)
    {
        // Load prefab from Resources folder
        _selectedPrefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");
        if (_selectedPrefab == null)
        {
            Debug.LogError($"Prefab not found: {prefabName}");
        }
        CancelPlacement();
    }

    private void StartPlacingObject()
    {
        _isPlacingObject = true;
        _previewObject = PrefabUtility.InstantiatePrefab(_selectedPrefab) as GameObject;
        _previewObject.hideFlags = HideFlags.HideInHierarchy;
        
        if (_isMovingPlatform)
        {
            _movementStartPoint = _mousePosition;
        }
        
        UpdatePreviewPosition();
    }

    private void FinishPlacingObject()
    {
        if (_previewObject != null)
        {
            GameObject finalObject = PrefabUtility.InstantiatePrefab(_selectedPrefab) as GameObject;
            finalObject.transform.position = _previewObject.transform.position;

            if (_isMovingPlatform)
            {
                MovingPlatform platform = finalObject.GetComponent<MovingPlatform>();
                if (platform != null)
                {
                    float distance = Vector2.Distance(_movementStartPoint, _mousePosition);
                    string movementType = Mathf.Abs(_mousePosition.x - _movementStartPoint.x) > 
                                       Mathf.Abs(_mousePosition.y - _movementStartPoint.y) ? 
                                       "horizontal" : "vertical";

                    PlatformProperties properties = new PlatformProperties
                    {
                        isMoving = true,
                        movementType = movementType,
                        speed = 2f, // Default speed
                        distance = distance
                    };

                    platform.Initialize(_movementStartPoint, properties);
                }
            }

            Undo.RegisterCreatedObjectUndo(finalObject, "Place Level Object");
        }

        CancelPlacement();
    }

    private void UpdatePreviewPosition()
    {
        if (_previewObject != null)
        {
            _previewObject.transform.position = new Vector3(
                _mousePosition.x,
                _mousePosition.y,
                0
            );
        }
    }

    private void CancelPlacement()
    {
        if (_previewObject != null)
        {
            DestroyImmediate(_previewObject);
            _previewObject = null;
        }
        _isPlacingObject = false;
    }

    private void ClearSelection()
    {
        _selectedPrefab = null;
        CancelPlacement();
    }

    private void DrawObjectPreview()
    {
        if (_previewObject != null)
        {
            // Draw preview mesh
            Handles.color = new Color(1f, 1f, 1f, 0.5f);
            Handles.DrawWireCube(
                _previewObject.transform.position,
                _previewObject.transform.localScale
            );

            // Draw movement preview for moving platforms
            if (_isMovingPlatform)
            {
                Handles.color = Color.yellow;
                Handles.DrawDottedLine(
                    _movementStartPoint,
                    _mousePosition,
                    4f
                );
            }
        }
    }

    public override void OnActivated()
    {
        ClearSelection();
    }

    public override void OnWillBeDeactivated()
    {
        CancelPlacement();
    }
} 