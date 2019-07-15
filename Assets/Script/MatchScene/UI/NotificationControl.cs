using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationControl : MonoBehaviour
{
    public enum ResultWindow
    {
        WinWindow,
        FailedWindow,
    }

    public class EnumEvent : UnityEvent<ResultWindow> { }

    public GameObject Ready;
    public GameObject Go;
    public GameObject End;

    public GameObject winnerWindow;
    public GameObject loserWindow;

    public UnityEvent OnGameStart;
    public UnityEvent OnGameEnd;
    public EnumEvent ResultEvent;

    private void Awake()
    {
        Ready.SetActive(false);
        Go.SetActive(false);
        End.SetActive(false);        
    }

    // Start is called before the first frame update
    void Start()
    {
        ReadyGoStart(1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Ready Go UI 띄우고
    /// 시작해야 할 때 OnGameStart 이벤트 호출해줌
    /// </summary>
    public void ReadyGoStart(float readyDelay)
    {
        StartCoroutine("ReadyGoCoroutine", readyDelay);
    }

    /// <summary>
    /// End UI 띄우고
    /// 끝난 시점에 이벤트 호출해줌
    /// </summary>
    public void EndStart(float endDelay, ResultWindow result)
    {
        StartCoroutine(EndCoroutine(endDelay, result));
    }


    private IEnumerator ReadyGoCoroutine(float readyDelay)
    {
        // READY
        Ready.SetActive(true);
        Go.SetActive(false);
        End.SetActive(false);

        yield return new WaitForSeconds(readyDelay);

        // GO
        Ready.SetActive(false);
        Go.SetActive(true);
        End.SetActive(false);

        OnGameStart?.Invoke();

        yield return new WaitForSeconds(1.5f);

        // 전부 꺼준다.
        Ready.SetActive(false);
        Go.SetActive(false);
        End.SetActive(false);

        yield break;
    }

    private IEnumerator EndCoroutine(float endDelay, ResultWindow gameResult)
    {
        // END
        Ready.SetActive(false);
        Go.SetActive(false);
        End.SetActive(true);

        yield return new WaitForSeconds(endDelay);

        // 전부 꺼준다.
        Ready.SetActive(false);
        Go.SetActive(false);
        End.SetActive(false);

        OnGameEnd?.Invoke();

        if (gameResult == ResultWindow.WinWindow)
            winnerWindow.SetActive(true);
        else
            loserWindow.SetActive(true);

        yield break;
    }
}
