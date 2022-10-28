using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ArrowButtonRelator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator[] animator;
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].Play("ArrowSelected");
        }

       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].Play("ArrowUnselected");
        }
    }
}
