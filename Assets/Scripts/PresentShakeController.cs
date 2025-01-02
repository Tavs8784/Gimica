using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentShakeController : MonoBehaviour
{
   private GameManager gameManager;
   
   private void Awake() 
   {
        gameManager = FindObjectOfType<GameManager>();
   }

   private void OnMouseDown()
   {
        gameManager.ShakePresent();
   }
}
