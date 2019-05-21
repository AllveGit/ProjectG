using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Map Data", menuName = "Create MapData")]
public class MapData : ScriptableObject
{
    public int Width;
    public int Height;

    public GameObject spacePrefab;
    public GameObject wallPrefab;
    public GameObject bushPrefab;
    public GameObject spawnPrefab;
    public GameObject waterPrefab;

    public List<string> m_MapInfo = new List<string>();
}
