using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
    public int index = 0;
    public string tileName;
    public Vector3 targetPos;
    public Vector2 matrixPos;

    void Awake()
    {
        tileName = gameObject.name;
        targetPos = transform.position;
    }

    void Update()
    {
        // Moving tiles
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
    }
}
