using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltSpriteMove : MonoBehaviour
{
    public GameObject beltSpritesContainer;
    public GameObject beltBellySprite;
    private GameObject beltBellyDuplicate;


    private float beltEdgeRight;
    private float spawnPointX;

    void Awake()
    {
        SpriteRenderer beltSprite = beltBellySprite.GetComponent<SpriteRenderer>();

        // spawn dupe belt
        spawnPointX = -beltSprite.bounds.extents.x * 2 + 1;
        beltBellyDuplicate = Instantiate(beltBellySprite, new Vector3(spawnPointX, beltBellySprite.transform.position.y, 0), Quaternion.identity, beltSpritesContainer.transform);

        // distance from centre to right of belt
        beltEdgeRight = beltSprite.bounds.extents.x;
        GameManager.Instance.BeltSpriteBoundY = beltSprite.bounds.extents.y;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.On)
        {
            MoveBelt(beltBellySprite);
            MoveBelt(beltBellyDuplicate);
        }
    }

    private void MoveBelt(GameObject belt)
    {
        belt.transform.position = new Vector3(belt.transform.position.x + GameManager.Instance.beltSpeed, belt.transform.position.y, 0);

        if ((belt.transform.position.x - beltEdgeRight) > beltEdgeRight)
        {
            belt.transform.position = new Vector3(spawnPointX, belt.transform.position.y, 0);
        }
    }
}
