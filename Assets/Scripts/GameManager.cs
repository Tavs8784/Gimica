using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Intro")]
    [SerializeField] private RectTransform introButton; // UI button to shake
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 50f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;
    [SerializeField] private Transform presentTransform; // The Present
    [SerializeField] private Transform startPoint;       // Starting position
    [SerializeField] private Transform endPoint;         // Ending position
    [SerializeField] private float moveDuration = 2f;    // Time to move from start to end
    [SerializeField] private float rotationDuration = 5f;// Time per 360° spin
    [SerializeField] private int rotationLoops = 3;
    [SerializeField] private float rotationTarget = 360f;
    [SerializeField] private float scaleUpDuration = 0.5f;
    [SerializeField] private Vector3 scaleTarget = new Vector3(13f, 13f, 13f);
    [SerializeField] private Image shineUIElement;
    private Material shineMaterial;
    [SerializeField] private float shineAlphaStart = 0f;
    [SerializeField] private float shineAlphaEnd = 1f;
    [SerializeField] private float shineAlphaDuration = 1f;

    public void PlayIntro()
    {
        Reveal();
    }

    private void Reveal()
    {
        if (introButton == null)
        {
            Debug.LogWarning("Intro Button is not assigned.");
            return;
        }

        introButton
            .DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
            .OnComplete(() =>
            {
                introButton.gameObject.SetActive(false);

                RevealPresent();
            });
    }
    private void RevealPresent()
    {
        if (presentTransform == null)
        {
            Debug.LogWarning("Present Transform is not assigned.");
            return;
        }

        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("StartPoint or EndPoint is not assigned.");
            return;
        }

        if (shineUIElement == null )
        {
            Debug.LogWarning("Shine UI element or its material is not assigned.");
        }

        presentTransform.position = startPoint.position;  
        presentTransform.localScale = Vector3.zero;       
        presentTransform.localEulerAngles = Vector3.zero;
        shineUIElement.gameObject.SetActive(true);
        shineMaterial = shineUIElement.material;
        shineMaterial.SetFloat("_Alpha", shineAlphaStart);

      
        Sequence revealSequence = DOTween.Sequence();

        revealSequence.Append(
            presentTransform.DOMove(endPoint.position, moveDuration)
                            .SetEase(Ease.OutBack)
        );

        revealSequence.Join(
            presentTransform.DOScale(scaleTarget, scaleUpDuration)
                            .SetEase(Ease.OutBack)
        );

     
        revealSequence.Join(
            presentTransform.DOLocalRotate(
                new Vector3(0f, rotationTarget, 0f), 
                rotationDuration, 
                RotateMode.FastBeyond360
            )
            .SetLoops(rotationLoops, LoopType.Restart)
            .SetEase(Ease.OutBack)
        );

       
        if (shineUIElement != null && shineMaterial != null)
        {
            revealSequence.Join(
                DOTween.To(
                    () => shineMaterial.GetFloat("_Alpha"),    
                    x  => shineMaterial.SetFloat("_Alpha", x), 
                    shineAlphaEnd,                             
                    shineAlphaDuration                        
                )
                .SetEase(Ease.OutBack)
            );
        }
    }
}
