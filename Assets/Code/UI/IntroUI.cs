using System;
using System.Globalization;
using General;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    class IntroUI : MonoBehaviour
    {
        [SerializeField]
        private string _nextScene;

        [SerializeField]
        private Button _playButton;
        [SerializeField]
        private Button _optionsButton;
        [SerializeField]
        private Button _quitButton;
        [SerializeField, Header("Options")]
        private InputField _playerHealthField;
        [SerializeField]
        private InputField _playerSpeedField;
        [SerializeField]
        private InputField _enemySpeedField;
        [SerializeField]
        private InputField _projectileDamageField;

        private GameplaySettings _gameplay;

        private void Start()
        {
            _gameplay = GameSettings.Instance.GameplaySettings;

            if (null != _playButton)
            {
                _playButton.onClick.AddListener(() =>
                {
                    LoadScene(_nextScene);
                });
            }

            if (null != _quitButton)
            {
                _quitButton.onClick.AddListener(Application.Quit);
            }

            SetOption(_playerHealthField, _gameplay.HealthPoints, str =>
            {
                int value;

                if (int.TryParse(str, out value))
                {
                    _gameplay.HealthPoints = value;
                }
            });
            SetOption(_playerSpeedField, _gameplay.PlayerSpeedMultiplier, str =>
            {
                float value;

                if (float.TryParse(str, out value))
                {
                    _gameplay.PlayerSpeedMultiplier = value;
                }
            });
            SetOption(_enemySpeedField, _gameplay.EnemySpeedMultiplier, str =>
            {
                float value;

                if (float.TryParse(str, out value))
                {
                    _gameplay.EnemySpeedMultiplier = value;
                }
            });
            SetOption(_projectileDamageField, _gameplay.ProjectileDamage, str =>
            {
                int value;

                if (int.TryParse(str, out value))
                {
                    _gameplay.ProjectileDamage = value;
                }
            });
        }

        private void SetOption(InputField field, float value,
                               UnityAction<string> action)
        {
            if (null == field || null == field.placeholder) return;

            var text = field.placeholder.GetComponent<Text>();

            if (null != text)
            {
                text.text = value.ToString(CultureInfo.CurrentCulture);
            }

            if (null != action)
            {
                field.onEndEdit.AddListener(action);
            }
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
