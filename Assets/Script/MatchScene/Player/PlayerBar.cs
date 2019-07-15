using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBar : MonoBehaviour
{
    [SerializeField]
    private Sprite redTeamHpGaugeSprite;
    [SerializeField]
    private Sprite blueTeamHpGaugeSprite;

    [SerializeField]
    private Image hpGauge;

    [SerializeField]
    private Image manaGauge;

    float oneHpLength = 0f;
    float oneShieldLength = 0f;

    GameObject ownerObject = null;

    private void Update()
    {
        if (ownerObject)
        {
            transform.position = Camera.main.WorldToScreenPoint(ownerObject.transform.position);
            transform.Translate(new Vector3(0f, 80f, 0f));
        }

    }

    public void BarInit(GameObject inOwnerObject, Enums.TeamOption teamOption, int maxHp, int maxShield)
    {
        hpGauge.sprite = (teamOption == Enums.TeamOption.BlueTeam) ? blueTeamHpGaugeSprite : redTeamHpGaugeSprite;
        oneHpLength = hpGauge.sprite.rect.width / maxHp;
        oneShieldLength = manaGauge.sprite.rect.width / maxShield;
        ownerObject = inOwnerObject;
    }

    public void UpdateHpBar(int hp)
    {
        hpGauge.rectTransform.sizeDelta = new Vector2(oneHpLength * hp, hpGauge.rectTransform.sizeDelta.y);
    }
    public void UpdateShieldBar(int shield)
    {
        manaGauge.rectTransform.sizeDelta = new Vector3(oneShieldLength * shield, manaGauge.rectTransform.sizeDelta.y);
    }
}
