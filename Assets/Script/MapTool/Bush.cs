using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public Renderer m_Mat;

    private void Awake()
    {
        m_Mat = GetComponent<Renderer>();
    }
}
