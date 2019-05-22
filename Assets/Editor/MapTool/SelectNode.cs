using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum NodeState { NODE_SPACE, NODE_WALL, NODE_BUSH, NODE_SPAWN, NODE_WATER }

public class SelectNode : EditorWindow
{
    static MapData mapData;

    public static MapData mapInfo { get { return mapData; } }

    Texture2D BGTexture;

    Rect BGSection;
    Rect MainSection;

    NodeState m_State = NodeState.NODE_SPACE;

    public GameObject spacePrefab;
    public GameObject wallPrefab;
    public GameObject bushPrefab;
    public GameObject spawnPrefab;
    public GameObject waterPrefab;

    string DataName;
    string curGridName = "GRID_SPACE";

    Material spaceMaterial;
    Material wallMaterial;
    Material bushMaterial;
    Material spawnMaterial;
    Material waterMaterial;

    Material curMaterial;

    [MenuItem("MapTool/GridManager")]
    static void OpenWindow()
    {
        SelectNode window = (SelectNode)GetWindow(typeof(SelectNode));
        window.minSize = new Vector2(350, 500);
        window.Show();
    }

    private void OnEnable()
    {
        wallMaterial = Resources.Load<Material>("MapTool/Material/Wall");
        bushMaterial = Resources.Load<Material>("MapTool/Material/Bush");
        spaceMaterial = Resources.Load<Material>("MapTool/Material/Space");
        spawnMaterial = Resources.Load<Material>("MapTool/Material/Spawn");
        waterMaterial = Resources.Load<Material>("MapTool/Material/Water");

        mapData = (MapData)ScriptableObject.CreateInstance(typeof(MapData));

        BGTexture = Resources.Load<Texture2D>("MapTool/MapToolBG");
    }

    private void OnGUI()
    {
        BGSection.x = 0;
        BGSection.y = 0;
        BGSection.width = Screen.width;
        BGSection.height = 150;

        GUI.DrawTexture(BGSection, BGTexture);

        MainSection.x = 0;
        MainSection.y = 150;
        MainSection.width = Screen.width;
        MainSection.height = Screen.height - 150;

        GUILayout.BeginArea(MainSection);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("NodeState");
        m_State = (NodeState)EditorGUILayout.EnumPopup(m_State);
        EditorGUILayout.EndHorizontal();

        switch (m_State)
        {
            case NodeState.NODE_SPACE:
                curMaterial = spaceMaterial;
                curGridName = "GRID_SPACE";
                break;
            case NodeState.NODE_SPAWN:
                curMaterial = spawnMaterial;
                curGridName = "GRID_SPAWN";
                break;
            case NodeState.NODE_BUSH:
                curMaterial = bushMaterial;
                curGridName = "GRID_BUSH";
                break;
            case NodeState.NODE_WALL:
                curMaterial = wallMaterial;
                curGridName = "GRID_WALL";
                break;
            case NodeState.NODE_WATER:
                curMaterial = waterMaterial;
                curGridName = "GRID_WATER";
                break;
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Space_Prefab");
        spacePrefab = EditorGUILayout.ObjectField(spacePrefab, typeof(GameObject), false) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wall_Prefab");
        wallPrefab = EditorGUILayout.ObjectField(wallPrefab, typeof(GameObject), false) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Bush_Prefab");
        bushPrefab = EditorGUILayout.ObjectField(bushPrefab, typeof(GameObject), false) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Spawn_Prefab");
        spawnPrefab = EditorGUILayout.ObjectField(spawnPrefab, typeof(GameObject), false) as GameObject;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Water_Prefab");
        waterPrefab = EditorGUILayout.ObjectField(waterPrefab, typeof(GameObject), false) as GameObject;
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Change", GUILayout.Height(30)))
        { 
            foreach (GameObject obj in Selection.gameObjects)
            {
                obj.GetComponent<MeshRenderer>().material = curMaterial;
                obj.GetComponent<GridType>().Type = curGridName;
            }
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("DataName");
        DataName = EditorGUILayout.TextField(DataName, GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("SAVE", GUILayout.Height(30)))
        {
            SaveData();
        }

        GUILayout.EndArea();
    }

    void SaveData()
    {
        GridManager GridMgr = FindObjectOfType<GridManager>();

        mapData.Width = GridMgr.Width;
        mapData.Height = GridMgr.Height;
        mapData.spacePrefab = spacePrefab;
        mapData.wallPrefab = wallPrefab;
        mapData.spawnPrefab = spawnPrefab;
        mapData.waterPrefab = waterPrefab;
        mapData.bushPrefab = bushPrefab;

        GameObject[] TempArr = GridMgr.GetGridArr();

        for(int i = 0; i < mapData.Height; i++)
        {
            for(int j = 0; j < mapData.Width; j++)
            {
                mapData.m_MapInfo.Add(TempArr[i * mapData.Width + j].GetComponent<GridType>().Type); 
            }
        }

        string datapath = "Assets/Resources/MapTool/Data/";

        datapath = datapath + DataName + ".asset";
        AssetDatabase.CreateAsset(mapData, datapath);


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
