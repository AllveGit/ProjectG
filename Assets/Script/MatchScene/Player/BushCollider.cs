using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public partial class BushCollider : MonoBehaviourPun
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
        if (!parentPlayer.photonView.IsMine)
            return;

        if (other.gameObject.tag == "RealBush")
        {
            other.gameObject.GetComponent<Bush>().m_Mat.material.SetColor("_TintColor", new Color(0.5f,0.5f,0.5f,0.15f));
            onBushEnter?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!parentPlayer.photonView.IsMine)
            return;

        if (other.gameObject.tag == "RealBush")
        {
            other.gameObject.GetComponent<Bush>().m_Mat.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 1f));
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
