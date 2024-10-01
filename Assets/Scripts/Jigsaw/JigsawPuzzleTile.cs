using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawPuzzleTile : MonoBehaviour
{
    public int index;
    public Vector3 targetPos;
    public bool isInPlace;

    private BoxCollider2D boxCollider;

    void Awake()
    {
        targetPos = transform.position;

        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.05f);

        if(isInPlace)
        {
            boxCollider.enabled = false;
        }
    }
}
