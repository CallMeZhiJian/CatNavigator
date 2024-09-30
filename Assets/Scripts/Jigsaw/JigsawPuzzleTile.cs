using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawPuzzleTile : MonoBehaviour
{
    public int index;
    public Vector3 targetPos;

    void Start()
    {
        targetPos = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);
    }
}
