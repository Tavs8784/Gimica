using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;  

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

    private Tween blinkTween;

    public event Action OnCounterFinished;
    
    private void Awake() 
    {
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

        countText.alpha = 1f;

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

        StartBlinking();

        OnCounterFinished?.Invoke();
    }

    private void StartBlinking()
    {
        blinkTween = countText.DOFade(0.2f, 0.5f)
                              .SetLoops(-1, LoopType.Yoyo)
                              .SetEase(Ease.InOutQuad);
    }

    public void StopBlinking()
    {
        if (blinkTween != null && blinkTween.IsActive())
        {
            blinkTween.Kill();
        }
        countText.alpha = 1f;
    }
}
