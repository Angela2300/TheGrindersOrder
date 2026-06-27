using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float smoothSpeed = 5f;

    [Header("Map Background")]
    public SpriteRenderer backgroundRenderer;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void Start()
    {
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Camera.main.aspect;

        Bounds bounds = backgroundRenderer.bounds;

        minX = bounds.min.x + cameraHalfWidth;
        maxX = bounds.max.x - cameraHalfWidth;
        minY = bounds.min.y + cameraHalfHeight;
        maxY = bounds.max.y - cameraHalfHeight;
    }

    void LateUpdate()
    {
        if (player == null || backgroundRenderer == null)
            return;

        Vector3 targetPosition = new Vector3(
            player.position.x,
            player.position.y,
            transform.position.z
        );

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}