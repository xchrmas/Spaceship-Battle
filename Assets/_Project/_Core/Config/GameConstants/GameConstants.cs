
namespace SpaceshipBattle.Core
{
    public static class GameConstants
    {
        public static class Audio
        {
            // SFX volumes
            public const float BlasterVolume   = 0.25f;
            public const float ExplosionVolume = 0.15f;

            // Music fade duration in seconds
            public const float MusicFadeDuration = 2f;

            // Duck effect — briefly lowers music on player hit
            public const float DuckTargetVolume   = 0.02f;
            public const float DuckDuration       = 0.5f;

            // Music — no spatial blend
            public const float SpatialBlendFlat = 0f;
        }

        public static class Combat
        {
            // Offset from player/enemy position to projectile spawn point
            public const float ProjectileSpawnOffset = 1.5f;
        }

        public static class Camera
        {
            public const float ShakeDuration = 0.5f;
            public const float ShakeStrength = 1.0f;

            // DOTween vibrato — controls how many vibrations per second
            public const int ShakeVibrato = 75;
        }

        public static class Vfx
        {
            // Muzzle flashlight duration in milliseconds
            public const int MuzzleFlashDurationMs = 100;
        }

        public static class Animation
        {
            // Menu intro
            public const float MenuFadeInDuration    = 0.6f;
            public const float TitleFadeInDelay      = 0.3f;
            public const float TitleFadeInDuration   = 0.8f;
            public const float TitlePulseScale       = 1.03f;
            public const float TitlePulseDuration    = 1.5f;
            public const float ButtonStartDelay      = 0.7f;
            public const float ButtonScoresDelay     = 0.9f;
            public const float ButtonSlideDuration   = 0.5f;
            public const float ButtonSlideOffset     = 60f;
            public const float TitleInitialScale     = 0.8f;

            // Gameplay HUD
            public const float VignetteFadeDuration  = 0.25f;
            public const int   VignetteLoops         = 2;
            public const float ScorePunchScale       = 0.25f;
            public const float ScorePunchDuration    = 0.125f;
        }

        public static class Enemy
        {
            // Row thresholds for enemy type assignment
            public const int TypeZeroMaxRow = 1;
            public const int TypeOneMaxRow  = 3;
        }
    }
}