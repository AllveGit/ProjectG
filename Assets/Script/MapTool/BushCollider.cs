using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushCollider : MonoBehaviour
{
    private SphereCollider m_coll;
    public bool m_bDie = false;
    int Count = 0;

    private void Start()
    {
        m_coll = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if(m_bDie)
        {
            if(Count == 0)
            {
                Destroy(this.gameObject);
            }

            Count = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_bDie)
        {
            if (other.gameObject.tag == "RealBush")
            {
                ++Count;
                other.gameObject.GetComponent<Bush>().m_Mat.material.SetFloat("Vector1_3BE6800", 1);
            }
        }
    }

    public void Dying()
    {
        m_bDie = true;
        Count = 1;
    }
}
