using Extensions;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Handles the restart level interface
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class RestartLevelUI : MonoBehaviour
    {
        private Button _button;
        private Animator _animator;
        private int _appearAnimation;

        private void Start ()
        {
            this.GetNeededComponent(ref _button);
            this.GetNeededComponent(ref _animator);

            // on player death show restart level ui
            if (null == _animator) return;

            EventManager.StartListening("PlayerDied", Appear);

            // add level restart action to button
            if (null == _button) return;

            _button.onClick.AddListener(Dissapear);
            // prefetch animation
            _appearAnimation = Animator.StringToHash("Appear");
        }

        /// <summary>
        /// Hides the restart level interface
        /// </summary>
        private void Dissapear()
        {
            _button.interactable = false;
            _animator.SetBool(_appearAnimation, false);
            LevelController.ActiveLevel.Restart();
        }

        /// <summary>
        /// Shows the restart level interface
        /// </summary>
        private void Appear()
        {
            _button.interactable = true;
            _animator.SetBool(_appearAnimation, true);
        }
    }
}
