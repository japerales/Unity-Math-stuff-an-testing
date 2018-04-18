using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    public Transform PointPrefab;
    [Range(10,100)]
    public int Resolution;
    Transform[] points;

    const float PI = Mathf.PI;

    static GraphFunction[] functions = { SineFunction, Sine2DFunction, MultiSineFunction, MultiSine2DFunction, Ripple, Cylinder, Sphere };
    public GraphFunctionName function;
    private void Awake()
    {

        float steps = 2f / Resolution;
        Vector3 scale = Vector3.one * steps;
        Vector3 position;
        position.z = 0;
        position.y = 0;
        points = new Transform[Resolution * Resolution];
        
        //Ahora mismo, por defecto, todos los puntos se quedan en 0,0,0
        for (int i=0; i<points.Length;i++){

                Transform point = Instantiate(PointPrefab);
                point.SetParent(transform,false);
                point.localScale = scale;
                points[i] = point;
            
        }
       
    }

    // Use this for initialization
    void Start () {
		
	}


    void Update()
    {
        float t = Time.time;
        GraphFunction f = functions[(int)function];
        float steps = 2f / Resolution;

        for (int i = 0, z = 0; z < Resolution; z++)
        {
            //(x+0.5f) = con esto colocamos un cubo inmediatamente a la derecha de la posición 0.
            // -1: colocamos dicho objeto a la izquierda que es por donde empezaremos.
            float v = ((z + 0.5f) * steps - 1f);
            for (int x = 0; x < Resolution; x++, i++)
            {
                float u = ((x + 0.5f) * steps - 1f);
                points[i].localPosition = f(u, v, t);
            }
        }
    }
    // Viejo update: solo servia mientras usabamos x and z eran valores constantes (cada cubo no cambiaba ni x ni z
    //En el nuevo update y las nuevas funciones van a cambiar tambien la x y la z.
    /*void Update()
    {
        float t = Time.time;

        GraphFunction f = functions[(int)function];
        //we only need to change y. x and z remains.
        for (int i = 0; i < points.Length; i++)
        {
        
            Vector3 position = points[i].localPosition;
            position = f(position.x, position.z, t);
            points[i].localPosition = position;
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(-100, 0, 0), new Vector3(100, 0, 0));
        Gizmos.DrawLine(new Vector3(0, -100, 0), new Vector3(0, 100, 0));
    }

    public static Vector3 SineFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.z = z;
        p.y = Mathf.Sin(PI * ( x+ t));
        return p;
    }

    public static Vector3 Sine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        //return Mathf.Sin(PI * (x + z + t)); //en este caso sumamos x con z. Por ejemplo, el seno de un cubo x=0.1 y z=0.1 es sin(0.2).
        p.x = x;
        p.z = z;
        float y = Mathf.Sin(PI * (x + t));
        y += Mathf.Sin(PI * (z + t));
        y *= 0.5f; //como usamos dos senos, la y va a superar sus limites de -1,1 a -2,2 por eso el *0.5
        p.y = y;
        return p;
    }


    public static Vector3 MultiSineFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.z = z;
        float y = Mathf.Sin(PI * (x + t));
        y += (Mathf.Sin(2f * PI * (x + 2f* t)) / 2f); //al sumarlo, inyectamos un nuevo seno del doble de frecuencia y dividimos su altura en 2
        //si multiplicamos el t de la función, ya no irán coordinados los senos.
        y *= 2f/3f; //we divide the function by 1.5 to reduce top/bottom to range 1,-1
        p.y = y;
        return p;
    }

    public static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        /*Let's create a 2D variant for the multi-sine function as well. In this case, we'll again use a single main wave but with two secondary waves, 
         * one per dimension, so we get a function of the form f(x,z,t)=M+Sx+Sz, where M stands for the main wave, Sx represents the secondary wave based on x, and Sz is the secondary wave based on z.
       We'll use M=sin(π(x+z+t2)) , so the main wave is a slow-moving diagonal wave. The first secondary wave is Sx=sin(π(x+t)), so it's a normal wave along X. And the third wave is Sz=sin(2π(z+2t)), which is double-frequency and fast-moving along Z.
       We'll make the main wave M big, four times the amplitude of Sx. Because Sz has double the frequency and speed of the other secondary wave, we'll give it half the amplitude. 
       This leads to the function f(x,z,t)=4M+Sx+sz2, which has to be divided by 5.5 to normalize it to the −1–1 range. Create a MultiSine2DFunction method for this.*/
        Vector3 p;
        p.x = x;
        p.z = z;
        float y = 4f * Mathf.Sin(PI * (x + z + t* 0.5f)); //la onda principal, x4 de alto (luego dividimos) y a mitad de velocidad. Amplitud = 4
        y += Mathf.Sin(PI * (x + t)); //le sumamos una secundaria que es un sin normal. Amplitud = 1
        y += Mathf.Sin(2f * PI * (z + 2f * t)) * 0.5f;  //y otra secundaria más que tiene el doble de frecuencia y el doble de velocidad y dividimos el tamaño por la mitad. Amplitud = 0.5
        y *= 1f/5.5f; //La suma de amplitudes es 5.5f
        p.y = y;
        return p;
    }

    public static Vector3 Ripple(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.z = z;
        //para la distancia usamos el teorema de pitagoras.
        float d = Mathf.Sqrt(x * x + z * z);
        float y = Mathf.Sin(4f*PI*d - t*2); //Para el seno simplemente multiplicamos la distancia * PI y *4, de esta forma tenemos 2 senos completos por cada lado y LE RESTAMOS el tiempo, con esto conseguimos que el seno vaya en al revés.
        y/= 1 + 10f * d; //finalmente esto es y / 1+10f*d, dicho de otro modo, hacemos que cuanto más lejos sea la distancia, más pequeña es la amplitud de la ola. Si no le sumamos 1, 10f*d en distancia 0 sería 0 e y/0 daría error.
                         //En el origen la división será y/1 que es y.
        p.y = y;
        return p;
    }

    //u and v are range values from -1 to 1
    public static Vector3 Cylinder(float u, float v, float t)
    {
        //el radio puede hacer que creemos formas diferentes, como una estrella.
        //float r = 1f+ Mathf.Sin(6f * PI * u) * 0.2f;
        //también podemos usar v si queremos hacer formas verticales
        //float r = 1f + Mathf.Sin(2f * PI * v) * 0.2f;
        //o podemos añadirle t al radio:
        float r = (0.8f + Mathf.Sin(PI * (6f * u + 2f * v + t)) * 0.2f );

        Vector3 p;
        /*xz reciben los valores del rango de u (-1,1). Con solo u nos basta para hacer un circulo unitario ya que
         recorre todo los valores de dicho rango. El seno y el coseno completos hacen falta para el circulo y por tanto
         hay que multiplicar por PI, que recorre toda la función.
         */
        p.x = r* Mathf.Cos(PI * u);
        p.y = v;
        p.z = r* Mathf.Sin(PI * u);

        return p;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {


        float r = Mathf.Cos(PI * 0.5f * v);
        Vector3 p;
        p.x = r * Mathf.Cos(PI * u);
        p.y = Mathf.Sin(PI * 0.5f * v);
        p.z = r * Mathf.Sin(PI * u);

        return p;
    }


}

