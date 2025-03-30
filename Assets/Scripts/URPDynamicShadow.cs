using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ShadowCaster2D))]
public class URPDynamicShadow : MonoBehaviour
{
    public int minOrderForShadow = 0; // Shadows appear if sortingOrder >= this
    
    private ShadowCaster2D shadowCaster;
    private TilemapRenderer tilemapRenderer;

    void Start()
    {
        shadowCaster = GetComponent<ShadowCaster2D>();
        tilemapRenderer = GetComponent<TilemapRenderer>();
        UpdateShadowState();
    }

    void Update()
    {
        UpdateShadowState();
    }

    private void UpdateShadowState()
    {
        // Enable shadow only if sortingOrder meets threshold
        bool shouldCastShadow = tilemapRenderer.sortingOrder >= minOrderForShadow;
        shadowCaster.enabled = shouldCastShadow;
    }
}