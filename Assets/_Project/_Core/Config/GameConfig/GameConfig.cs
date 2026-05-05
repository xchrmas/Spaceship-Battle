using SpaceshipBattle.Models;
using SpaceshipBattle.Services;
using UnityEngine;

namespace SpaceshipBattle.Core
{

    public class GameConfig : MonoBehaviour
    {
        [Header("Enemy")]
        public EnemyConfig Enemy = new();

        [Header("Player")]
        public PlayerConfig Player = new();

        [Header("Level")]
        public LevelConfig Level = new();

        [Header("Audio")]
        public AudioConfig Audio = new();
    }
}