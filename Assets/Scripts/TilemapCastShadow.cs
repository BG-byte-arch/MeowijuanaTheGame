using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCastShadow : MonoBehaviour
{
    public Vector2 offset = new Vector2(-0.1f, -0.1f); // Shadow offset
    public float alpha = 0.5f; // Transparency level (50%)
    private GameObject shadowObj;

    void Start()
    {
        CreateShadow();
    }

    void CreateShadow()
    {
        Tilemap mainTilemap = GetComponent<Tilemap>();
        TilemapRenderer mainRenderer = GetComponent<TilemapRenderer>();

        if (mainTilemap == null || mainRenderer == null)
        {
            Debug.LogError("DropShadow requires a Tilemap and TilemapRenderer on the main GameObject!");
            return;
        }

        // Create a new shadow GameObject
        shadowObj = new GameObject("Shadow");
        shadowObj.transform.SetParent(transform);
        shadowObj.transform.localPosition = (Vector3)offset;
        shadowObj.transform.localRotation = Quaternion.identity;
        shadowObj.transform.localScale = Vector3.one;

        // Add Tilemap and TilemapRenderer to the shadow object
        Tilemap shadowTilemap = shadowObj.AddComponent<Tilemap>();
        TilemapRenderer shadowRenderer = shadowObj.AddComponent<TilemapRenderer>();

        // Copy the tilemap content
        shadowTilemap.SetTilesBlock(mainTilemap.cellBounds, mainTilemap.GetTilesBlock(mainTilemap.cellBounds));

        // Darken the shadow color (black with transparency)
        shadowTilemap.color = new Color(0f, 0f, 0f, alpha);

        // Set sorting order behind the main tilemap
        shadowRenderer.sortingLayerID = mainRenderer.sortingLayerID;
        shadowRenderer.sortingOrder = mainRenderer.sortingOrder - 1;
    }
}
