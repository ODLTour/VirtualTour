using UnityEngine;
using DG.Tweening;

public class MenuToggle : MonoBehaviour
{
    public GameObject dropdownMenuPanel;
    private RectTransform panelRectTransform;

    void Start()
    {
        panelRectTransform = dropdownMenuPanel.GetComponent<RectTransform>();
        dropdownMenuPanel.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(250, 0);
        dropdownMenuPanel.SetActive(false);
    }
    public void ToggleMenu()
    {

        if (dropdownMenuPanel.activeSelf == true)
        {
            panelRectTransform.DOSizeDelta(new UnityEngine.Vector2(250, 0), 0.5f).SetEase(Ease.InOutBounce).OnComplete(() =>
            {
                dropdownMenuPanel.SetActive(false);
            });;
        }
        else
        {
            dropdownMenuPanel.SetActive(true);
            panelRectTransform.DOSizeDelta(new UnityEngine.Vector2(250, 200), 0.5f).SetEase(Ease.InQuad);
        }
    }
}