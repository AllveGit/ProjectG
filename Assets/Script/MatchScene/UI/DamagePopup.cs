using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public Animator animator;
    public Text damageText;

    void Start()
    {
        animator = GetComponent<Animator>();
        //damageText = this.gameObject.GetComponent<Text>();
        Destroy(gameObject, 10f);
    }

    public void SetText(float _Damage, Vector3 position)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);

        transform.localPosition = screenPosition;
        damageText.text = _Damage.ToString();
    }
}
