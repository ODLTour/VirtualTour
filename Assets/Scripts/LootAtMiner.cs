using UnityEngine;

[ExecuteInEditMode]
public class LootAtMiner : MonoBehaviour
{

    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // Calcule la direction de la caméra
        Vector3 directionToCamera = target.position - transform.position;

        // Réinitialise l'axe Y pour ignorer les rotations en X et Z
        directionToCamera.y = 0;

        // Calcule la rotation pour regarder la caméra
        Quaternion rotation = Quaternion.LookRotation(directionToCamera);

        // Applique la rotation au transform du mineur
        transform.rotation = rotation;
    }
}
