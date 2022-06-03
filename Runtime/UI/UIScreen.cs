using Langep.JamKit.Systems;
using UnityEngine;
using UnityEngine.Events;

namespace Langep.JamKit.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [DisallowMultipleComponent]
    public class UIScreen : MonoBehaviour
    {
        public bool ExitOnNewScreenPush => _exitOnNewScreenPush;

        [SerializeField] private bool _exitOnNewScreenPush = false;
        
        [Header("Transition")]
        [SerializeField] [Range(0.0f, 10f)] private float _animationDuration = 1f;
        [SerializeField] private UITransitionMode _entryTransition = UITransitionMode.Slide;
        [SerializeField] private Direction _entryDirection = Direction.LEFT;
        [SerializeField] private UITransitionMode _exitTransition = UITransitionMode.Slide;
        [SerializeField] private Direction _exitDirection = Direction.LEFT;

        [Header("Events")]
        [SerializeField] private UnityEvent _prePushAction;
        [SerializeField] private UnityEvent _postPushAction;
        [SerializeField] private UnityEvent _prePopAction;        
        [SerializeField] private UnityEvent _postPopAction;
        

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Coroutine _animationCoroutine;

        private IUISystem _system;
        private IAnimationHelper _animationHelper;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _system = SystemsBootstrapper.Instance.GetService<IUISystem>(typeof(IUISystem));
            _animationHelper = _system.GetAnimationHelper();
        }
        
        public void Enter()
        {
            _prePushAction?.Invoke();

            switch(_entryTransition)
            {
                case UITransitionMode.Fade:
                    FadeIn();
                    break;
                case UITransitionMode.Slide:
                    SlideIn();
                    break;
            }
        }

        public void Exit()
        {
            _prePopAction?.Invoke();
            
            switch (_exitTransition)
            {
                case UITransitionMode.Fade:
                    FadeOut();
                    break;
                case UITransitionMode.Slide:
                    SlideOut();
                    break;                
            }
        }

        private void SlideIn()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(_animationHelper.SlideIn(_rectTransform, _entryDirection, _animationDuration, _postPushAction));      
        }

        private void SlideOut()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(_animationHelper.SlideOut(_rectTransform, _exitDirection, _animationDuration, _postPushAction));
        }
        
        private void FadeIn()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(_animationHelper.FadeIn(_canvasGroup, _animationDuration, _postPushAction));
        }

        private void FadeOut()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
            _animationCoroutine = StartCoroutine(_animationHelper.FadeOut(_canvasGroup, _animationDuration, _postPopAction));
        }
    }
}
