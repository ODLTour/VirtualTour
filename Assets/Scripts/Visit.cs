using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(MeshRenderer))]
public class Visit : MonoBehaviour
{
    [SerializeField] private NavigationNode dollHouse;
    [SerializeField] private Transform mine;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private NavigationNode firstNavNode;
    
    [Header("Animation Settings")]
    [SerializeField] private float navDuration = 2.5f;
    [SerializeField] private float fadeDuration = 0.25f;
    [SerializeField] private float fadeValue = 0.25f;

    // Champs privés
    private NavigationNode previousNode;
    private MeshRenderer[] allRenderers;
    private Material targetMaterial;
    private Sequence transitionSequence;

    public NavigationNode CurrentNode
    {
        get { return previousNode; }
        set
        {
            if (previousNode != value)
            {
                NavigationNode oldNode = previousNode;
                previousNode = value; 
                OnNodeChanged(oldNode, value);
            }
        }
    }

    void Awake()
    {
        targetMaterial = GetComponent<MeshRenderer>().material;
    }

    void Start()
    {
        allRenderers = mine.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in allRenderers)
        {
            renderer.enabled = true;
        }
        if (previousNode == null)
        {
            previousNode = dollHouse;
        }
        targetMaterial.SetTexture("_Texture1", previousNode.photo);
        targetMaterial.SetFloat("_Alpha", 0.0f);
        if (firstNavNode != null)
        {
            DOTween.Sequence()
                .AppendInterval(1f)
                .AppendCallback(() => {
                    ToNode(firstNavNode); 
                });
        }
        else
        {
            CurrentNode = dollHouse; 
        }

    }

    public void ToNode(NavigationNode node)
    {
        CurrentNode = node;
    }

    private void OnNodeChanged(NavigationNode oldNode, NavigationNode newNode)
    {
        cameraController.ToggleIsChangingNode(true);
        if (transitionSequence != null && transitionSequence.IsActive())
        {
            transitionSequence.Kill(true);
        }

        bool isGoingToDollHouse = newNode == dollHouse;
        bool isLeavingDollHouse = oldNode == dollHouse;

        if (isGoingToDollHouse || isLeavingDollHouse || newNode != dollHouse)
        {
            foreach (MeshRenderer renderer in allRenderers)
            {
                renderer.enabled = true;
            }
        }
        
        transitionSequence = DOTween.Sequence();
        transitionSequence.AppendCallback(() => 
        {
            cameraController.ToggleIsDollHouse(isGoingToDollHouse);
        });
        transitionSequence.Append(transform.DOMove(newNode.transform.position, navDuration).SetEase(Ease.InOutCirc));
            transitionSequence.Join(cameraController.transform.DOLocalMove(Vector3.zero, navDuration).SetEase(Ease.InOutCirc));
        if (isGoingToDollHouse)
        {
            transitionSequence.Join(targetMaterial.DOFloat(fadeValue, "_Alpha", fadeDuration));
        }
        else
        {
            targetMaterial.SetTexture("_Texture2", newNode.photo);
            transitionSequence.Join(targetMaterial.DOFloat(fadeValue, "_Alpha", fadeDuration).SetEase(Ease.InExpo));
            transitionSequence.Append(targetMaterial.DOFloat(1f, "_LerpFactor", 0.1f));
            transitionSequence.AppendCallback(() =>
            {
                targetMaterial.SetVector("_Offset", newNode.offset);
            });

            transitionSequence.Append(targetMaterial.DOFloat(1f, "_Alpha", fadeDuration).SetEase(Ease.OutCirc));
        }
        transitionSequence.OnComplete(() => OnTransitionComplete(newNode));
    }
    
    private void OnTransitionComplete(NavigationNode newNode)
    {
        targetMaterial.SetTexture("_Texture1", newNode.photo);
        targetMaterial.SetFloat("_LerpFactor", 0f);

        if (newNode != dollHouse)
        {
            foreach (MeshRenderer renderer in allRenderers)
            {
                renderer.enabled = false;
            }
        }
        
        cameraController.ToggleIsChangingNode(false);
    }
}