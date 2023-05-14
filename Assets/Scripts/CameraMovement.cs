using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public HandlerScript Handler;
    [Space]
    public float UpDownBorder;
    public float LeftRightBorder;
    public float Speed;
    public float Smoothing;
    [Space]
    public float MinZoom;
    public float MaxZoom;
    public float ZoomSpeed;
    [Space]
    public GameObject Partical;

    private Vector3 pos;
    private Vector3 pr_pos;

    private Vector3 Target;
    void Start()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pr_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            pr_pos = pos;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Target = transform.position;
        }

        // Передвижение цели
        if (Input.GetMouseButton(1))
        {
            if (Handler.Moving == false)
            {
                Target += -(pos - pr_pos) * Time.deltaTime * Speed;
                Target = new Vector3(Mathf.Clamp(Target.x, -LeftRightBorder, LeftRightBorder), Mathf.Clamp(Target.y, -UpDownBorder, UpDownBorder), transform.position.z);
            }   
        }
        // Перемещение самой камеры 
        transform.position = Vector3.Lerp(transform.position, Target, Smoothing);

        // Управление зумом
        gameObject.GetComponent<Camera>().orthographicSize += -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomSpeed;
        gameObject.GetComponent<Camera>().orthographicSize = Mathf.Clamp(gameObject.GetComponent<Camera>().orthographicSize, MinZoom, MaxZoom);

        // Перемещение звездного неба
        Partical.transform.position = new Vector3(0, gameObject.transform.position.y + gameObject.GetComponent<Camera>().orthographicSize, -transform.position.z);

        pr_pos = pos;
    }
}
