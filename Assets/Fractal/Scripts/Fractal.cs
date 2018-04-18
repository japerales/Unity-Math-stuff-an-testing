using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {


    public Mesh mesh;
    public Material material;
    public int maxDepth;
    private int depth;
    public float childscale; //este valor no se guarda en los objetos hijo!!!
	// Use this for initialization
	void Start () {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;
        if (depth < maxDepth) { 
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this);

        }
    }

    //Initialize is called before start.
    private void Initialize(Fractal parent)
    {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childscale = parent.childscale;
        //si no cogemos el valor del padre, se perderá.
        transform.parent = parent.transform;
        /*por qué esto es así? El cubo mide 1, y nosotros tenemos que subir el cubo a 0.5 del padre (la mitad)
         + 0.5 del hijo multiplicado por la escala que le hayamos puesto. Por tanto todos los cubos van a estar
         a 0.75 de distancia de su padre. Lo que ocurre es que esto siempre se hace en función de la escala.
         Cada hijo está escalado *0.5 respecto de su padre por lo que la relación siempre se mantiene entre padre 
         e hijo*/
        transform.localPosition = new Vector3(0f, 0.5f + 0.5f * childscale, 0);
        transform.localScale = Vector3.one * childscale;
    }



    // Update is called once per frame
    void Update () {
		
	}
}
