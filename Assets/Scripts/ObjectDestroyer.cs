using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public string destructionPointName;
    public bool generateDownward;
    private GameObject DestructionPoint;

    // Start is called before the first frame update
    void Start()
    {
        DestructionPoint = GameObject.Find(destructionPointName);
    }

    // Update is called once per frame
    void Update()
    {
        if (generateDownward && transform.position.y > DestructionPoint.transform.position.y)
        {
            gameObject.SetActive(false);
        }
        else if (!generateDownward && transform.position.y < DestructionPoint.transform.position.y)
        {
            gameObject.SetActive(false);
        }
    }
}
