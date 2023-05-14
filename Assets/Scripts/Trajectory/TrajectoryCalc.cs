using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryCalc : MonoBehaviour
{
    public Vector3[] Points;
    private LineRenderer lineRendererComponent;
    void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRendererComponent.SetPositions(Points);
    }
}
