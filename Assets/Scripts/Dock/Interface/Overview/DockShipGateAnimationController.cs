using Awaiter;
using UnityEngine;

namespace Dock.Interface.Overview
{
    public class DockShipGateAnimationController : MonoBehaviour
    {
        public CustomAwaiter UpAnimationAwaiter = new();
        public CustomAwaiter DownAnimationAwaiter = new();
        public Animator GateAnimator;

        private static readonly int IsUp = Animator.StringToHash("IsUp");
        private static readonly int IsDown = Animator.StringToHash("IsDown");

        private bool _isMovingUp;
        private bool _isMovingDown;

        public async void PlayUpAnimation()
        {
            UpAnimationAwaiter = new CustomAwaiter();
            
            if (_isMovingDown && !DownAnimationAwaiter.IsCompleted)
            {
                await DownAnimationAwaiter;
            }

            _isMovingUp = true;
            GateAnimator.SetBool(IsUp, true);
        }
        
        public async void PlayDownAnimation()
        {
            DownAnimationAwaiter = new CustomAwaiter();
            
            if (_isMovingUp && !UpAnimationAwaiter.IsCompleted)
            {
                await UpAnimationAwaiter;
            }
            
            _isMovingDown = true;
            GateAnimator.SetBool(IsDown, true);
        }
        
        public void OnUpAnimationEnd(string message)
        {
            _isMovingUp = false;
            UpAnimationAwaiter.Complete();
        }

        public void OnDownAnimationEnd(string message)
        {
            _isMovingDown = false;
            DownAnimationAwaiter.Complete();
            
            GateAnimator.SetBool(IsDown, false);
            GateAnimator.SetBool(IsUp, false);
        }
    }
}