using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//lo que cambia son los limites del trazo:
//segment: 0<= t <=1
//line: infinito <= t <= infinito
//ray: 0<=t<=infinito
public enum LineType { segment, line, ray }

/// <summary>
/// La expresión parametrica de una linea es:
/// A + vt, donde A es un punto, v es un vector y t es
/// el punto de la linea que queremos averiguar.

public class Line 
{
    public Vector3 a, b, v;
    LineType linetype;
    //constructor donde definimos del punto A a B
    public Line(Vector3 _a, Vector3 _b, LineType type)
    {
        a = _a;
        b = _b;
        v = _b - _a;
        linetype = type;
    }
    //constructor donde definimos el punto A y su dirección v
    public Line(Vector3 _a, Vector3 _v)
    {
        a = _a;
        v = _v;
        b = a + v;
    }

    public Vector3 Lerp(float t)
    {
        switch (linetype)
        {
            case LineType.segment:
                t = Mathf.Clamp01(t); break;
            case LineType.line:
                t = Mathf.Clamp(t, -Mathf.Infinity, Mathf.Infinity); break;
            case LineType.ray:
                t = Mathf.Clamp(t, 0, Mathf.Infinity); break;
        }
        
        return a + v * t;
    }


    /// Para saber la intersección de dos lineas:
    /// A + vt = B + us ===> vt = B-A + us, convertimos B-A en C
    /// vt = C + us ===> añadimos un perp u┬
    /// u┬vt = u┬C + u┬us ==> u┬vt = u┬C
    /// t = u┬C / u┬v
    /// </summary>
    public float IntersectAt(Line l)
    {
        //si el dot de la linea 1 y la perpendicular de la linea 2 da 0 es que son paralelas.
        if (Vector3.Dot(HolisticMath.Perpendicular(l.v), v) == 0)
            return float.NaN;


        Vector3 C = (l.a - a);
        Vector3 uPerp = HolisticMath.Perpendicular(l.v);
        float t = Vector3.Dot(uPerp, C) / Vector3.Dot(uPerp, v);
        if (linetype.Equals(LineType.segment) && !(t >= 0 && t <= 1))
            return float.NaN;
        return t;
    }

    public Vector3 IntersectAtPoint(Line l)
    {
        float t =  IntersectAt(l);
        return Lerp(t);
    }





}
