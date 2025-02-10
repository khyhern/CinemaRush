using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorOpenSound : MonoBehaviour
{
    private bool isOpen = false; // Track door state
    private float openThreshold = 30f;

    void Update()
    {
        float doorAngle = transform.localEulerAngles.y; // Adjust axis if needed

        if (!isOpen && doorAngle > openThreshold && doorAngle < 180f)
        {
            SoundManager.Instance.PlayOneShot("door open");
            isOpen = true;
        }
        else if (isOpen && (doorAngle < openThreshold || doorAngle > 270f))
        {
            SoundManager.Instance.PlayOneShot("door close");
            isOpen = false;
        }
    }
}