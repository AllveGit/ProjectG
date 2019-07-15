using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownRender : MonoBehaviour
{
    public UnityEngine.UI.Text text;

    public BasePlayer targetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int count = (int)(targetPlayer.UltimateCooldown);

        text.text = $"{count.ToString()}S";

        if (count == 0)
        {
            text.text = "";
        }
    }
}
