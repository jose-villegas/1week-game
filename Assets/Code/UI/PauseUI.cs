using UnityEngine;
using System.Collections;
using Extensions;
using General;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _quitButton;

    private CanvasGroup _canvasGroup;
    private Animator _animator;
    private int _appearAnimation;

    void Start ()
    {
        if (null == _continueButton)
        {
            Debug.LogError("No continue " + typeof(Button) + " given. " +
                           "Disabling script...");
            enabled = false;
            return;
        }

        if (null == _quitButton)
        {
            Debug.LogError("No quit " + typeof(Button) + " given. " +
                           "Disabling script...");
            enabled = false;
            return;
        }

        this.GetNeededComponent(ref _canvasGroup);
        this.GetNeededComponent(ref _animator);
        // prefetch animation
        _appearAnimation = Animator.StringToHash("Appear");
        // add action to buttons
        _continueButton.onClick.AddListener(Dissapear);
        _quitButton.onClick.AddListener(Dissapear);
        // add resume game to continue
        _continueButton.onClick.AddListener(() =>
        {
            EventManager.TriggerEvent("GameResumed");
        });
        // add quit action to quit button
        _quitButton.onClick.AddListener(Application.Quit);
    }

    private void Update()
    {
        bool pressedScape = Input.GetKeyUp(KeyCode.Escape);

        if (pressedScape && !_animator.GetBool(_appearAnimation))
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _animator.SetBool(_appearAnimation, true);
            EventManager.TriggerEvent("GamePaused");
        }
        else if(pressedScape && _animator.GetBool(_appearAnimation))
        {
            Dissapear();
            EventManager.TriggerEvent("GameResumed");
        }
    }

    /// <summary>
    /// Hides the pause interface
    /// </summary>
    private void Dissapear()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _animator.SetBool(_appearAnimation, false);
    }
}
