using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class ContextualHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject contextualInfo;
    public bool animated;
    private Image buttonImage;
    private RectTransform rectTransform;

    void Start()
    {
        if (contextualInfo != null)
        {
            contextualInfo.SetActive(false);
        }
        buttonImage = this.GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (contextualInfo != null)
        {
            contextualInfo.SetActive(true);
        }
        if (animated)
        {
            buttonImage.DOFade(1.0f, 0.75f);
            rectTransform.DOSizeDelta(new Vector2(15, 15), 0.75f).SetEase(Ease.InOutSine);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (contextualInfo != null)
        {
            contextualInfo.SetActive(false);
        }
        if (animated)
        {
            buttonImage.DOFade(0.35f, 0.75f);
            rectTransform.DOSizeDelta(new Vector2(10, 10), 0.75f).SetEase(Ease.InOutSine);
        }
    }
}