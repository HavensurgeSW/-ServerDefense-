using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowButtonRelator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator[] animator = null;

    private int isSelectedHash = Animator.StringToHash("isSelected");

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < animator.Length; i++)
        {
            //animator[i].Play("ArrowSelected");
            animator[i].SetBool(isSelectedHash, true);
        }       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < animator.Length; i++)
        {
            //animator[i].Play("ArrowUnselected");
            animator[i].SetBool(isSelectedHash, false);
        }
    }
}
