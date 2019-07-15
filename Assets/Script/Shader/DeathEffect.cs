using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    bool bActive = true;
    float fAmount = 0f;

    Renderer m_Render;

    void Start()
    {
        m_Render = GetComponent<Renderer>();
    }

    void Update()
    {
        if(bActive)
        {
            fAmount += Time.deltaTime * 0.006f;

            if (fAmount > 0.015f)
                fAmount = 0.015f;
        }

        else
        {
            fAmount -= Time.deltaTime * 0.006f;

            Destroy(this.gameObject);
        }

        m_Render.material.SetFloat("Vector1_B140D59", fAmount);
    }

    public IEnumerator ActiveFalse(float delay)
    {
        yield return new WaitForSeconds(delay);

        bActive = false;

        yield break;
    }
}
