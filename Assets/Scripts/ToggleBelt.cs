using UnityEngine;

public class ToggleBelt : MonoBehaviour
{
    public bool On;
    public GameObject spawner;
    public GameObject spriteMove;
    public GameObject beltAudio;


    public void Awake()
    {
        ToggleConveyorBelt(On);
    }

    public void ToggleConveyorBelt(bool toggle)
    {
        GameManager.Instance.On = toggle;
        beltAudio.GetComponent<BeltAudio>().ToggleConveyorBelt(toggle);
    }
}
