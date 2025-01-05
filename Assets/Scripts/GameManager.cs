using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
  

    [Header("Intro")]
    [SerializeField] private RectTransform introButton;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 50f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float shakeRandomness = 90f;
    [SerializeField] private Transform presentTransform;
    [SerializeField] private Transform presentTextBubble;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float rotationDuration = 5f;
    [SerializeField] private int rotationLoops = 3;
    [SerializeField] private float rotationTarget = 360f;
    [SerializeField] private float scaleUpDuration = 0.5f;
    [SerializeField] private Vector3 scaleTarget;
    [SerializeField] private Image shineUIElement;
    [SerializeField] private float shineAlphaStart = 0f;
    [SerializeField] private float shineAlphaEnd = 1f;
    [SerializeField] private float shineAlphaDuration = 1f;

    [Space]
    [Header("Opening")]
    [SerializeField] private float presentShakeDuration = 0.5f;
    [SerializeField] private float presentShakeStrength = 30f;
    [SerializeField] private int presentShakeVibrato = 10;
    [SerializeField] private float presentShakeRandomness = 90f;

    [SerializeField] private PlayableDirector lidAnim;
    [SerializeField] private GameObject coinParticles;
    [SerializeField] private CoinCounter coinCounter;
    [SerializeField] private RectTransform bankroll;
    [SerializeField] private float bankrollAnimTime = 1f;
    [SerializeField] private Vector2 finalBankrollPos;
    [SerializeField] private Vector3 finalBankrollScale;

    [Space]
    [Header("Go Back")]
    [SerializeField] private RectTransform backBtn;
    [SerializeField] private PlayableDirector goBackBtnAnim;

    private Material shineMaterial;
    private Sequence revealSequence;


    private void Awake()
    {
        Application.targetFrameRate = 60; 
        QualitySettings.vSyncCount = 0;  
        
        if (coinCounter != null)
        {
            coinCounter.OnCounterFinished += HandleCoinCounterFinished;
        }
    }

    private void OnDestroy()
    {
        if (coinCounter != null)
        {
            coinCounter.OnCounterFinished -= HandleCoinCounterFinished;
        }
    }


    public void PlayIntro()
    {
        RevealIntroButton();
    }

    private void RevealIntroButton()
    {
        if (introButton == null)
        {
            Debug.LogWarning("Intro Button is not assigned.");
            return;
        }

        introButton.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, shakeRandomness)
                   .OnComplete(() =>
                   {
                       introButton.gameObject.SetActive(false);
                       RevealPresent();
                       ShowTextBubble(true, true);
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

        if (shineUIElement == null)
        {
            Debug.LogWarning("Shine UI element is not assigned.");
        }

        presentTransform.position = startPoint.position;
        presentTransform.localScale = Vector3.zero;
        presentTransform.localEulerAngles = Vector3.zero;

        shineUIElement.gameObject.SetActive(true);
        shineMaterial = shineUIElement.material;
        shineMaterial.SetFloat("_Alpha", shineAlphaStart);

        revealSequence = DOTween.Sequence();

        revealSequence
            .Append(presentTransform.DOMove(endPoint.position, moveDuration).SetEase(Ease.OutBack))
            .Join(presentTransform.DOScale(scaleTarget, scaleUpDuration).SetEase(Ease.OutBack))
            .Join(presentTransform
                    .DOLocalRotate(
                        new Vector3(0f, rotationTarget, 0f),
                        rotationDuration,
                        RotateMode.FastBeyond360
                    )
                    .SetLoops(rotationLoops, LoopType.Restart)
                    .SetEase(Ease.OutBack))
            .OnRewind(() =>
            {
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            })
            .SetAutoKill(false);

        if (shineUIElement != null)
        {
            revealSequence.Join(
                DOTween.To(
                    () => shineMaterial.GetFloat("_Alpha"),
                    x => shineMaterial.SetFloat("_Alpha", x),
                    shineAlphaEnd,
                    shineAlphaDuration
                ).SetEase(Ease.OutBack)
            );
        }
    }

   
    private void ShowTextBubble(bool isVisible, bool animateScale)
    {
        presentTextBubble.gameObject.SetActive(isVisible);
        presentTextBubble.localScale = Vector3.zero;

        if (animateScale)
        {
            presentTextBubble.DOScale(1f, 0.5f)
                             .SetDelay(0.3f)
                             .SetEase(Ease.OutBack);
        }
    }


    public void ShakePresent()
    {
        var colorAnim = presentTransform.GetComponent<MatController>();
        if (colorAnim != null)
        {
            colorAnim.StopColorAnimation();
        }

        ShowTextBubble(false, false);

        presentTransform.DOShakeRotation(
            presentShakeDuration,
            presentShakeStrength,
            presentShakeVibrato,
            presentShakeRandomness,
            true
        ).OnComplete(() =>
        {
            coinParticles.SetActive(true);
            lidAnim.Play();
            Debug.LogWarning("YOU WON 100000000 COINS");
        });
    }

 

       private void HandleCoinCounterFinished()
    {
        DOTween.To(
                () => shineMaterial.GetFloat("_Alpha"),
                x => shineMaterial.SetFloat("_Alpha", x),
                shineAlphaStart,
                shineAlphaDuration
            )
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                shineUIElement.gameObject.SetActive(false);
            });

        bankroll.DOAnchorPos(finalBankrollPos, bankrollAnimTime).SetEase(Ease.OutCirc);
        bankroll.DOScale(finalBankrollScale, bankrollAnimTime).SetEase(Ease.OutCirc)
            .OnComplete(() =>
            {
                goBackBtnAnim.Play();
            });
    }

    public void GoBack()
    {
        goBackBtnAnim.enabled = false;
        bankroll.DOScale(Vector3.zero, bankrollAnimTime / 2f).SetEase(Ease.InOutCubic);
        backBtn.DOAnchorPos(new Vector3(0f,-2000f,0f), bankrollAnimTime / 2f).SetEase(Ease.InCubic);

        if (revealSequence != null)
        {
            revealSequence.PlayBackwards();
        }
    }

}
