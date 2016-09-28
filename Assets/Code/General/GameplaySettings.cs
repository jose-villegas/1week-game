using UnityEngine;

namespace General
{
    /// <summary>
    /// Contains parameters for runtime gameplay
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Gameplay")]
    public class GameplaySettings : ScriptableObject
    {
        [SerializeField, Header("Player")]
        private int _healthPoints = 3;
        [SerializeField]
        private float _playerSpeedMultiplier = 1.0f;
        [SerializeField, Header("Enemies")]
        private int _projectileDamage = 1;
        [SerializeField]
        private float _enemySpeedMultiplier = 1.0f;

        public int HealthPoints
        {
            get { return _healthPoints; }
            set { _healthPoints = value; }
        }

        public float PlayerSpeedMultiplier
        {
            get { return _playerSpeedMultiplier; }
            set { _playerSpeedMultiplier = value; }
        }

        public int ProjectileDamage
        {
            get { return _projectileDamage; }
            set { _projectileDamage = value; }
        }

        public float EnemySpeedMultiplier
        {
            get { return _enemySpeedMultiplier; }
            set { _enemySpeedMultiplier = value; }
        }
    }
}
