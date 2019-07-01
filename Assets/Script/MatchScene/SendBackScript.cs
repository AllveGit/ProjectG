using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendBackScript : MonoBehaviour
{
    public GameObject damagePopup = null;
    float currentHp = 100;

    // Start is called before the first frame update

    public void ShowDamagePopUp()
    {
        var go = Instantiate(damagePopup, transform.position, Quaternion.identity);
        go.GetComponent<TextMesh>().text = currentHp.ToString();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
            ShowDamagePopUp();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
            ShowDamagePopUp();
    }


}
