using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectPoolScript : MonoBehaviour {

    //referencia a si mismo (ok...)
    public static NewObjectPoolScript current;

    public GameObject pooledObject;
    public int pooledAmount = 20;
    //indica si la pool crece o no
    public bool willGrow = true;

    //con una sola lista nos vale, la busqueda es muy rapida.
    //tener dos listas, una con activaciones y otras con desactivaciones es lento
    List<GameObject> pooledObjects;

	void Start () {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
	}

    private void Awake()
    {
        current = this;
    }

    public GameObject getPooledObject()
    {
        //esta busqueda se puede hacer con una expresion LINQ
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                /*MUCHAS SOLUCIONES SACAN EL OBJETO DE LA COLECCION Y LO VUELVEN A METER LUEGO.
                 ESTO NO ES NECESARIO Y ES COSTOSO. Por otro lado, el objeto que hace la request
                 es el que se encarga de activar el objeto*/
                return pooledObjects[i];
            }
        }

        //si queremos crecer, añadimos un nuevo elemento a la lista.
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj; //la referencia de la lista es la misma que esta.
        }

        return null;

    }
}
