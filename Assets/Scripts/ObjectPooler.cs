using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject Object;
    public int initalObjects;
    List<GameObject> Objects;

    // Start is called before the first frame update
    void Start()
    {
        Objects = new List<GameObject>();

        for(int i = 0; i < initalObjects; i++)
        {
            GameObject obj = Instantiate(Object);
            obj.SetActive(false);
            Objects.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        for (int i = 0; i < Objects.Count; i++)
        {
            if(!Objects[i].activeInHierarchy)
            {
                Objects[i].SetActive(true);
                return Objects[i];
            }
        }

        GameObject newObject = Instantiate(Object);
        Objects.Add(newObject);
        return newObject;
    }
}
