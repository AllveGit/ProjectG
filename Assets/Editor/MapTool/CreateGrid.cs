using System.Collections;
using UnityEngine;
using UnityEditor;

public class CreateGrid : EditorWindow
{
    int Width = 0;
    int Height = 0;

    [MenuItem("MapTool/CreateGrid")]
    static void OpenWindow()
    {
        CreateGrid window = (CreateGrid)GetWindow(typeof(CreateGrid));
        window.minSize = new Vector2(250, 200);
        window.Show();
    }

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        RenderGUI();        
    }

    //맵툴 GUI 렌더
    void RenderGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Grid Width ");
        Width = EditorGUILayout.IntField(Width);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Grid Height");
        Height = EditorGUILayout.IntField(Height);
        EditorGUILayout.EndHorizontal();

        if(GUILayout.Button("Create", GUILayout.Height(40)))
        {
            FindObjectOfType<GridManager>().SettingGrid(Width, Height);
        }
    }
}
