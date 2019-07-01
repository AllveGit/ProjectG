using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSelectButton : MonoBehaviour
{
    public PlayerProfile Original;
    public PlayerProfile CopyTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Apply()
    {
        CopyTarget.ProfileImgChange(Original.currentIndex);
    }
}
