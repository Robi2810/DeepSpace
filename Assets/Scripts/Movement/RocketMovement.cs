using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketMovement : MonoBehaviour
{
    public Vector3 StartPos;
    public HandlerScript Handler;

    public float Force;
    public float Step;
    [Space]
    public float G;
    public GameObject[] Planets;

    private Rigidbody2D rb;
    [Space]
    public float Scale;
    private Vector3[] Points = new Vector3[2];
    public GameObject Arrow;
    private GameObject ArrowCopy;

    public GameObject BlackHole;
    public GameObject Bang;

    [HideInInspector]
    public bool startMove = false;
    public bool Moving = false;
    public bool mousePressed = false;
    public bool AbilityUse;
    public bool InBlackHole = false;

    [HideInInspector]
    public Vector2 start_pos;
    //private Vector3 posVector;
    private Vector2 end_pos;
    public Vector2 ForceVector;

    public int LocatorAbility;

    public int LevelNumber;

    //private Vector3 TestVector;
    

    void Start()
    {
        G = Handler.G;
        Force = Handler.Force;
        rb = GetComponent<Rigidbody2D>();
        Planets = GameObject.FindGameObjectsWithTag("Planet");
        Handler.LocatorAbility = LocatorAbility;
        Scale = Handler.Scale;
        
    }

    void Update()
    {
        AbilityUse = Handler.AbilityUse;
        if (AbilityUse == false && Moving == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Задаю начальное положение вектора
                start_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePressed = true;

                gameObject.GetComponent<LineRenderer>().enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                mousePressed = false;
                
                for (int i=0; i < Points.Length; i++)
                {
                    Points[i] = transform.position;
                }
                
                gameObject.GetComponent<LineRenderer>().enabled = false;
            }

            if (Input.GetMouseButtonUp(0) && mousePressed == true)
            {
                startMove = true;
                Moving = true;
                mousePressed = false;

                //Destroy(ArrowCopy);
                gameObject.GetComponent<LineRenderer>().enabled = false;

                // Вектор запуска ракеты
                ForceVector = start_pos - end_pos;
            }
            
            end_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if (InBlackHole == true)
        {
            rb.velocity = Vector3.zero;
            ObjectRotation(gameObject, BlackHole.transform.position - transform.position, Step);
            //transform.position += (BlackHole.transform.position - transform.position) * 0.005f;
            transform.Translate((BlackHole.transform.position - transform.position) * 0.01f);
        }
        
        if ((BlackHole.transform.position - transform.position).magnitude < 0.2f)
        {
            LoadScene();
        }
    }

    void FixedUpdate()
    {
        if (AbilityUse == false)
        {
            // Толкаю объект
            if (startMove == true)
            {
                rb.AddForce(ForceVector * Force);
                startMove = false;
            }

            if (Moving == true)
            {
                // Рассчет силы притяжения планет
                GravityForce();

                // поворот игрока по движению
                ObjectRotation(gameObject, rb.velocity, 1);
            }

            // Плавно поворачиваю объект
            if (mousePressed == true)
            {
                ObjectRotation(gameObject, start_pos - end_pos, Step);
                DrawArrow(start_pos - end_pos);
            }
        }
    }

    void GravityForce()
    {
        Vector3 ResultVector = Vector3.zero;
        for (int i = 0; i < Planets.Length; i++)
        {
            Rigidbody2D rbP = Planets[i].GetComponent<Rigidbody2D>();
            float GravityForce = G * (rb.mass * rbP.mass) / Mathf.Pow(Vector2.Distance(transform.position, Planets[i].transform.position), 2);
            Vector3 GravityVector = (Planets[i].transform.position - transform.position).normalized * GravityForce;
            ResultVector += GravityVector;
        }
        rb.AddForce(ResultVector, ForceMode2D.Impulse);
        //TestVector = ResultVector;
    }

    void ObjectRotation(GameObject Obj, Vector3 Target, float Step)
    {
        if (Obj.transform.up != Target.normalized)
        {
            Obj.transform.up += Target.normalized / Step;
        }
    }

    void DrawArrow(Vector3 Dir)
    {
        Points[0] = transform.position;
        Points[1] = transform.position + Dir * Scale;
        gameObject.GetComponent<LineRenderer>().SetPositions(Points);

        //ObjectRotation(Arrow, Dir, 1);
        //ArrowCopy.transform.position = (Dir * Scale + transform.position);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Planet" || col.gameObject.tag == "Meteor")
        {
            Crash();
        }

        if (col.gameObject.tag == "BlackHole")
        {
            //BlackHole.GetComponent<Animation>().Play("HoleSucking");
            Handler.AbilityUse = true;
            InBlackHole = true;
            gameObject.GetComponent<Animator>().SetBool("BecomeSmaller", true);
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(LevelNumber);
    }

    public void Crash()
    {
        Instantiate(Bang, transform.position, Quaternion.identity);
        gameObject.transform.position = StartPos;
        Handler.Heartcount -= 1;
        Handler.Crash = true;
        Moving = false;
        rb.velocity = Vector3.zero;
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + TestVector);
        //Gizmos.DrawLine(transform.position, transform.position + new Vector3(rb.velocity.x, rb.velocity.x, 0) / 5);
    }
    
}
