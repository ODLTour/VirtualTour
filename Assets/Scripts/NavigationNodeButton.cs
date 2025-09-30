using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NavigationNodeButton : MonoBehaviour
{
    public NavigationNode targetNode;

    public VRVisit vrVisitManager; 

    private Button buttonComponent;

    void Start()
    {
        buttonComponent = GetComponent<Button>();
        if (buttonComponent != null && vrVisitManager != null)
        {
            buttonComponent.onClick.AddListener(HandleButtonClick);
        }
    }

    private void HandleButtonClick()
    {
        if (targetNode != null && vrVisitManager != null)
        {
            vrVisitManager.TeleportToNode(targetNode);
        }
    }
}
