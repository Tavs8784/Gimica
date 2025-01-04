using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private ParticleSystem targetParticleSystem;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private int startValue = 0;
    [SerializeField] private int finalValue = 1000000;

    private float timeToReachTarget = 5f;
    private bool isCounting = false;    
    private bool wasPlaying = false;    
    private float currentTime = 0f; 

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
        countText.text = startValue.ToString();
    }

    private void StopCounting()
    {
        isCounting = false;
        countText.text = finalValue.ToString();

        OnCounterFinished?.Invoke();
    }
}
