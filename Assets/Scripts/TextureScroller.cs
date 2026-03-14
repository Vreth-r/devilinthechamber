using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    public bool scrollX = false;
    public float scrollSpeedX = 0f;
    public bool scrollY = false;
    public float scrollSpeedY = 0f;

    Material material;
    private Vector2 currentOffset = new Vector2 (0, 0);

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollX)
            currentOffset.x += scrollSpeedX * Time.deltaTime;
        if (scrollY)
            currentOffset.y += scrollSpeedY * Time.deltaTime;
        
        material.SetTextureOffset("_BaseMap", currentOffset);
    }
}
