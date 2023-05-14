using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public HandlerScript Handler;

    public GameObject Player;

    public GameObject Locator;

    public Text TextField;

    public GameObject[] Hearts = new GameObject[3];

    private RocketMovement rm;



    //private bool locatorActivate = false;
    // Start is called before the first frame update
    void Start()
    {
        rm = Player.GetComponent<RocketMovement>(); 
    }

    // Update is called once per frame
    void Update()
    {
        TextField.text = Handler.LocatorAbility.ToString();
        if (Handler.Crash == true)
        {
            Hearts[Handler.Heartcount].SetActive(false);
            Handler.Crash = false;
        }
        
        if (Handler.Heartcount == 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void LocatorEnabele()
    {
        // Что происходит при нажатии на кнопку с локатором
        if (Player.GetComponent<Rigidbody2D>().velocity == Vector2.zero && Handler.UseAgain == true && Handler.LocatorAbility > 0)
        {
            Handler.UseAgain = false;
            Handler.AbilityUse = true;
            //rm.startSimulate = false;
            Instantiate(Locator, Player.transform.position, Quaternion.identity);
            Player.GetComponent<CapsuleCollider2D>().isTrigger = true;
            Handler.LocatorAbility -= 1;
            TextField.text = Handler.LocatorAbility.ToString();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        Player.GetComponent<RocketMovement>().Crash();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(0);
        SceneManager.LoadScene("Tutorial");
    }
}
