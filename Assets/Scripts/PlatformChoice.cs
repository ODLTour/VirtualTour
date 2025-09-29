using UnityEngine;

public class PlatformChoice : MonoBehaviour
{

    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
            Debug.Log("Windows");
        else if (Application.platform == RuntimePlatform.Android)
            Debug.Log("VR");
    }

}
