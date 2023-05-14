using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryFinder : MonoBehaviour
{
    public GameObject Player;
    public GameObject Trajector;
    private LineRenderer lineRendererComponent;
    public int len;

    private Rigidbody2D rb;
    private GameObject[] Planets;

    private Vector3[] Points;

    private int counter;
    // Start is called before the first frame update
    void Start()
    {
        Points = new Vector3[len];
        transform.position = Player.transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.mass = Player.GetComponent<Rigidbody2D>().mass;
        Planets = GameObject.FindGameObjectsWithTag("Planet");
        lineRendererComponent = Trajector.GetComponent<LineRenderer>();
        Time.timeScale = 20f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!Player.GetComponent<RocketMovement>().Moving)
        {
            if (counter == len)
            {
                counter = 0;
                transform.position = Player.transform.position;
                lineRendererComponent.SetPositions(Points);
                Points = new Vector3[len];
                rb.velocity = Vector3.zero;
                rb.AddForce(Player.GetComponent<RocketMovement>().ForceVector * Player.GetComponent<RocketMovement>().Force);
            }

            GravityForce();

            Points[counter] = transform.position;

            counter += 1;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void GravityForce()
    {
        Vector3 ResultVector = Vector3.zero;
        for (int i = 0; i < Planets.Length; i++)
        {
            Rigidbody2D rbP = Planets[i].GetComponent<Rigidbody2D>();
            float GravityForce = Player.GetComponent<RocketMovement>().G * (rb.mass * rbP.mass) / Mathf.Pow(Vector2.Distance(transform.position, Planets[i].transform.position), 2);
            Vector3 GravityVector = (Planets[i].transform.position - transform.position).normalized * GravityForce;
            ResultVector += GravityVector;
        }
        rb.AddForce(ResultVector);
    }
}
