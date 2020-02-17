using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CursorScript : MonoBehaviour
{
    public GameObject beltToggle;  // for toggling belt on/off
    public GameObject spawnerContainer; // for making item child when dropping
    private RubbishItem currentlyHolding;

    public GameObject leftBin;
    public GameObject rightBin;
    private float angle;
    
    private Animator animator;

    public bool windUpSlap { get; set; }
    public bool flick { get; set; }


    private AudioSource audioSource;
    public AudioClip RubbishPickUp;
    public AudioClip RubbishDrop;
    public AudioClip CrispPickUp;
    public AudioClip CrispDrop;

    void Awake()
    {
        windUpSlap = false;
        flick = false;
        angle = 45 * Time.deltaTime * 5;
        Cursor.visible = false;
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // move cursor image with mouse
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0;
        transform.position = cursorPos;

        // push button or pick up item
        if (Input.GetMouseButtonDown(0) && !currentlyHolding)
        {
            LeftClickInteract();
        }

        // drop held item
        if (Input.GetMouseButtonUp(0))
        {
            DropItem();
        }

        // fling item to nearest bin
        if (Input.GetKey(KeyCode.LeftShift))
        {
            AutoBin();
        }

        //if not swiping, return arm to neutral position
        if (transform.rotation.z != 0 && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D))
        {
            var angle = 45 * Time.deltaTime * 30;
            transform.rotation *= Quaternion.AngleAxis(angle, Vector3.forward);
            if (transform.rotation.z > 0)
            {
                transform.rotation = new Quaternion(
                    transform.rotation.x,
                    transform.rotation.y,
                    0,
                    transform.rotation.w);
            }
        }

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Sweeping", true);
        }
        else
        {
            animator.SetBool("Sweeping", false);
        }

        //if (Input.GetAxis("Mouse ScrollWheel") > 0)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("Flicking");
        }
    }

    // right click swipe
    void OnTriggerStay2D(Collider2D collidedWith)
    {
        if (collidedWith.gameObject.tag == "RubbishItem")
        {
            if (!collidedWith.gameObject.GetComponent<RubbishItem>().isBeingHeld)
            {
                if (flick)
                {
                    collidedWith.gameObject.GetComponent<RubbishItem>().Flick();
                }
                if (windUpSlap)
                {
                    collidedWith.gameObject.GetComponent<RubbishItem>().Slap();
                }
            }
        }
    }

    private void AutoBin()
    {
        GameObject go = GetTopRubbishItem();
        if (go)
        {
            if (go.tag == "RubbishItem")
            {
                go.GetComponent<RubbishItem>().AutoBin();
            }
        }
    }

    private void LeftClickInteract()
    {
        GameObject go = GetTopRubbishItem();
        if (go)
        {
            if (go.tag == "RedButton")
            {
                beltToggle.GetComponent<ToggleBelt>().ToggleConveyorBelt(false);
            }
            else if (go.tag == "GreenButton")
            {
                beltToggle.GetComponent<ToggleBelt>().ToggleConveyorBelt(true);
            }
            else if (go.tag == "RubbishItem")
            {
                animator.SetBool("Grabbing", true);
                currentlyHolding = go.GetComponent<RubbishItem>();

                if(currentlyHolding.isBadRubbish)
                {
                    audioSource.clip = CrispPickUp;
                }
                else
                {
                    audioSource.clip = RubbishPickUp;
                }

                audioSource.Play();
                currentlyHolding.HoldItem();
            }
        }
    }

    private void DropItem()
    {
        if (currentlyHolding)
        {
            animator.SetBool("Grabbing", false);
            currentlyHolding.transform.parent = spawnerContainer.transform;

            if (currentlyHolding.isBadRubbish)
            {
                audioSource.clip = CrispDrop;
            }
            else
            {
                audioSource.clip = RubbishDrop;
            }

            audioSource.Play();
            currentlyHolding.DropItem();
            currentlyHolding = null;
        }
    }

    
    // get rubbish item under mouse with highest sprite order
    private GameObject GetTopRubbishItem()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector3(0, 0, 1));
        if (hits.Length > 0)
        {
            GameObject topGameObject = null;
            for (int i = 0; i < hits.Length; i++)
            {
                GameObject gameObject = hits[i].collider.gameObject;
                if (gameObject.tag == "RubbishItem")
                {
                    if (!topGameObject)
                    {
                        topGameObject = gameObject;
                    }
                    if (gameObject.GetComponent<SpriteRenderer>().sortingOrder > topGameObject.GetComponent<SpriteRenderer>().sortingOrder)
                    {
                        topGameObject = gameObject;
                    }
                }
            }

            return topGameObject;
        }

        return null;
    }

    // get all rubbish items under mouse
    private List<GameObject> GetRubbishItems()
    {
        List<GameObject> rubbishItems = new List<GameObject>();

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector3(0, 0, 1));
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                GameObject gameObject = hits[i].collider.gameObject;
                if (gameObject.tag == "RubbishItem")
                {
                    rubbishItems.Add(gameObject);
                }
            }
        }

        return rubbishItems;
    }
}
