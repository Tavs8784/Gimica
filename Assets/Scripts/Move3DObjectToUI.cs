using UnityEngine;

public class Move3DObjectToUI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The UI element we want to track")]
    public RectTransform uiElement;

    [Tooltip("The 3D object we want to move")]
    public Transform objectToMove;

    [Tooltip("Camera used for conversion. Default is main camera.")]
    public Camera mainCamera;


    private void Awake()
    {
        // If no camera is assigned, use the main camera by default
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (uiElement == null || objectToMove == null || mainCamera == null)
            return;

        // 1. Convert UI element's position to screen space
       
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, uiElement.position);

        // 2. Convert screen space to a world position in front of the camera
        // We create a Vector3 with zDistanceFromCamera so it appears in 3D space
          float currentZInScreenSpace = mainCamera.WorldToScreenPoint(objectToMove.position).z;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, currentZInScreenSpace));

         float originalZ = objectToMove.position.z;

        // 3. Assign the new world position to the 3D object
        objectToMove.position = new Vector3(worldPos.x, worldPos.y, originalZ);    }
}
