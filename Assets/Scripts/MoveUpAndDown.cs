using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    float speed = 2f;
    float height = 0.25f;
    public float offset = 4.25f;
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        float newY = Mathf.Sin(Time.time * speed);
        transform.localPosition = new Vector3(pos.x, newY + offset, pos.z) * height;
    }
}
