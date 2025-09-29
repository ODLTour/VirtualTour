using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class VirtualTour : MonoBehaviour
{
    public List<NavigationNode> navNodes = new List<NavigationNode>();
    public NavigationNode currentNode;

    [Header("Animation Settings")]
    public float navDuration = 2.5f;
    public float fadeDuration = 0.25f;
    public float fadeValue = 0.25f;
    public Transform mine;
    private MeshRenderer[] allRenderers;
    public Material targetMaterial;
    private Sequence transitionSequence;

    public NavigationNode CurrentNode
    {
        get { return currentNode; }
        set
        {
            if (currentNode != value)
            {
                currentNode = value;
                OnNodeChanged();
            }
        }
    }

    void Awake()
    {
        targetMaterial = this.GetComponent<MeshRenderer>().material;
    }
    void Start()
    {
        CurrentNode = navNodes[0];
        targetMaterial.SetTexture("_Texture1", CurrentNode.photo);
        targetMaterial.SetFloat("_Alpha", 1);
        allRenderers = mine.GetComponentsInChildren<MeshRenderer>();
    }

    private void OnNodeChanged()
    {
        foreach (MeshRenderer renderer in allRenderers)
        {
            renderer.enabled = true;
        }
        targetMaterial.SetTexture("_Texture2", currentNode.photo);

        transitionSequence = DOTween.Sequence();

        transitionSequence.Append(transform.DOMove(currentNode.transform.position, navDuration).SetEase(Ease.InOutCirc));

        transitionSequence.Join(targetMaterial.DOFloat(fadeValue, "_Alpha", fadeDuration));

        transitionSequence.Append(targetMaterial.DOFloat(1f, "_LerpFactor", 0.1f));
        transitionSequence.AppendCallback(() => {
            targetMaterial.SetVector("_Offset", currentNode.offset);
        });

        transitionSequence.Append(targetMaterial.DOFloat(1f, "_Alpha", fadeDuration));

        transitionSequence.OnComplete(() =>
        {
            targetMaterial.SetTexture("_Texture1", currentNode.photo);
            targetMaterial.SetFloat("_LerpFactor", 0f);
            foreach (MeshRenderer renderer in allRenderers)
        {
            renderer.enabled = false;
        }
        });
    }

    public void ToNode(NavigationNode node)
    {
        CurrentNode = node;
    }
}
