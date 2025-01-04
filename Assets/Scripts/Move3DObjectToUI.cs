using UnityEngine;

public class Move3DObjectToUI : MonoBehaviour
{
   
    public RectTransform uiElement;

    public Transform objectToMove;

    private Camera mainCamera;


    private void Awake()
    {
        mainCamera = Camera.main;  
    }

    private void Update()
    {
     
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(mainCamera, uiElement.position);

      
        float currentZInScreenSpace = mainCamera.WorldToScreenPoint(objectToMove.position).z;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, currentZInScreenSpace));

        float originalZ = objectToMove.position.z;

        objectToMove.position = new Vector3(worldPos.x, worldPos.y, originalZ);    
    }
}
