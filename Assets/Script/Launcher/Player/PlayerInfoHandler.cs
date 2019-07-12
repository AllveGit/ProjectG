using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoHandler : MonoBehaviour
{
    public UnityEngine.UI.Text nameField;
    public UnityEngine.UI.Text levelField;

    // Start is called before the first frame update
    void Start()
    {
        OnNameChanged();
        OnLevelChanged();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnNameChanged()
    {
        nameField.text = PlayerManager.Instance.PlayerName;
    }

    public void OnLevelChanged()
    {
        levelField.text = $"Lv.{PlayerManager.Instance.PlayerLevel}";
    }
}
