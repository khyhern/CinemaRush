using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonVR : MonoBehaviour
{
    [Header("Button Settings")]
    public GameObject button;               // The visual button GameObject to move when pressed
    public UnityEvent onPress;              // Event to invoke on press
    public UnityEvent onRelease;            // Event to invoke on release

    [Header("References")]
    public CupDetector cupDetector;  // Reference to SocketDetector script
    public Transform socketPoint;           // The socket where the object should be attached
    public ParticleSystem effect;           // The effect to play during the 7 seconds
    public Collider targetCollider;         // The collider to turn on for 7 seconds

    [Header("Audio")]
    public AudioSource clickSoundEffect;    // Sound for button press
    public AudioSource SoundEffect;      // Sound to play AFTER the effect ends

    private bool isPressed;

    // Positions for button pressed and released
    private Vector3 pressedPosition = new Vector3(0, 0.003f, 0);
    private Vector3 releasedPosition = new Vector3(0, 0.015f, 0);

    private void Start()
    {
        isPressed = false;
        if (targetCollider != null)
            targetCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "VR_Hand_Yellow" && !isPressed)
        {
            if (cupDetector != null && cupDetector.IsCupInSocket()) // Check if NEWSodaCup is in socket
            {
                StartCoroutine(ButtonPressedRoutine());
            }
            else
            {
                Debug.Log("NEWSodaCup is not in the socket. Button will not activate.");
            }
        }
    }

    private IEnumerator ButtonPressedRoutine()
    {
        isPressed = true;

        // Move button to pressed position
        button.transform.localPosition = pressedPosition;

        // Play button press sound
        onPress.Invoke();
        if (clickSoundEffect != null)
            clickSoundEffect.Play();

        // Play the effect
        if (effect != null)
        {
            effect.Play();
            // Play sound effect 
            if (SoundEffect != null)
            {
                SoundEffect.Play();
            }
        }

        // Enable the referenced collider
        if (targetCollider != null)
        {
            targetCollider.enabled = true;
        }

        // Wait for 7 seconds while the effect plays
        yield return new WaitForSeconds(7f);

        // Stop the effect
        if (effect != null)
        {
            effect.Stop();
        }


        // Move button back to released position and disable collider
        button.transform.localPosition = releasedPosition;
        onRelease.Invoke();

        if (targetCollider != null)
        {
            targetCollider.enabled = false;
        }

        isPressed = false;
    }
}


/*
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
    public AudioSource SoundEffect;      // Sound to play AFTER the effect ends

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
            //if (socketPoint.childCount > 0)
            if (other.gameObject.name == "NEWSodaCup") // Check if NEWSodaCup collides

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


        if (effect != null)
        {
            effect.Play();
            // Play sound effect 
            if (SoundEffect != null)
            {
                SoundEffect.Play();
            }
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
*/