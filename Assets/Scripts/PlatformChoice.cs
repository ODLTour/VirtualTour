using UnityEngine;

public class PlatformChoice : MonoBehaviour
{
    [SerializeField] private GameObject pcCameraRig;
    [SerializeField] private GameObject vrCameraRig;
    [SerializeField] private VRVisit vrVisit;
    [SerializeField] private Visit visitScript; 
    [SerializeField] private GameObject screenCanvas; 
    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        if (pcCameraRig != null) 
        {
            pcCameraRig.SetActive(false);
            if (visitScript != null)
            {
                visitScript.enabled = false;
            }
        }
        
        if (vrCameraRig != null)
        {
            vrCameraRig.SetActive(true);
            if (vrVisit != null)
            {
                vrVisit.enabled = true;
            }
        }
        if (screenCanvas != null)
        {
            screenCanvas.SetActive(false);
        }
#else
        if (vrCameraRig != null) 
        {
            vrCameraRig.SetActive(false);
            if (vrVisit != null)
            {
                vrVisit.enabled = false;
            }
        }
        
        if (pcCameraRig != null)
        {
            pcCameraRig.SetActive(true);
            if (visitScript != null)
            {
                visitScript.enabled = true;
            }
        }
        if (screenCanvas != null)
        {
            screenCanvas.SetActive(true);
        }
#endif
    }
}
