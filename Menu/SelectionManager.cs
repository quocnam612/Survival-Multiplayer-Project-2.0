public class SelectionManager : MonoBehaviourPunCallbacks
{
    private InteractableObject currentInteractable;
    private Ray ray;
    private RaycastHit hit;
    
    private void Awake()
    {
        ray = new Ray();
    }

    void Update()
    {
        ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, interaction_distance, ~LayerMask.GetMask("Ignore Raycast"))) 
        {
            var interactable = hit.transform.GetComponent<InteractableObject>();
            if (interactable != null && interactable != currentInteractable)
            {
                currentInteractable = interactable;
                UpdateInteractionText(interactable);
            }
        }
        else if (currentInteractable != null)
        {
            interaction_text.enabled = false;
            currentInteractable = null;
        }
    }
}
