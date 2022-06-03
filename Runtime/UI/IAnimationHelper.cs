using System.Collections;
using Langep.JamKit.Systems;
using UnityEngine;
using UnityEngine.Events;

namespace Langep.JamKit.UI
{
    public interface IAnimationHelper : IService
    {
        public IEnumerator FadeIn(CanvasGroup canvasGroup, float duration, UnityEvent onEnd);
        public IEnumerator FadeOut(CanvasGroup canvasGroup, float duration, UnityEvent onEnd);
        public IEnumerator SlideIn(RectTransform transform, Direction direction, float duration, UnityEvent onEnd);
        public IEnumerator SlideOut(RectTransform transform, Direction direction, float duration, UnityEvent onEnd);
    }
}