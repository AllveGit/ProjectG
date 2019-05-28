using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private GameObject[] m_GridArr;
    private GameObject GridNode;

    float x = 0;
    float y = 0;

    public int Width;
    public int Height;

    public GameObject[] GetGridArr() { return m_GridArr; }

    public void SettingGrid(int _Width, int _Height)
    {
        if(transform.childCount > 0)
        {
            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        GridNode = Resources.Load<GameObject>("MapTool/GridNode");

        Width = _Width;
        Height = _Height;

        m_GridArr = new GameObject[Height * Width];

        x = -(Width / 2);
        y = -(Height / 2);

        for (int i = 0; i < Height; i++)
        {
            for(int j = 0; j < Width; j++)
            {
                GameObject Temp = Instantiate(GridNode, new Vector3(x + j, y + i, 0),
                    Quaternion.identity, this.transform) as GameObject;

                m_GridArr[(i * Width) + j] = Temp;

                Temp.transform.SetParent(this.transform);
            }
        }
    }
}
