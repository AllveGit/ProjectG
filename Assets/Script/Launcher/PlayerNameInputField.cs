using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

// 이 스크립트를 컴포넌트에 추가할시 자동으로 아래 컴포넌트를 추가합니다 존재하지않을시 에러를 표시합니다.
[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    // 사용자 이름을 저장할 PlayerPrefab의 키 입니다.
    const string playerNamePrefKey = "PlayerName";

    private void Start()
    {
        string defaultName = string.Empty;

        // InputFiled Component를 받아옵니다.
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            // PlaterPrefs 에서 해당 Hash키로 문자열을 찾습니다.
            // 만약 있을 경우 현재 기본으로  설정된 문자열을 저장된 문자열로 바꿉니다.
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
        }

        // Network 상 플레이어 이름을 설정합니다.
        PlayerManager.Instance.PlayerName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;
        
        PhotonNetwork.NickName = value;

        // PlayerPrefs에 해당 Hash 키값을 저장합니다.
        PlayerPrefs.SetString(playerNamePrefKey, value);
    }
}
