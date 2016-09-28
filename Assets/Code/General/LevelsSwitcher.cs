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
        private int _currentLevel = 0;

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
        }

        private void GoToNextLevel()
        {
            if (null == _levels) return;

            // final level, can't advance anymore
            if (_currentLevel > _levels.Length - 1) return;

            // deactivate current
            _levels[_currentLevel].gameObject.SetActive(false);
            // activate next level
            _levels[++_currentLevel].gameObject.SetActive(true);
        }
    }
}
