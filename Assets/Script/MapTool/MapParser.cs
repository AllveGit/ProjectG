using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapParser : MonoBehaviour
{
    MapData m_mapData;

    string dataPath = "MapTool/Data/";
    public string MapDataName;

    private GameObject[] m_GridArr;

    [SerializeField]
    private int Width;
    [SerializeField]
    private int Height;

    void Awake()
    {
        m_mapData = Resources.Load<MapData>(dataPath + MapDataName);
        Parsing();
    }

    void Update()
    {
        
    }

    void Parsing()
    {
        Width = m_mapData.Width;
        Height = m_mapData.Height;

        m_GridArr = new GameObject[Height * Width];

        float x = -(Width / 2);
        float y = -(Height / 2);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                GameObject Temp/* = new GameObject()*/;

                switch (m_mapData.m_MapInfo[(i * Width) + j])
                {
                    case "GRID_SPACE":
                        Temp = Instantiate(m_mapData.spacePrefab, new Vector3(x + j, 0, y + i),
                           Quaternion.identity, this.transform) as GameObject;
                        break;
                    case "GRID_WALL":
                        Instantiate(m_mapData.spacePrefab, new Vector3(x + j, 0, y + i),
                           Quaternion.identity, this.transform);

                        Temp = Instantiate(m_mapData.wallPrefab, new Vector3(x + j, 1, y + i),
                            Quaternion.identity, this.transform) as GameObject;
                        break;
                    case "GRID_BUSH":
                        Instantiate(m_mapData.spacePrefab, new Vector3(x + j, 0, y + i),
                            Quaternion.identity, this.transform);

                        Temp = Instantiate(m_mapData.bushPrefab, new Vector3(x + j, 1, y + i),
                            Quaternion.identity, this.transform) as GameObject;
                        break;
                    case "GRID_WATER":
                        Instantiate(m_mapData.spacePrefab, new Vector3(x + j, 0, y + i),
                            Quaternion.identity, this.transform);

                        Temp = Instantiate(m_mapData.waterPrefab, new Vector3(x + j, 1, y + i),
                            Quaternion.identity, this.transform) as GameObject;
                        break;
                    case "GRID_SPAWN":
                        Temp = Instantiate(m_mapData.spawnPrefab, new Vector3(x + j, 0, y + i),
                            Quaternion.identity, this.transform) as GameObject;
                        break;
                    default:
                        Temp = new GameObject();
                        break;
                }

                m_GridArr[(i * Width) + j] = Temp;

                Temp.transform.SetParent(this.transform);
            }
        }
    }
}
