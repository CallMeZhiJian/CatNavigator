using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawManager : MonoBehaviour
{
    [SerializeField] private Transform[] pointTransform;   
    private PointData[] pointDatas;

    [SerializeField] private JigsawPuzzleTile[] jigsawTiles;

    private void Start()
    {
        for (int i = 0; i < pointTransform.Length; i++) 
        {
            pointDatas[i].index = i;
            pointDatas[i].position = pointTransform[i].position;
        }

        for(int i = 0;i < jigsawTiles.Length; i++)
        {
            jigsawTiles[i].index = i;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}

public class PointData
{
    public int index;
    public Vector3 position;
}
