using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Intro")]
    [SerializeField] private RectTransform introButton; // UI button to shake

    [Header("Button Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 50f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;

    [Header("Present Movement & Rotation")]
    [SerializeField] private Transform presentTransform;    // The Present
    [SerializeField] private Transform startPoint;          // Starting position
    [SerializeField] private Transform endPoint;            // Ending position
    [SerializeField] private float moveDuration = 2f;       // Time to move from start to end
    [SerializeField] private float rotationDuration = 5f;   // Time per 360° spin
    [Tooltip("Number of rotation loops. Set to -1 for infinite loops.")]
    [SerializeField] private int rotationLoops = 3;
    [SerializeField] private float rotationTarget;

    [Header("Present Scale")]
    [SerializeField] private float scaleUpDuration = 0.5f;
    [SerializeField] private Vector3 scaleTarget = new Vector3(13f, 13f, 13f);

    /// <summary>
    /// Call this method (e.g., from a UI Button onClick) to start the intro sequence:
    /// 1) Shake the button
    /// 2) Hide the button
    /// 3) Move + scale + rotate the Present
    /// </summary>
    public void PlayIntro()
    {
        ShakeButton();
    }

    /// <summary>
    /// Shakes the button in anticipation, then hides it and reveals the present.
    /// </summary>
    private void ShakeButton()
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
                // Hide the button
                introButton.gameObject.SetActive(false);

                // Reveal the present
                RevealPresent();
            });
    }

    /// <summary>
    /// Moves the present from startPoint to endPoint, scales it up, and rotates it on Y.
    /// </summary>
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

        // Reset the present
        presentTransform.position = startPoint.position;  // Start location
        presentTransform.localScale = Vector3.zero;       // Shrunk to 0
        presentTransform.localEulerAngles = Vector3.zero; // No rotation initially

        // Build a sequence to move, scale, and rotate simultaneously
        Sequence revealSequence = DOTween.Sequence();

        // 1) Move from startPoint to endPoint
        revealSequence.Append(
            presentTransform.DOMove(endPoint.position, moveDuration)
                            .SetEase(Ease.OutBack)
        );

        // 2) Scale from 0 to scaleTarget with a bounce - run in parallel
        revealSequence.Join(
            presentTransform.DOScale(scaleTarget, scaleUpDuration)
                            .SetEase(Ease.OutBack)
        );

        // 3) Rotate around Y-axis for rotationLoops times - also in parallel
        //    NOTE: If rotationLoops > 1, the sequence will last for (rotationDuration * rotationLoops).
        revealSequence.Join(
            presentTransform.DOLocalRotate(
                new Vector3(0f, rotationTarget, 0f), 
                rotationDuration, 
                RotateMode.FastBeyond360
            )
            .SetLoops(rotationLoops, LoopType.Restart)
            .SetEase(Ease.OutBack)
        );
    }
}
