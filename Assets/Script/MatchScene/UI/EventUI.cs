using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Closing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Closing()
    {
        yield return new WaitForSeconds(1.5f);

        while (transform.localScale.x > 0.1)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.15f);
            yield return null;
        }

        Destroy(gameObject);
    }
}
