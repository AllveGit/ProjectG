using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    public Sprite[] profileImgs;

    public GameObject profileImg;

    public int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProfileImgChange(int index)
    {
        if (profileImgs[index] == null)
            return;

        profileImg.GetComponent<UnityEngine.UI.Image>().sprite = profileImgs[index];
        currentIndex = index;
    }
}
