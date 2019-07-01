using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelecter : MonoBehaviour
{
    // 선택지가 순회하는가
    public bool isCirculate;

    // 캐릭터 2D 이미지 뷰어
    public GameObject characterView;

    // 이미지 배열
    public Sprite[] characterSprites;

    // 현재 선택된 인덱스
    private int currentIndex = 0;
    private int maxIndex = 0;

    void Start()
    {
        maxIndex = characterSprites.Length;
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
        if (characterSprites[currentIndex] == null)
            return;

        characterView.GetComponent<UnityEngine.UI.Image>().sprite = characterSprites[currentIndex];
    }
}
