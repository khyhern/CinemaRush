using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;  // Needed for XRGrabInteractable

public class ButtonVR : MonoBehaviour
{
    [Header("Button Settings")]
    public GameObject button;               // The visual button GameObject to move when pressed
    public UnityEvent onPress;              // Event to invoke on press
    public UnityEvent onRelease;            // Event to invoke on release

    [Header("References")]
    public Transform socketPoint;           // The socket where the object should be attached
    public ParticleSystem effect;           // The effect to play during the 7 seconds
    public Collider targetCollider;         // The collider to turn on for 7 seconds

    private AudioSource clickSoundEffect;   // Audio source for the click sound
    private bool isPressed;

    // Positions for button pressed and released (adjust as needed)
    private Vector3 pressedPosition = new Vector3(0, 0.003f, 0);
    private Vector3 releasedPosition = new Vector3(0, 0.015f, 0);

    private void Start()
    {
        clickSoundEffect = GetComponent<AudioSource>();
        isPressed = false;

        // Optionally ensure the target collider starts off
        if (targetCollider != null)
            targetCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only allow press if not already pressed.
        if (!isPressed)
        {
            // Check if there is an object attached to the socket
            if (socketPoint.childCount > 0)
            {
                // Proceed with the sequence only if an object is attached.
                StartCoroutine(ButtonPressedRoutine());
            }
            else
            {
                // No attached object; do nothing.
                Debug.Log("No object attached to the socket. Button press will not proceed.");
            }
        }
    }

    private IEnumerator ButtonPressedRoutine()
    {
        isPressed = true;

        // Move button to pressed position
        button.transform.localPosition = pressedPosition;

        // Invoke onPress event and play click sound
        onPress.Invoke();
        if (clickSoundEffect != null)
            clickSoundEffect.Play();


        // Play the effect if assigned
        if (effect != null)
        {
            effect.Play();
        }

        // Enable the referenced collider (e.g., a capsule collider on the mesh)
        if (targetCollider != null)
        {
            targetCollider.enabled = true;
        }

        // Wait for 7 seconds while the effect plays and the object stays locked.
        yield return new WaitForSeconds(7f);

        // After 7 seconds, stop the effect (if needed)
        if (effect != null)
        {
            effect.Stop();
        }

        // Release the button: move to released position, disable collider, and re-enable grabbing
        button.transform.localPosition = releasedPosition;
        onRelease.Invoke();

        if (targetCollider != null)
        {
            targetCollider.enabled = false;
        }

        // Reset the button state
        isPressed = false;

    }
}


/*
public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress; 
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource clickSoundEffect;
    bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        clickSoundEffect = GetComponent<AudioSource>();
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            clickSoundEffect.Play();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.015f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    // Test. Can delete after or change to other fucntions like dispensing soda. 
    public void SpawnSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        sphere.transform.localPosition = new Vector3(0,1,2);
        sphere.AddComponent<Rigidbody>();
    }
}
*/