using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawPuzzleManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

[System.Serializable]
public class PuzzleData
{
    public GameObject puzzle;
    public BoxCollider2D puzzleCollider;
    public Vector3 currentPosition;
}
