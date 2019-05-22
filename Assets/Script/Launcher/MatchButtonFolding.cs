using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchButtonFolding : MonoBehaviour
{
    private RectTransform playerButtonMaskRT = null;

    private bool IsUnFold = false;
    private bool IsWorking = false;
    private float nowHeight = 0f;
    private const float maxHeight = 120f;
    
    [SerializeField]
    private float fLerpSpeed = 0.1f;
    private float fLerpDistance = 0f;

    void Start()
    {
        playerButtonMaskRT = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    
    public void OnUnFoldButton()
    {
        if (IsWorking)
            return;

        fLerpDistance = 0f;

        if (IsUnFold)
            StartCoroutine("FoldingTap");
        else
            StartCoroutine("UnfoldingTap");
    }

    IEnumerator UnfoldingTap()
    {
        IsWorking = true;
        while (true)
        {
            nowHeight = Mathf.Lerp(nowHeight, maxHeight, fLerpSpeed);
            playerButtonMaskRT.sizeDelta = new Vector2(160, nowHeight);

            if ((maxHeight - nowHeight) < 0.2f)
                break;

            yield return null;
        }
        nowHeight = maxHeight;
        playerButtonMaskRT.sizeDelta = new Vector2(160, nowHeight);

        IsWorking = false;
        IsUnFold = true;
        yield return null;
    }

    IEnumerator FoldingTap()
    {
        IsWorking = true;

        while (nowHeight >= 0)
        {
            nowHeight = Mathf.Lerp(nowHeight, 0, fLerpSpeed);
            playerButtonMaskRT.sizeDelta = new Vector2(160, nowHeight);

            if (nowHeight < 0.2f)
                break;

            yield return null;
        }
        nowHeight = 0;
        playerButtonMaskRT.sizeDelta = new Vector2(160, nowHeight);

        IsWorking = false;
        IsUnFold = false;
        yield return null;
    }
}
