using UnityEngine;
using DG.Tweening;

public class MatController : MonoBehaviour
{
    [Header("MeshRenderers")]
    [SerializeField] private MeshRenderer ribbon1;
    [SerializeField] private MeshRenderer ribbon2;
    [SerializeField] private MeshRenderer ribbon3;
    [SerializeField] private MeshRenderer ribbon4;
    [SerializeField] private MeshRenderer ribbon5;

    [Header("Tween Settings")]
    [SerializeField] private Color startColor = Color.red;

    [SerializeField] private Color endColor = Color.blue;

    [SerializeField] private float duration = 2f;

    private MaterialPropertyBlock[] _mpbs;
    private Color _currentColor;

    private Tween _colorTween;

    private void Awake()
    {
        _mpbs = new MaterialPropertyBlock[5];
        for (int i = 0; i < _mpbs.Length; i++)
        {
            _mpbs[i] = new MaterialPropertyBlock();
        }

        _currentColor = startColor;

     
        _colorTween = DOTween.To(() => _currentColor, 
                   value => _currentColor = value, 
                   endColor, 
                   duration)
               .SetLoops(-1, LoopType.Yoyo)
               .SetEase(Ease.Linear)
               .OnUpdate(UpdateColors);
    }

  


    private void UpdateColors()
    {
        ribbon1.GetPropertyBlock(_mpbs[0]);
        ribbon2.GetPropertyBlock(_mpbs[1]);
        ribbon3.GetPropertyBlock(_mpbs[2]);
        ribbon4.GetPropertyBlock(_mpbs[3], 0); 
        ribbon5.GetPropertyBlock(_mpbs[4], 1); 

        for (int i = 0; i < _mpbs.Length; i++)
        {
            _mpbs[i].SetColor("_BaseColor", _currentColor);
        }

        ribbon1.SetPropertyBlock(_mpbs[0]);
        ribbon2.SetPropertyBlock(_mpbs[1]);
        ribbon3.SetPropertyBlock(_mpbs[2]);
        ribbon4.SetPropertyBlock(_mpbs[3], 0);
        ribbon5.SetPropertyBlock(_mpbs[4], 1);
    }

    public void StopColorAnimation()
    {
       
         _colorTween.Kill(); 
        _currentColor = startColor;

        UpdateColors();
    }
}
