using UnityEngine;

public class CookSausage : MonoBehaviour
{
    private bool isInPan = false; // Tracks if the sausage is in the frying pan
    private float cookTime = 0f; // Timer to track cooking time
    private bool isCooked = false; // Tracks if sausage is cooked
    private bool isBurnt = false; // Tracks if sausage is burnt

    [SerializeField] private Material cookedMaterial; // Reference to the cooked sausage material
    [SerializeField] private Material burntMaterial;  // Reference to the burnt sausage material
    [SerializeField] private float cookDuration = 5f; // Time needed to cook
    [SerializeField] private float burnDuration = 10f; // Time after which the sausage burns

    private FryingPan fryingPan; // Reference to the frying pan script

    public enum SausageState { Raw, Cooked, Burnt }
    public SausageState state; // Assign state in Inspector or change dynamically

    private void Start()
    {
        state = SausageState.Raw;
        // Find the frying pan in the scene and reference its FryingPan script
        fryingPan = FindObjectOfType<FryingPan>();
        if (fryingPan == null)
        {
            Debug.LogError("FryingPan script not found in the scene!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FryingPan"))
        {
            if (fryingPan != null)
            {
                fryingPan.AddSausage(gameObject); // Register with the pan

                if (isBurnt)
                {
                    fryingPan.AddBurntSausage(gameObject); // Notify the pan that this burnt sausage is in it
                }
                else if (fryingPan.IsBurnt)
                {
                    BurnSausage(); // If the pan is already burnt, the sausage burns immediately
                }
                else
                {
                    isInPan = true; // Start cooking logic if the pan is not burnt
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("FryingPan"))
        {
            if (fryingPan != null)
            {
                fryingPan.RemoveSausage(gameObject); // Notify the pan that this sausage left
            }
            isInPan = false;
        }
    }

    private void Update()
    {
        if (isInPan && !isBurnt && fryingPan != null && !fryingPan.IsBurnt)
        {
            cookTime += Time.deltaTime;

            // Cook the sausage after cookDuration
            if (!isCooked && cookTime >= cookDuration)
            {
                CookSausageFully();
                SoundManager.Instance.PlayOneShot("sausage done");
            }

            // Burn the sausage after burnDuration
            if (cookTime >= burnDuration)
            {
                BurnSausage();
                SoundManager.Instance.PlayOneShot("burn");
            }
        }
    }

    private void CookSausageFully()
    {
        if (!isCooked && !isBurnt) // Only cook if not burnt
        {
            isCooked = true;
            state = SausageState.Cooked;
            Debug.Log("Sausage is cooked!");

            // Change the sausage's material to the cooked material
            if (cookedMaterial != null)
            {
                GetComponent<Renderer>().material = cookedMaterial;
            }
            else
            {
                Debug.LogWarning("Cooked material is not assigned!");
            }

            // Change the tag to "Food" when cooked
            gameObject.tag = "Food";
        }
    }

    public void BurnSausage()
    {
        if (isBurnt) return; // Prevent duplicate burning logic

        isBurnt = true;
        isCooked = false;
        state = SausageState.Burnt;
        Debug.Log("Sausage is burnt!");

        // Change the sausage's material to the burnt sausage material
        if (burntMaterial != null)
        {
            GetComponent<Renderer>().material = burntMaterial;
        }
        else
        {
            Debug.LogWarning("Burnt material is not assigned!");
        }

        // Change the tag back to "Untagged" when burnt
        gameObject.tag = "Untagged";

        // Notify the frying pan that this burnt sausage is now in it
        if (fryingPan != null)
        {
            fryingPan.AddBurntSausage(gameObject);
        }
    }

    public void OnPanReset()
    {
        if (isBurnt && fryingPan != null)
        {
            fryingPan.AddBurntSausage(gameObject); // Re-register the burnt sausage after pan reset
        }
    }
}
