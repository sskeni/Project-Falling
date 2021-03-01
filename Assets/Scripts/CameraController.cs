using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform FlyingPosition;
    public Transform FallingPosition;
    public float camMoveSpeed;
    private float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        yOffset = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.getIsFlying())
            yOffset = Mathf.Lerp(yOffset, FlyingPosition.position.y, camMoveSpeed * Time.deltaTime);
        else
            yOffset = Mathf.Lerp(yOffset, FallingPosition.position.y, camMoveSpeed * Time.deltaTime);

        if (!GameManager.instance.GameOver())
            transform.position = new Vector3(transform.position.x, PlayerController.instance.gameObject.transform.position.y + yOffset, transform.position.z);
    }
}
