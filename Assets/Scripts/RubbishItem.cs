using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubbishItem : MonoBehaviour
{
    public bool isBeingHeld { get; protected set; }
    public bool isBeingAutoBinned { get; protected set; }
    public bool isShrinking { get; protected set; }

    public bool isBadRubbish { get; set; }

    private bool hasBeenSlapped;
    private float slapYDirection;
    private bool hasBeenFlicked;
    private float flickYDirection;
    private float flickXDirection;

    float elapsed = 0f;
    float slapTime = 0.5f;
    float flickTime = 0.25f;
    
    private SpriteRenderer spriteRender;
    private GameObject indicator;

    private void Start()
    {
        isBeingHeld = false;
        isBeingAutoBinned = false;
        isShrinking = false;

        hasBeenSlapped = false;
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        isBadRubbish = false;
    }

    void FixedUpdate()
    {
        if (isBadRubbish)
        {
            transform.GetChild(0).gameObject.transform.Rotate(0, 0, 10);
        }

        // move if slapped
        if (hasBeenSlapped)
        {
            MoveWithSlap();
        }

        // move if flicked
        else if (hasBeenFlicked)
        {
            MoveWithFlick();
        }

        // shrink size and destroy
        else if (isShrinking)
        {
            ShrinkAndDestroy();
        }

        // move if shift throw
        else if (isBeingAutoBinned)
        {
            MoveTowardsBin();
        }

        // move along con belt
        else if (!isBeingHeld && GameManager.Instance.On)
        {
            MoveAlongConveyorBelt();
        }

        // destory objects out of view
        if (transform.position.x > GameManager.Instance.edgeOfScreen)
        {
            Destroy(gameObject);
        }

        // parked code for getting direction relative to mouse
        // direction = (GetMousePosition() - previousPosition).normalized;
    }

    // Render indicator below 
    void OnMouseEnter()
    {
        if (isBadRubbish)
        {
            var go = transform.GetChild(0).gameObject;
            go.GetComponent<SpriteRenderer>().sortingOrder = spriteRender.sortingOrder - 1;
        }
    }

    void OnMouseExit()
    {
        if (isBadRubbish)
        {
            var go = transform.GetChild(0).gameObject;
            go.GetComponent<SpriteRenderer>().sortingOrder = 201;
        }
    }

    #region Input

    private void MoveAlongConveyorBelt()
    {
        if (IsOnConveyorBelt())
        {
            transform.position = new Vector3(
                transform.transform.position.x + GameManager.Instance.beltSpeed,
                transform.transform.position.y,
                0);
        }
    }

    private void MoveWithSlap()
    {
        elapsed += Time.deltaTime;
        if (elapsed <= slapTime)
        {
            // bounce off barrier
            if (!IsOnConveyorBelt())
            {
                slapYDirection = -slapYDirection * 1.5f;
            }

            // move to new position
            transform.position = new Vector3(
                    transform.transform.position.x - GameManager.Instance.slapSpeed,
                    transform.transform.position.y + slapYDirection,
                    0);
        }
        else
        {
            hasBeenSlapped = false;
            elapsed = 0f;
        }        
    }

    private void MoveWithFlick()
    {
        elapsed += Time.deltaTime;
        if (elapsed <= flickTime)
        {
            // bounce off barrier
            if (!IsOnConveyorBelt())
            {
                flickYDirection = -flickYDirection * 1.5f;
            }

            // move to new position
            transform.position = new Vector3(
                    transform.transform.position.x + flickXDirection,
                    transform.transform.position.y + flickYDirection,
                    0);
        }
        else
        {
            hasBeenFlicked= false;
            elapsed = 0f;
        }        
    }

    private void ShrinkAndDestroy()
    {
        if (transform.localScale.x < 0)
        {

            Destroy(gameObject);
        }
        else
        {
            float shrinkSpeed = 0.75f * Time.deltaTime;

            transform.localScale = new Vector3(
                transform.localScale.x - shrinkSpeed,
                transform.localScale.y - shrinkSpeed,
                0);
            transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + GameManager.Instance.fallIntoBinSpeed, Vector3.forward);

            MoveTowardsBin();
        }
    }

    private void MoveTowardsBin()
    {
        if (transform.position.x < 0)
        {
            transform.position = Vector2.MoveTowards(
                transform.transform.position,
                GameManager.Instance.leftBinPosition,
                GameManager.Instance.fallIntoBinSpeed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.transform.position,
                GameManager.Instance.rightBinPosition,
                GameManager.Instance.fallIntoBinSpeed);
        }

        transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + GameManager.Instance.throwSpeed, Vector3.forward);
    }

    #endregion

    #region Set State
    public void HoldItem()
    {
        transform.SetParent(GameManager.Instance.CursorTransform, true);
        isBeingHeld = true;
        spriteRender.sortingOrder = GameManager.Instance.GetTopSpriteLater();
    }

    public void Slap()
    {
        hasBeenSlapped = true;
        slapYDirection = RandomYAxis();
    }

    public void Flick()
    {
        hasBeenFlicked = true;
        flickYDirection = Random.Range(0.05f, 0.1f);
        flickXDirection = Random.Range(-0.03f, 0.03f);
    }

    public void AutoBin()
    {
        isBeingAutoBinned = true;
        isBeingHeld = false;
    }

    public void DropItem()
    {
        isBeingHeld = false;
        if (isBadRubbish)
        {
            spriteRender.sortingOrder = transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
    }

    public void Shrink()
    {
        isShrinking = true;
        if (isBadRubbish)
        {
            GameManager.Instance.dialogueManager.PraisePlayer();
        }
        else
        {
            GameManager.Instance.dialogueManager.BeratePlayer();
        }
    }

    #endregion


    #region Helper Methods
    private Vector2 GetMousePosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = GameManager.Instance.DistanceFromCamera;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private bool IsOnConveyorBelt()
    {
        return transform.position.y <= GameManager.Instance.BarrierTopY
            && transform.position.y >= GameManager.Instance.BarrierBottomY;
    }

    private float RandomYAxis()
    {
        float randomY = 0;

        while (randomY == 0)
        {
            randomY = Random.Range(-0.3f, 0.3f);
        }

        return randomY/3;
    }

    #endregion
}
