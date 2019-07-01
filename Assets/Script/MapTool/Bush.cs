using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public Renderer m_Mat;

    private void Start()
    {
        m_Mat = GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Bush")
        {
            m_Mat.material.SetFloat("Vector1_3BE6800", 0.2f); //Alpha
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bush")
        {
            m_Mat.material.SetFloat("Vector1_3BE6800", 1f); //Alpha
        }
    }
}
