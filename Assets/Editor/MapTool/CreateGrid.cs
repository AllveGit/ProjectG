using System.Collections;
using UnityEngine;
using UnityEditor;

public class CreateGrid : EditorWindow
{
    Texture2D BGTexture;

    Rect BGSection;
    Rect MainSection;

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
        BGTexture = Resources.Load<Texture2D>("MapTool/MapToolBG");
    }

    private void OnGUI()
    {
        RenderGUI();        
    }

    //맵툴 GUI 렌더
    void RenderGUI()
    {
        BGSection.x = 0;
        BGSection.y = 0;
        BGSection.width = Screen.width;
        BGSection.height = 80;

        GUI.DrawTexture(BGSection, BGTexture);

        MainSection.x = 0;
        MainSection.y = 80;
        MainSection.width = Screen.width;
        MainSection.height = Screen.height - 80;

        GUILayout.BeginArea(MainSection);

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

        GUILayout.EndArea();
    }
}
