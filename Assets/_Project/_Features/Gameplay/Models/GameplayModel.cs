using System;

namespace SpaceshipBattle.Models
{
    /// <summary>
    /// Tracks score and wave during a gameplay session.
    /// Fires events on change — UI subscribes, no polling needed.
    /// </summary>
    public class GameplayModel
    {
        private int _currentScore;
        private int _currentWave;

        public event Action<int> OnScoreChanged;
        public event Action<int> OnWaveChanged;

        public int CurrentScore
        {
            get => _currentScore;
            set { _currentScore = value; OnScoreChanged?.Invoke(value); }
        }

        public int CurrentWave
        {
            get => _currentWave;
            set { _currentWave = value; OnWaveChanged?.Invoke(value); }
        }

        /// <summary>
        /// Resets to zero at the start of each new game.
        /// </summary>
        public void Reset()
        {
            CurrentScore = 0;
            CurrentWave  = 0;
        }
    }
}