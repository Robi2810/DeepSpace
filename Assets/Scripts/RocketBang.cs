using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketBang : MonoBehaviour
{
    public float LifeTime;
    private float time;
    void Start()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Destroy(gameObject, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (time > LifeTime)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        time += 0.1f;
        */
    }
}
