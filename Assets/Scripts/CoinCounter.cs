using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;  // <-- Important for DOTween

public class CoinCounter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem targetParticleSystem;
    [SerializeField] private TextMeshProUGUI countText;

    [Header("Counter Values")]
    [SerializeField] private int startValue = 0;
    [SerializeField] private int finalValue = 1000000;

    private float timeToReachTarget = 5f;
    private bool isCounting = false;    
    private bool wasPlaying = false;    
    private float currentTime = 0f; 

    // If you want to stop the blink at some point, store the tween so you can kill it
    private Tween blinkTween;

    public event Action OnCounterFinished;
    
    private void Awake() 
    {
        // Use the duration of the particle system as timeToReachTarget
        timeToReachTarget = targetParticleSystem.main.duration;  
    }

    private void Update()
    {
        if (targetParticleSystem.isPlaying)
        {
            if (!wasPlaying)
            {
                StartCounting();
            }

            if (isCounting)
            {
                currentTime += Time.deltaTime;
                float progress = currentTime / timeToReachTarget;
                int range = finalValue - startValue;
                int displayValue = startValue + Mathf.FloorToInt(progress * range);
                countText.text = displayValue.ToString();
            }

            wasPlaying = true;
        }
        else
        {
            if (wasPlaying && isCounting)
            {
                StopCounting();
            }
            wasPlaying = false;
        }
    }

    private void StartCounting()
    {
        isCounting = true;
        currentTime = 0f;

        // Ensure the text is visible
        countText.alpha = 1f;

        // If a blink tween is still running from a previous run, kill it
        if (blinkTween != null && blinkTween.IsActive())
        {
            blinkTween.Kill();
        }

        countText.text = startValue.ToString();
    }

    private void StopCounting()
    {
        isCounting = false;
        countText.text = finalValue.ToString();

        // Trigger the blink effect
        StartBlinking();

        // Invoke the event in case something else should happen once counting finishes
        OnCounterFinished?.Invoke();
    }

    /// <summary>
    /// Makes the text blink by animating its alpha.
    /// </summary>
    private void StartBlinking()
    {
        // DOTween can animate TextMeshPro's alpha directly
        // Example: fade to 0 over 0.5s, loop indefinitely in Yoyo mode
        blinkTween = countText.DOFade(0.2f, 0.5f)
                              .SetLoops(-1, LoopType.Yoyo)
                              .SetEase(Ease.InOutQuad);
    }

    /// <summary>
    /// Call this if you ever need to stop the blinking and reset alpha to 1.
    /// </summary>
    public void StopBlinking()
    {
        if (blinkTween != null && blinkTween.IsActive())
        {
            blinkTween.Kill();
        }
        countText.alpha = 1f;
    }
}
