using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public Transform ObjectGenerationPoint;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public bool generateDownward;
    public ObjectPooler pool;

    // Update is called once per frame
    void Update()
    {
        if (!generateDownward && transform.parent != null && PlayerController.instance.getIsFlying())
            transform.SetParent(null);
        if(generateDownward)
        {
            if (transform.position.y > ObjectGenerationPoint.position.y)
            {
                float xPos = Random.Range(minX, maxX);
                float yPos = Random.Range(minY, maxY);
                transform.position = new Vector2(xPos, transform.position.y - yPos);

                GameObject obj = pool.GetObject();
                obj.transform.position = transform.position;
            }
        }
        else
        {
            if (transform.position.y < ObjectGenerationPoint.position.y)
            {
                float xPos = Random.Range(minX, maxX);
                float yPos = Random.Range(minY, maxY);
                transform.position = new Vector2(xPos, transform.position.y + yPos);

                GameObject obj = pool.GetObject();
                obj.transform.position = transform.position;
            }
        }
    }
}
