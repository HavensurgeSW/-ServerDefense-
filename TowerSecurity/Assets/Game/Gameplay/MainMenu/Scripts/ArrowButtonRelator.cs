using UnityEngine;
using UnityEngine.EventSystems;


public class ArrowButtonRelator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Animator[] animator = null;

    private int isSelectedHash = Animator.StringToHash("isSelected");

    public void OnPointerEnter(PointerEventData eventData)
    {
        RunAnimatorLoop(true);     
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RunAnimatorLoop(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        RunAnimatorLoop(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        RunAnimatorLoop(false);
    }

    private void RunAnimatorLoop(bool b) {
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].SetBool(isSelectedHash, b);
        }
    }
}
