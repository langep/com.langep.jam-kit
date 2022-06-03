using System;
using System.Collections.Generic;
using UnityEngine;

namespace Langep.JamKit.UI
{
    [CreateAssetMenu(fileName = "UISystem", menuName = "JamKit/Systems/UISystem")]
    public class UISystem : Langep.JamKit.Systems.System, IUISystem
    {
        private IAnimationHelper _animationHelper;
        private readonly IEnumerable<Type> _providedServices = new List<Type>(){typeof(IUISystem)};

        public override IEnumerable<Type> ProvidedServices()
        {
            return _providedServices;
        }

        protected override void OnInitialize()
        {
            _animationHelper = new DOTweenAnimationHelper();
        }

        public IAnimationHelper GetAnimationHelper()
        {
            return _animationHelper;
        }
    }
}