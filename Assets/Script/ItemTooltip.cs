using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string itemName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.Show(itemName);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }
}
