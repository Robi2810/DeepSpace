using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesMovement : MonoBehaviour
{
    public HandlerScript Handler;

    public float Force;
    public float Step;
    public float G;

    public GameObject[] Planets;

    private Vector3[] Points = new Vector3[2];
    private float Scale;

    private Rigidbody2D rb;

    public GameObject Bang;

    private bool startMove = false;
    public bool Moving = false;
    public bool mousePressed = false;

    private Vector2 start_pos;
    private Vector3 posVector;
    private Vector2 end_pos;
    public Vector2 ForceVector;

    void Start()
    {
        //rm = Player.GetComponent<RocketMovement>();
        //Force = rm.Force;
        //Step = rm.Step;
        //G = rm.Step;
        //Planets = rm.Planets;
        Handler = GameObject.FindGameObjectWithTag("Handler").GetComponent<HandlerScript>();
        G = Handler.G;
        Force = Handler.Force;
        rb = GetComponent<Rigidbody2D>();
        Planets = GameObject.FindGameObjectsWithTag("Planet");
        Scale = Handler.Scale;
    }

    void Update()
    {
        if (Moving == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Задаю начальное положение вектора
                start_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePressed = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                mousePressed = false;
                
                for (int i = 0; i < Points.Length; i++)
                {
                    Points[i] = transform.position;
                }
                
                gameObject.GetComponent<LineRenderer>().SetPositions(Points);
            }

            if (Input.GetMouseButtonUp(0) && ForceVector != Vector2.zero && mousePressed == true)
            {
                startMove = true;
                Moving = true;
                //Handler.Moving = Moving;

                for (int i = 0; i < Points.Length; i++)
                {
                    Points[i] = transform.position;
                }

                gameObject.GetComponent<LineRenderer>().SetPositions(Points);

                mousePressed = false;
            }

            // Вектор запуска ракеты
            end_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ForceVector = start_pos - end_pos;
        }
    }

    void FixedUpdate()
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
            PlayerRotation(rb.velocity, 1);
        }

        // Плавно поворачиваю объект
        if (mousePressed == true)
        {
            PlayerRotation(ForceVector, Step);
            DrawArrow(ForceVector);
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
    }

    void PlayerRotation(Vector3 Target, float Step)
    {
        if (transform.up != Target.normalized)
        {
            transform.up += Target.normalized / Step;
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
            GetComponent<Locator>().Die();
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.position + TestVector);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(rb.velocity.x, rb.velocity.y, 0) / 5);
    }
}
