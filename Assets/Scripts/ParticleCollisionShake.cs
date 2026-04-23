using UnityEngine;
using DG.Tweening;
using TMPro;

public class ParticleCollisionShake : MonoBehaviour
{
    [SerializeField] private RectTransform uiElement;

    [SerializeField] private float scaleTarget = 1.2f;
    [SerializeField] private float tweenDuration = 0.2f;
    [SerializeField] private Ease easeType = Ease.OutBack;
    [SerializeField] private LoopType loopType = LoopType.Yoyo;
  

    private int collisionCount;
    private bool isShaking = false;
    private Tween shakeTween;

    private void OnParticleCollision(GameObject other)
    {
        // If we are already mid-shake, just ignore the new hit to let the current animation finish
        if (isShaking) return;

        StartPulse();
    }

    private void StartPulse()
    {
        isShaking = true;
        uiElement.gameObject.SetActive(true);

        // Kill any existing tween just in case, though isShaking check should prevent overlap
        shakeTween.Kill();

        // Perform a single pulse (Scale Up and back down)
        shakeTween = uiElement
            .DOScale(scaleTarget, tweenDuration)
            .SetEase(easeType)
            .SetLoops(2, LoopType.Yoyo) // 2 loops = Up and then Down
            .OnComplete(() =>
            {
                isShaking = false;
                uiElement.localScale = Vector3.one;
            });
    }

    // Optional: Clean up on disable
    private void OnDisable()
    {
        shakeTween.Kill();
        isShaking = false;
        if(uiElement != null) uiElement.localScale = Vector3.one;
    }
}
