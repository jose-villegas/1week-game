namespace Interfaces
{
    /// <summary>
    /// Describres an object that can be restarted to an original state
    /// </summary>
    public interface IRestartable
    {
        /// <summary>
        /// Restarts this object.
        /// </summary>
        void Restart();
    }
}
