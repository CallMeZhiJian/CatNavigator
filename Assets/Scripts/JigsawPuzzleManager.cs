using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class JigsawPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzlePoints;

    Dictionary<string, Vector2> posData;
    Dictionary<Vector2, string> initialMapPosData;
    Dictionary<Vector2, string> mapPosData;

    private GapData puzzleGap;
    
    private int puzzleIndexDestroyed = 0; 
    
    void Start()
    {
        puzzleGap = new GapData();

        StoreInitialData();

        puzzleIndexDestroyed = Random.Range(1, puzzlePoints.Length);

        UpdatePuzzleGap(puzzlePoints[puzzleIndexDestroyed - 1].transform.position, posData[puzzleIndexDestroyed.ToString()]);

        Destroy(puzzlePoints[puzzleIndexDestroyed - 1]);
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                if (CheckIfCanMove(hitObject))
                {
                    Vector3 tempWPos = hitObject.transform.position;
                    Vector2 tempMatPos = new Vector2(0, 0);

                    hitObject.transform.position = puzzleGap.worldPos;

                    mapPosData[puzzleGap.matrixPos] = hitObject.name;

                    if (posData.TryGetValue(hitObject.name, out Vector2 value))
                    {
                        tempMatPos = value;
                    }

                    mapPosData[tempMatPos] = "0";

                    UpdatePuzzleGap(tempWPos, tempMatPos);

                    CheckIfWin();
                } 
            }

            foreach(KeyValuePair<Vector2, string> pos in initialMapPosData)
            {
                Debug.Log(pos.Key);
                Debug.Log(pos.Value);
            }
        }
    }

    private void UpdatePuzzleGap(Vector3 worldPos, Vector2 matrixPos)
    {
        puzzleGap.worldPos = worldPos;
        puzzleGap.matrixPos = matrixPos;
    }

    private bool CheckIfCanMove(GameObject obj)
    {
        bool check = false;

        float xDiff = puzzleGap.worldPos.x - obj.transform.position.x;
        float yDiff = puzzleGap.worldPos.y - obj.transform.position.y;

        if(Mathf.Abs(xDiff) == 2.25 && yDiff == 0)
        {
            check = true;
        }
        else if(Mathf.Abs(yDiff) == 2.25 && xDiff == 0)
        {
            check = true;
        }

        return check;
    }

    private void CheckIfWin()
    {
        int count = 1;
        Vector2 checkingPos;

        for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                checkingPos = new Vector2(i, j);

                if (count == puzzleIndexDestroyed)
                {
                    count++;
                    continue;
                }

                if(mapPosData.TryGetValue(checkingPos, out string currentValue))
                {
                    Debug.Log(currentValue + " = " + initialMapPosData[checkingPos]);
                    if (currentValue == initialMapPosData[checkingPos])
                    {
                        count++;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        if(count == puzzlePoints.Length)
        {
            Debug.Log("Win");
        }
    }

    private void StoreInitialData()
    {
        posData = new Dictionary<string, Vector2>();
        initialMapPosData = new Dictionary<Vector2, string>();
        mapPosData = new Dictionary<Vector2, string>();

        int count = 1;

        for(int i = 1; i <= 4; i++)
        {
            for(int j = 1; j <= 4; j++)
            {
                posData.Add(count.ToString(), new Vector2(i, j));
                initialMapPosData.Add(new Vector2(i, j), count.ToString());
                count++;
            }
        }

        mapPosData = initialMapPosData;
    }
}

public class GapData
{
    public Vector3 worldPos;
    public Vector2 matrixPos;
}