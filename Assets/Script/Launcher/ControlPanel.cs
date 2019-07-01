using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public Launcher photonLauncher;
    public MatchButtonFolding buttonFolder;
    public UnityEngine.UI.Text matchButtonText;

    private MatchingState panelState = MatchingState.None;

    enum MatchingState : int
    {
        None = 0,
        Matching = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        matchButtonText.text = "Play";
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
            matchButtonText.text = "Play";
            panelState = MatchingState.None;
        }
    }

    public void OnMatchingButtonClick(int matchMode)
    {
        Enums.MatchType matchType = (Enums.MatchType)matchMode;

        if (panelState == MatchingState.None)
        {
            photonLauncher.MatchingStart(matchType);
            buttonFolder.Fold();
            matchButtonText.text = "Matching";
            panelState = MatchingState.Matching;
        }
    }
}
