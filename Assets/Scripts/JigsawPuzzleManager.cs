using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class JigsawPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzlePoints;

    Dictionary<string, PuzzleData> puzzleData;
    Dictionary<Vector2, string> initialMapPosData;
    Dictionary<Vector2, string> mapPosData;

    private GapData puzzleGap;

    private int puzzleIndexDestroyed = 0;

    [Header("Debug Properties")]
    public TextMeshProUGUI initialData;
    public TextMeshProUGUI mapData;

    void Start()
    {
        puzzleGap = new GapData();

        StoreInitialData();

        DestroyRandomTile();

        PrintInitialData();
        PrintMapData();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                if (CheckIfCanMove(hitObject))
                {
                    Vector3 tempWPos = new Vector3(0, 0, 0);
                    Vector2 tempMatPos = new Vector2(0, 0);

                    var pData = new PuzzleData();

                    //Move pressed puzzle
                    hitObject.transform.position = puzzleGap.worldPos;

                    if (puzzleData.TryGetValue(hitObject.name, out PuzzleData data))
                    { 
                        // Store new gap data
                        tempWPos = data.worldPos;
                        tempMatPos = data.matrixPos;                        
                    }

                    //Store new data for pressed puzzle
                    pData.worldPos = puzzleGap.worldPos;
                    pData.matrixPos = puzzleGap.matrixPos;
                    puzzleData[hitObject.name] = pData;

                    //Update mapPosData
                    mapPosData[puzzleGap.matrixPos] = hitObject.name;

                    UpdatePuzzleGap(tempWPos, tempMatPos);

                    CheckIfWin();
                }
            }
        }
    }

    private void UpdatePuzzleGap(Vector3 worldPos, Vector2 matrixPos)
    {
        puzzleGap.worldPos = worldPos;
        puzzleGap.matrixPos = matrixPos;

        mapPosData[matrixPos] = "gap";

        PrintInitialData();
        PrintMapData();
    }

    private void DestroyRandomTile()
    {
        puzzleIndexDestroyed = Random.Range(1, puzzlePoints.Length);

        UpdatePuzzleGap(puzzlePoints[puzzleIndexDestroyed - 1].transform.position, puzzleData[puzzlePoints[puzzleIndexDestroyed - 1].name].matrixPos);

        Destroy(puzzlePoints[puzzleIndexDestroyed - 1]);

        initialMapPosData[puzzleData[puzzlePoints[puzzleIndexDestroyed - 1].name].matrixPos] = "gap";
    }

    private bool CheckIfCanMove(GameObject obj)
    {
        bool check = false;

        float xDiff = puzzleGap.worldPos.x - obj.transform.position.x;
        float yDiff = puzzleGap.worldPos.y - obj.transform.position.y;

        if (Mathf.Abs(xDiff) == 2.25 && yDiff == 0)
        {
            check = true;
        }
        else if (Mathf.Abs(yDiff) == 2.25 && xDiff == 0)
        {
            check = true;
        }

        return check;
    }

    private void CheckIfWin()
    {
        Vector2 checkingPos;

        bool isWin = false;
        bool checkBreak = false;

        for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                checkingPos = new Vector2(i, j);

                if (initialMapPosData.TryGetValue(checkingPos, out string initialValue)
                    && mapPosData.TryGetValue(checkingPos, out string currentValue))
                {
                    if (currentValue == initialValue)
                    {
                        Debug.Log(checkingPos + ": " + currentValue + " = " + initialValue);
                        isWin = true;
                        continue;
                    }
                    else
                    {
                        Debug.Log("Not the same");
                        checkBreak = true;
                        isWin = false;
                        break;
                    }
                }
                else
                {
                    Debug.Log("Not found");
                    isWin = false;
                    break;
                }
            }
            if (checkBreak)
            {
                break;
            }
        }

        if (isWin)
        {
            Debug.Log("Win");
        }
    }

    private void StoreInitialData()
    {
        puzzleData = new Dictionary<string, PuzzleData>();
        initialMapPosData = new Dictionary<Vector2, string>();
        mapPosData = new Dictionary<Vector2, string>();

        int count = 1;

        for (int i = 1; i <= 4; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                var data = new PuzzleData();
                data.worldPos = puzzlePoints[count - 1].transform.position;
                data.matrixPos = new Vector2(i, j);

                puzzleData.Add(count.ToString(), data);

                initialMapPosData.Add(new Vector2(i, j), count.ToString());
                mapPosData.Add(new Vector2(i, j), count.ToString());

                count++;
            }
        }
    }

    private void PrintInitialData()
    {
        initialData.text = "";

        var vec2 = initialMapPosData.Keys.ToArray();
        var str = initialMapPosData.Values.ToArray();

        for (int i = 0; i < vec2.Length; i++)
        {
            initialData.text += vec2[i].ToString() + " : " + str[i] + "\n";
        }
    }
    private void PrintMapData()
    {
        mapData.text = "";

        var vec2 = mapPosData.Keys.ToArray();
        var str = mapPosData.Values.ToArray();

        for (int i = 0; i < vec2.Length; i++)
        {
            mapData.text += vec2[i].ToString() + " : " + str[i] + "\n";
        }
    }
}

public class GapData
{
    public Vector3 worldPos;
    public Vector2 matrixPos;
}

public class PuzzleData
{
    public Vector3 worldPos;
    public Vector2 matrixPos;
}