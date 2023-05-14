using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    public HandlerScript Handler;

    public GameObject SpawnObject;
    private GameObject Player;

    public float SpawnTime;
    public float LifeTime;
    private float Life;
    private float StartSpawnTime = 0.1f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Handler = GameObject.FindGameObjectWithTag("Handler").GetComponent<HandlerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<AbilitiesMovement>().Moving)
        {
            if (StartSpawnTime <= 0)
            {
                StartSpawnTime = SpawnTime;
                Instantiate(SpawnObject, transform.position, Quaternion.identity);
            }

            if ( Life >= LifeTime)
            {
                Die();
            } 

            StartSpawnTime -= Time.deltaTime;
            Life += Time.deltaTime;
        }
        
    }

    void OnBecameInvisible()
    {
        Die();
    }

    public void Die()
    {
        Handler.AbilityUse = false;
        Player.GetComponent<CapsuleCollider2D>().isTrigger = false;
        Player.transform.rotation = Quaternion.Euler(0, 0, -90);
        Player.GetComponent<RocketMovement>().mousePressed = false;
        Handler.UseAgain = true;
        Handler.Moving = false;
        Instantiate(gameObject.GetComponent<AbilitiesMovement>().Bang, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
