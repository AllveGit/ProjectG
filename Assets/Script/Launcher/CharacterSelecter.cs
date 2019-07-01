using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelecter : MonoBehaviour
{
    public Launcher photonLauncher;

    // 선택지가 순회하는가
    public bool isCirculate;

    // 캐릭터 2D 이미지 뷰어
    public GameObject characterView;

    // 배경 뷰어
    public GameObject backgroundView;

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

        Apply();
    }

    private void Apply()
    {
        if (characterSprites[(int)currentIndex] == null)
            return;

        characterView.GetComponent<UnityEngine.UI.Image>().sprite = characterSprites[(int)currentIndex];
        photonLauncher.characterType = currentIndex;

        if (backgroundSprites[(int)currentIndex] == null)
            return;

        backgroundView.GetComponent<UnityEngine.UI.Image>().sprite = backgroundSprites[(int)currentIndex];
    }
}
