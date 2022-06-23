using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    public Transform ball;
    public Transform point1;
    public Transform point2;
    Line l;
    // Start is called before the first frame update
    void Start()
    {
        l = new Line(point1.position, point2.position);
    }

    // Update is called once per frame
    void Update()
    {
        ball.position = l.Lerp(Time.time / 3);    
    }
}
