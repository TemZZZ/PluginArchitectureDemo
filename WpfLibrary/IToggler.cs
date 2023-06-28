namespace WpfLibrary
{
    /// <summary>
    /// Интерфейс переключателя двух состояний.
    /// </summary>
    public interface IToggler
    {
        /// <summary>
        /// Включен или выключен переключатель.
        /// </summary>
        bool IsToggleOn { get; set; }
    }
}