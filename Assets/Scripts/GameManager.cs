using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Intro")]
    public Transform presentTransform;

    [SerializeField] private float scaleUpDuration = 0.5f;
    [SerializeField] private Vector3 scaleTarget;

   
    [SerializeField] private float rotationDuration = 5f;

    public void RevealPresent()
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
}
