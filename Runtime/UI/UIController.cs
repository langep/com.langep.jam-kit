using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Langep.JamKit.UI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public class UIController : MonoBehaviour
    {
        [SerializeField] private UIScreen _initialScreen;
        [SerializeField] private GameObject _initialFocusItem;

        private Canvas _canvas;
        private Stack<UIScreen> _stack = new Stack<UIScreen>();

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            if (_initialFocusItem != null)
            {
                EventSystem.current.SetSelectedGameObject(_initialFocusItem);
            }

            if (_initialScreen != null)
            {
                PushScreen(_initialScreen);
            }
        }

        private void OnCancel()
        {
            if (!_canvas.enabled || _canvas.gameObject.activeInHierarchy) return;
            if (_stack.Count == 0) return;

            PopScreen();
        }

        public void PushScreen(UIScreen screen)
        {
            screen.OnEnterAlways();
            screen.Enter();

            if (_stack.Count > 0)
            {
                var currentScreen = _stack.Peek();
                currentScreen.OnExitAlways();
                if (currentScreen.ExitOnNewScreenPush)
                {
                    currentScreen.Exit();
                    Debug.Log("Exit");
                }
            }

            _stack.Push(screen);
        }

        public void PopScreen()
        {
            if (_stack.Count <= 1)
            {
                Debug.LogWarning("Trying to pop last page in screen stack.");
                return;
            }

            var screen = _stack.Pop();
            screen.OnEnterAlways();
            screen.Exit();

            screen = _stack.Peek();
            screen.OnEnterAlways();
            if (screen.ExitOnNewScreenPush)
            {
                screen.Enter();
            }
        }
    }
}