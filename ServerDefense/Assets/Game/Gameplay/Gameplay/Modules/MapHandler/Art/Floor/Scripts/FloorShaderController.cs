using UnityEngine;

public class FloorShaderController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Vector2 movementSpeed = Vector2.zero;

    private Material material = null;

    private void Awake()
    {
        material = spriteRenderer.material;
    }

    private void Update()
    {
        Vector2 offset = movementSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
