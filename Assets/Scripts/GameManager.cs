using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Intro")]
    [SerializeField] private Transform presentTransform;
    [SerializeField] private RectTransform introButton;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private  float shakeStrength = 50f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;

    [SerializeField] private float scaleUpDuration = 0.5f;
    [SerializeField] private Vector3 scaleTarget;

   
    [SerializeField] private float rotationDuration = 5f;

    private void RevealPresent()
    {
        if (presentTransform == null)
        {
            return;
        }

        presentTransform.localScale = Vector3.zero;
        presentTransform.localEulerAngles = Vector3.zero;

        presentTransform
            .DOScale(scaleTarget, scaleUpDuration)
            .SetEase(Ease.OutBounce);

        presentTransform
            .DOLocalRotate(
                new Vector3(0f, 360f, 0f),  
                rotationDuration, 
                RotateMode.FastBeyond360
            )
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void ShakeButton()
    {
         introButton
                // Shakes the RectTransform along its anchored position
                .DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
                // When the shake finishes:
                .OnComplete(() =>
                {
                    // 2. Deactivate the Play Button
                    introButton.gameObject.SetActive(false);

                    // 3. Begin revealing the Present
                    RevealPresent();
                });
    }

    public void PlayIntro()
    {
        ShakeButton();
    }
}
