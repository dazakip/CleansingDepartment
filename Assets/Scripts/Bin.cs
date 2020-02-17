using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    // if rubbish item collides, shrink and delete
    void OnTriggerStay2D(Collider2D collidedWith)
    {
        if (collidedWith.gameObject.tag == "RubbishItem")
        {
            if (!collidedWith.gameObject.GetComponent<RubbishItem>().isBeingHeld &&
                !collidedWith.gameObject.GetComponent<RubbishItem>().isShrinking)
            {
                collidedWith.gameObject.GetComponent<RubbishItem>().Shrink();
            }
        }
    }
}
