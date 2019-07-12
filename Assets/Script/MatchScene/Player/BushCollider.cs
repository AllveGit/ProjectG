using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class BushCollider : MonoBehaviour
{
    private new SphereCollider collider = null;
    private BasePlayer parentPlayer = null;

    public event UnityAction onBushEnter;
    public event UnityAction onBushExit;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        if (collider == null)
            Debug.LogError("BushCollider.cs / phereCollider 컴포넌트를 찾을 수 없습니다.");
        parentPlayer = transform.parent.GetComponent<BasePlayer>();
        if (parentPlayer == null)
            Debug.LogError("parentPlayer.cs / BasePlayer 컴포넌트를 찾을수 없습니다");
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "RealBush")
        {
            other.gameObject.GetComponent<Bush>().m_Mat.material.SetFloat("Vector1_3BE6800", 0.3f);
            onBushEnter?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RealBush")
        {
            other.gameObject.GetComponent<Bush>().m_Mat.material.SetFloat("Vector1_3BE6800", 1f);
            onBushExit?.Invoke();
        }
    }

    public void RespawnProccess()
    {
        onBushExit?.Invoke();
        collider.enabled = true;
    }
    public void DieProccess()
    {
        onBushExit?.Invoke();
        collider.enabled = false;
    }
}
