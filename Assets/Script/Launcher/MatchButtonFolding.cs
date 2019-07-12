using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchButtonFolding : MonoBehaviour
{
    private RectTransform playerButtonMaskRT = null;

    private bool IsUnFold = false;
    private float nowHeight = 0f;
    public float maxHeight;
    public float maxWidth;

    [SerializeField]
    private float fLerpSpeed = 0.1f;

    void Start()
    {
        playerButtonMaskRT = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame

    public void OnFoldButtonDown()
    {
        if (IsUnFold)
            Fold();
        else
            UnFold();
    }

    public void Fold()
    {
        StopCoroutine("UnfoldingTap");
        StartCoroutine("FoldingTap");
    }

    public void UnFold()
    {
        StopCoroutine("FoldingTap");
        StartCoroutine("UnfoldingTap");
    }

    IEnumerator UnfoldingTap()
    {
        IsUnFold = true;

        while (true)
        {
            nowHeight = Mathf.Lerp(nowHeight, maxHeight, fLerpSpeed);
            playerButtonMaskRT.sizeDelta = new Vector2(maxWidth, nowHeight);

            if ((maxHeight - nowHeight) < 0.2f)
                break;

            yield return null;
        }
        nowHeight = maxHeight;
        playerButtonMaskRT.sizeDelta = new Vector2(maxWidth, nowHeight);

        yield break;
    }

    IEnumerator FoldingTap()
    {
        IsUnFold = false;

        while (nowHeight >= 0)
        {
            nowHeight = Mathf.Lerp(nowHeight, 0, fLerpSpeed);
            playerButtonMaskRT.sizeDelta = new Vector2(maxWidth, nowHeight);

            if (nowHeight < 0.2f)
                break;

            yield return null;
        }

        nowHeight = 0;
        playerButtonMaskRT.sizeDelta = new Vector2(maxWidth, nowHeight);

        yield break;
    }
}
