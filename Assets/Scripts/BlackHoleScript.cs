using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleScript : MonoBehaviour
{
    public GameObject Rocket;
    public GameObject Camera;
    public HandlerScript Handler;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartAnim()
    {
        Handler.AbilityUse = true;
    }
}
