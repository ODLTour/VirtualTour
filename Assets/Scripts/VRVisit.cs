using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;
using System;

[RequireComponent(typeof(TeleportationProvider))]
public class VRVisit : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor; 

    [SerializeField] private MeshRenderer sphereRenderer;
    [SerializeField] private GameObject mineModel;
    [SerializeField] private Camera mainCam;
    [SerializeField] private List<NavigationNode> navigationNodes;
    [SerializeField] private List<Vector3> navigationNodesPos;
    //[SerializeField] private List<TextMeshProUGUI> texts;
    [SerializeField] private float snapDistance = 1.5f;
    private InputAction _selectAction;
    private bool _isTeleportingActive = false; 
    private TeleportationProvider teleportationProvider;

    void Awake()
    {
        teleportationProvider = GetComponent<TeleportationProvider>();
        InputAction activateAction = actionAsset.FindActionMap("XRI RightHand Locomotion").FindAction("Teleport Mode");
        activateAction.Enable();
        activateAction.performed += OnTeleportActivate;
        activateAction.canceled += OnTeleportDeactivate;
        _selectAction = actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("Select");
        if (_selectAction != null)
        {
            _selectAction.Enable();
        }
        
        if (rayInteractor != null) rayInteractor.enabled = true;
    }

    void OnDestroy()
    {
        InputAction activateAction = actionAsset.FindActionMap("XRI RightHand Locomotion").FindAction("Teleport Mode");
        activateAction.performed -= OnTeleportActivate;
        activateAction.canceled -= OnTeleportDeactivate;
    }

    void Update()
    {
        foreach (Vector3 pos in navigationNodesPos)
        {
            //texts[i].text = "distance pos " + i + " = " + Vector3.Distance(this.transform.position, pos);
            float distanceX = Math.Abs(mainCam.transform.position.x - pos.x);
            float distanceZ = Math.Abs(mainCam.transform.position.z - pos.z);
            if (distanceX < snapDistance && distanceZ < snapDistance)
            {
                if (sphereRenderer != null && sphereRenderer.enabled == false)
                {
                    sphereRenderer.enabled = true;
                }
                if (mineModel != null && mineModel.activeSelf == true)
                {
                    mineModel.SetActive(false);
                }
                return;
            }
            else
            {
                if (sphereRenderer != null && sphereRenderer.enabled == true)
                {
                    sphereRenderer.enabled = false;
                }
                if (mineModel != null && mineModel.activeSelf == false)
                {
                    mineModel.SetActive(true);
                }
            }
        }
    }
    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isTeleportingActive = true;
        }
    }

    private void OnTeleportDeactivate(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _isTeleportingActive = false;
            
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.collider.GetComponent<TeleportationArea>() != null ||
                    hit.collider.GetComponent<TeleportationAnchor>() != null)
                {
                    TeleportRequest request = new TeleportRequest()
                    {
                        destinationPosition = hit.point,
                    };
                    teleportationProvider.QueueTeleportRequest(request);
                }
            }
        }
    }

    public void TeleportToNode(NavigationNode node)
    {
        if (node == null)
        {
            return;
        }
        _isTeleportingActive = false;
        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = new Vector3(node.transform.position.x, 0, node.transform.position.z),
        };
        teleportationProvider.QueueTeleportRequest(request);
        if (sphereRenderer != null)
            {
                //sphereRenderer.enabled = true;
                sphereRenderer.material.SetTexture("_Texture1", node.photo);
                sphereRenderer.material.SetVector("_Offset", node.offset);
            }
            // if (mineModel != null)
            // {
            //     mineModel.SetActive(false);
            // }
    }
}
