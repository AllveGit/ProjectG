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
    private GameObject m_SpawnPos;

    [SerializeField]
    private int Width;
    [SerializeField]
    private int Height;

    [SerializeField]
    private float RealWidth;
    [SerializeField]
    private float RealHeight;

    public float TileSize;

    void Awake()
    {
        m_mapData = Resources.Load<MapData>(dataPath + MapDataName);
        m_SpawnPos = Resources.Load<GameObject>("SpawnPosition");
        SoundManager.instance.PlayMusic(Resources.Load<AudioClip>("Sounds/xeon6"));
        Parsing();
    }

    void Update()
    {
    }

    void Parsing()
    {
        Width = m_mapData.Width;
        Height = m_mapData.Height;

        RealWidth = Width * TileSize;
        RealHeight = Height * TileSize;

        m_GridArr = new GameObject[Height * Width];

        float x = -(RealWidth / 2);
        float y = -(RealHeight / 2);

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                GameObject Temp;

                switch (m_mapData.m_MapInfo[(i * Width) + j])
                {
                    case "GRID_SPACE":
                        Temp = Instantiate(m_mapData.spacePrefab, new Vector3(x + (j * TileSize), 0, y + (i * TileSize)),
                           Quaternion.Euler(-90, 0, 0), this.transform) as GameObject;
                        break;
                    case "GRID_WALL":
                        Instantiate(m_mapData.spacePrefab, new Vector3(x + (j * TileSize), 0, y + (i * TileSize)),
                           Quaternion.Euler(-90, 0, 0), this.transform);

                        Temp = Instantiate(m_mapData.wallPrefab, new Vector3(x + (j * TileSize), TileSize, y + (i * TileSize)),
                            Quaternion.Euler(-90, 0, 0), this.transform) as GameObject;
                        break;
                    case "GRID_BUSH":
                        Temp = Instantiate(m_mapData.bushPrefab, new Vector3(x + (j * TileSize), 0, y + (i * TileSize)),
                            Quaternion.Euler(-90, 0, 0), this.transform) as GameObject;
                        break;
                    case "GRID_WATER":
                        Temp = Instantiate(m_mapData.waterPrefab, new Vector3(x + (j * TileSize), 0.6f, y + (i * TileSize)),
                            Quaternion.Euler(0, 0, 0), this.transform) as GameObject;
                        break;
                    case "GRID_SPAWN":
                        Temp = Instantiate(m_mapData.spawnPrefab, new Vector3(x + (j * TileSize), 0, y + (i * TileSize)),
                           Quaternion.Euler(-90, 0, 0), this.transform) as GameObject;

                        Instantiate(m_SpawnPos, new Vector3(x + (j * TileSize), 0, y + (i * TileSize)),
                            Quaternion.identity, this.transform.GetChild(0));
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
