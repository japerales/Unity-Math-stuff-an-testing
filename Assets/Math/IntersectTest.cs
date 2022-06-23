using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectTest : MonoBehaviour
{
    Coords coord1, coord2, coord3, coord4;
    // Start is called before the first frame update
    
    
    void Start()
    {
        coord1 = new Coords(0, 0);
        coord2 = new Coords(10, 10);
        coord3 = new Coords(10, 0);
        coord4 = new Coords(0, 10);
        Coords.DrawLine(coord1, coord2, 0.4f,Color.red);
        Coords.DrawLine(coord3, coord4, 0.4f, Color.blue);

        Line l1 = new Line(coord1.ToVector(), coord2.ToVector(), LineType.segment);
        Line l2 = new Line(coord3.ToVector(), coord4.ToVector(), LineType.segment);
        Vector3 intersect = l1.IntersectAtPoint(l2);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = intersect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
