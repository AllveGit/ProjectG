using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    public bool flag;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [SerializeField]
    public BoolEvent toggleClick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnToggleClick()
    {
        toggleClick?.Invoke(flag);
        flag = !flag;
    }
}
