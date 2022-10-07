using System.Collections.Generic;

using UnityEngine;

public class Aimbot : MonoBehaviour
{
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private CircleCollider2D circleCollider;

    private string[] targetTags = null;

    public List<GameObject> TARGETS { get => targets; }

    public void Init(params string[] targetTags)
    {
        this.targetTags = targetTags;
    }

    public void SetRange(float range)
    {
        circleCollider.radius = range;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTags == null)
        {
            TryAddTarget(collision.gameObject);
            return;
        }

        for (int i = 0; i < targetTags.Length; i++)
        {
            if (collision.CompareTag(targetTags[i]))
            {
                TryAddTarget(collision.gameObject);
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TryRemoveTarget(collision.gameObject);
    }

    private void TryAddTarget(GameObject obj)
    {
        if (!targets.Contains(obj))
        {
            targets.Add(obj);
        }
    }

    private void TryRemoveTarget(GameObject obj)
    {
        if (targets.Contains(obj))
        {
            targets.Remove(obj);
        }
    }

    public bool ContainsTargets()
    {
        return targets != null && targets.Count > 0;
    }

    public T GetTargetComponent<T>(int i) where T : Component
    {
        return targets[i].GetComponent<T>();
    }

    public bool TryGetTargetComponent<T>(out T component) where T : Component
    {
        component = null;

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].TryGetComponent(out component))
            {
                return true;
            }
        }

        return false;
    }
}
