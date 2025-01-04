using UnityEngine;
using DG.Tweening;
using TMPro;

public class ParticleCollisionShake : MonoBehaviour
{
    [SerializeField] private RectTransform uiElement;

    [SerializeField] private float scaleTarget = 1.2f;
    [SerializeField] private float tweenDuration = 0.2f;
  

    private int collisionCount;     
    private bool isShaking = false;   
    private Tween shakeTween;  

    private void OnParticleCollision(GameObject other)
    {
        collisionCount++;
    }

    private void LateUpdate()
    {
        if (collisionCount > 0)
        {
            uiElement.gameObject.SetActive(true);
            if (!isShaking)
            {
                StartShake();
            }

        }
        else
        {
            if (isShaking)
            {
                StopShake();
            }
        }

        collisionCount = 0;
    }

    private void StartShake()
    {
        isShaking = true;
         uiElement.gameObject.SetActive(true);
        shakeTween.Kill();
        shakeTween = uiElement
            .DOScale(scaleTarget, tweenDuration)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void StopShake()
    {
        isShaking = false;
        shakeTween.Kill();
        uiElement.localScale = Vector3.one;
        //uiElement.gameObject.SetActive(false);

    }
}
