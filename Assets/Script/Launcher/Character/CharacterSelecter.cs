using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelecter : MonoBehaviour
{
    public Launcher photonLauncher;

    // 선택지가 순회하는가
    public bool isCirculate;

    // 캐릭터 선택 2D 이미지 뷰어
    public GameObject characterSelectViewer;

    // 배경 뷰어
    public GameObject backgroundView;

    // 캐릭터 일러스트 2D 이미지 뷰어
    public CharacterViewer characterViewer;

    // 이미지 배열
    public Sprite[] characterSprites;

    // 배경 배열 ( 배경은 캐릭터에 따라 다르게 바뀜 )
    public Sprite[] backgroundSprites;

    // 현재 선택된 인덱스
    [SerializeField]
    private Enums.CharacterIndex currentIndex = 0;
    [SerializeField]
    private Enums.CharacterIndex maxIndex = 0;

    void Start() 
    {
        maxIndex = (Enums.CharacterIndex)characterSprites.Length - 1;
    }

    void Update()
    {
        
    }

    public void Prev()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            if (isCirculate)
            {
                currentIndex = maxIndex;
            }
            else
            {
                currentIndex = 0;
            }
        }

        SoundManager.instance.PlayEffect(Resources.Load<AudioClip>("Sounds/click3"));

        Apply();
    }

    public void Next()
    {
        currentIndex++;

        if (currentIndex > maxIndex)
        {
            if (isCirculate)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex = maxIndex;
            }
        }

        SoundManager.instance.PlayEffect(Resources.Load<AudioClip>("Sounds/click3"));

        Apply();
    }

    private void Apply()
    {
        if (characterSprites[(int)currentIndex] == null)
            return;

        characterSelectViewer.GetComponent<UnityEngine.UI.Image>().sprite = characterSprites[(int)currentIndex];
        PlayerManager.Instance.CharacterType = currentIndex;
    }

    public void ApplyToModelViewer()
    {
        characterViewer.SetIndex((int)currentIndex);

        if (backgroundSprites[(int)currentIndex] == null)
            return;

        backgroundView.GetComponent<UnityEngine.UI.Image>().sprite = backgroundSprites[(int)currentIndex];
    }
}
