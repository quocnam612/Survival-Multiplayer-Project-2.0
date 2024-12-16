public class SelectionManager : MonoBehaviourPunCallbacks
{
    private InteractableObject currentInteractable;
    private Ray ray;
    private RaycastHit hit;
    private Camera playerCamera;
    private Text interaction_text;
    private InteractableObject hitInteractable;
    private Vector3 lastMousePosition;

    private void Awake()
    {
        ray = new Ray();
        playerCamera = GetComponent<Camera>();
        interaction_text = GetComponent<Text>();
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (lastMousePosition != Input.mousePosition)
        {
            lastMousePosition = Input.mousePosition;
            ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, interaction_distance, ~LayerMask.GetMask("Ignore Raycast"))) 
            {
                hitInteractable = hit.transform.GetComponent<InteractableObject>();
                if (hitInteractable != null && hitInteractable != currentInteractable)
                {
                    currentInteractable = hitInteractable;
                    UpdateInteractionText(hitInteractable);
                }
            }
            else if (currentInteractable != null)
            {
                interaction_text.enabled = false;
                currentInteractable = null;
            }
        }
    }
}
