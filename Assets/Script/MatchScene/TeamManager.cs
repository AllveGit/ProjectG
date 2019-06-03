using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool IsAttackable(string myTag, string targetTag)
    {
        if (myTag == targetTag)
            return false;

        return true;
    }
}
