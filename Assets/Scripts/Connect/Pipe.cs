using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public bool isInPlace;
    public bool isFilled;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite filledSprite;
    [SerializeField] private int[] correctRot;

    private int[] rot = { 0, 90, 180, 270 };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySprite;

        int rand = Random.Range(0, rot.Length);

        transform.eulerAngles = new Vector3(0, 0, rot[rand]);

        checkRot();
    }

    private void Update()
    {
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, currentRotation, 0.05f);
        if (isInPlace && isFilled)
        {
            spriteRenderer.sprite = filledSprite;
        }
        else
        {
            spriteRenderer.sprite = emptySprite;
        }
    }

    public void RotateSelf()
    {
        transform.Rotate(new Vector3(0, 0, 90));
    }

    public void checkRot()
    {
        if (correctRot.Length == 1)
        {
            if (transform.eulerAngles.z == correctRot[0] && !isInPlace)
            {
                isInPlace = true;
            }
            else
            {
                isInPlace = false;
            }
        }
        else if (correctRot.Length == 2)
        {
            if (transform.eulerAngles.z == correctRot[0] || transform.eulerAngles.z == correctRot[1] && !isInPlace)
            {
                isInPlace = true;
            }
            else
            {
                isInPlace = false;
            }
        }
    }
}