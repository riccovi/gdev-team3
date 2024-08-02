using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DrawLineToMouse : MonoBehaviour
{
    private float lineLength = 5.0f;  // Set this value in the inspector
    public LineRenderer lineRenderer;

    private Camera mainCamera;

    public Transform ShootPosition;
    public playerMovement player;

    void Start()
    {
        player=transform.parent.parent.GetComponent<playerMovement>();
        mainCamera = Camera.main;

        lineLength =player.ThrowLenght;

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Set the LineRenderer to 2D
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        Vector3 mousePosition = new Vector2(GetMouseWorldPosition().x,GetMouseWorldPosition().y);

        // Calculate the direction from the player to the mouse
        Vector2 direction = (mousePosition - ShootPosition.position).normalized;

        // Calculate the end position of the line
        var shootPos2D= new Vector2(ShootPosition.position.x,ShootPosition.position.y);
        Vector2 endPosition = shootPos2D + direction * lineLength;

        // Draw the line
        lineRenderer.SetPosition(0, ShootPosition.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = -1;  // Set Z to 0 for 2D
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}

