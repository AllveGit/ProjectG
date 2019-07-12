using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewer : MonoBehaviour
{
    public Sprite[] characterModels;

    public UnityEngine.UI.Image targetImg;

    // Start is called before the first frame update
    void Start()
    {
        SetIndex((int)PlayerManager.Instance.CharacterType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIndex(int index)
    {
        if (characterModels[index] == null)
            return;

        targetImg.sprite = characterModels[index];
        //targetImg.SetNativeSize();
    }
}
