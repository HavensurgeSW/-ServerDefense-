using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowButtonRelator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator currentAnimator = null;
    [SerializeField] private Animator[] recursiveAnimator = null;

    private int isSelectedHash = Animator.StringToHash("isSelected");

    public void OnPointerEnter(PointerEventData eventData)
    {
        RunAnimatorLoop(true);     
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RunAnimatorLoop(false);
    }

    private void RunAnimatorLoop(bool b) 
    {
        currentAnimator.SetBool(isSelectedHash, b);
    
        for (int i = 0; i < recursiveAnimator.Length; i++)
        {
            recursiveAnimator[i].SetBool(isSelectedHash, b);
        }
    }
}
