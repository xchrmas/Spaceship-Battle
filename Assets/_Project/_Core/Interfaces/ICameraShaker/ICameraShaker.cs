
namespace SpaceshipBattle.Services
{
    /// <summary>
    /// Camera shaker interface for shaking the camera during events like explosions or impacts.
    /// </summary>
    public interface ICameraShaker
    {
        void Shake(float duration, float strength);
    }
}