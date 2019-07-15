using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public Launcher photonLauncher;
    public MatchButtonFolding buttonFolder;
    public GameObject matchButton;

    public Sprite[] matchButtonImgs;

    private MatchingState panelState = MatchingState.None;

    enum MatchingState : int
    {
        None = 0,
        Matching = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        matchButton.GetComponent<UnityEngine.UI.Image>().sprite = matchButtonImgs[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButtonClick()
    {
        if (panelState == MatchingState.None)
        {
            buttonFolder.OnFoldButtonDown();
        }
        else
        {
            photonLauncher.MatchingCancel();
            matchButton.GetComponent<UnityEngine.UI.Image>().sprite = matchButtonImgs[0];
            panelState = MatchingState.None;
        }

        SoundManager.instance.PlayEffect(Resources.Load<AudioClip>("Sounds/click3"));
    }

    public void OnMatchingButtonClick(int matchMode)
    {
        Enums.MatchType matchType = (Enums.MatchType)matchMode;

        if (panelState == MatchingState.None)
        {
            photonLauncher.MatchingStart(matchType);
            buttonFolder.Fold();
            matchButton.GetComponent<UnityEngine.UI.Image>().sprite = matchButtonImgs[1];
            panelState = MatchingState.Matching;
        }

        SoundManager.instance.PlayEffect(Resources.Load<AudioClip>("Sounds/click3"));
    }
}
