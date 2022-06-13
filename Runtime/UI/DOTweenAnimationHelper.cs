using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Langep.JamKit.UI
{
    public class DOTweenAnimationHelper : IAnimationHelper
    {
        public DOTweenAnimationHelper()
        {
            DOTween.Init();
        }
        
        public IEnumerator FadeIn(CanvasGroup canvasGroup, float duration, UnityEvent onEnd)
        {
            canvasGroup.alpha = 0;
            var rectTransform = canvasGroup.GetComponent<RectTransform>();
            if (rectTransform)
            {
                rectTransform.anchoredPosition = Vector2.zero;
            }
            
            var tween = canvasGroup.DOFade(1, duration).SetEase(Ease.InOutSine);
            tween.OnComplete(() => onEnd?.Invoke());
            yield return tween.WaitForCompletion();
        }

        public IEnumerator FadeOut(CanvasGroup canvasGroup, float duration, UnityEvent onEnd)
        {
            var tween = canvasGroup.DOFade(0, duration).SetEase(Ease.InOutSine);
            tween.OnComplete(() => onEnd?.Invoke());
            yield return tween.WaitForCompletion();
            var rectTransform = canvasGroup.GetComponent<RectTransform>();
            if (rectTransform)
            {
                rectTransform.anchoredPosition = new Vector2(0, Screen.height);
            }
        }

        public IEnumerator SlideIn(RectTransform transform, Direction direction, float duration, UnityEvent onEnd)
        {
            var startPosition = direction switch
            {
                Direction.UP => new Vector2(0, -Screen.height),
                Direction.RIGHT => new Vector2(-Screen.width, 0),
                Direction.DOWN => new Vector2(0, Screen.height),
                Direction.LEFT => new Vector2(Screen.width, 0),
                _ => new Vector2(0, -Screen.height)
            };
            var endPosition = Vector2.zero;
            
            transform.anchoredPosition = startPosition;
            var tween = transform.DOAnchorPos(endPosition, duration).SetEase(Ease.InOutCirc);
            tween.OnComplete(() => onEnd?.Invoke());
            yield return tween.WaitForCompletion();
        }

        public IEnumerator SlideOut(RectTransform transform, Direction direction, float duration, UnityEvent onEnd)
        {
            var startPosition = Vector2.zero;
            var endPosition = direction switch
            {
                Direction.UP => new Vector2(0, Screen.height),
                Direction.RIGHT => new Vector2(Screen.width, 0),
                Direction.DOWN => new Vector2(0, -Screen.height),
                Direction.LEFT => new Vector2(-Screen.width, 0),
                _ => new Vector2(0, Screen.height)
            };

            transform.anchoredPosition = startPosition;
            var tween = transform.DOAnchorPos(endPosition, duration).SetEase(Ease.InOutCirc);
            tween.OnComplete(() => onEnd?.Invoke());
            yield return tween.WaitForCompletion();
        }
    }
}