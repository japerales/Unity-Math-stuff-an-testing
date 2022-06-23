using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathLibrary;
public class PlaneTest : MonoBehaviour
{
    public Transform p1, p2, p3;

    MathLibrary.Plane p;
    // Start is called before the first frame update
    void Start()
    {
        p = new MathLibrary.Plane(p1.position, 
            p2.position, 
            p3.position);

        for (int i = 0; i < 20; i++)
            for (int e = 0; e < 10; e++)
            {
                float fi = i, fe = e;
                Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), p.GetPointInPlane(fi, fe), Quaternion.identity);
            }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
