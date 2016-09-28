using UnityEngine;

namespace General
{
    /// <summary>
    /// Handles level progression.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class LevelsSwitcher : MonoBehaviour
    {
        private LevelController[] _levels;
        private int _currentLevel;
        private int _startOver = -1;

        private void Awake()
        {
            _levels = GetComponentsInChildren<LevelController>();

            // deactivate all levels except the first one
            for (int i = 0; i < _levels.Length; i++)
            {
                if(i > 0) _levels[i].gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            // level end event
            EventManager.StartListening("GoToNextLevel", GoToNextLevel);
            EventManager.StartListening("LevelReset", StartOverReset);
        }

        /// <summary>
        /// Deactivates the current level and activates the next
        /// </summary>
        private void GoToNextLevel()
        {
            if (null == _levels) return;

            // final level, can't advance anymore
            if (_currentLevel >= _levels.Length - 1)
            {
                StartOver();
                return;
            }

            // deactivate current
            _levels[_currentLevel].gameObject.SetActive(false);
            // activate next level
            _levels[++_currentLevel].gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets up the game to start over from the first level
        /// </summary>
        private void StartOver()
        {
            _startOver = _currentLevel;
            _currentLevel = 0;
            _levels[_currentLevel].gameObject.SetActive(true);
            EventManager.TriggerEvent("StartOver");
        }

        /// <summary>
        /// Deactivates the last level once the game has started over again
        /// </summary>
        private void StartOverReset()
        {
            if (null == _levels || _startOver < 0 || _startOver > _levels.Length) return;

            _levels[_startOver].gameObject.SetActive(false);
            _startOver = -1;
        }
    }
}
