namespace WpfLibrary
{
    /// <summary>
    /// Интерфейс классов, значения свойств которых можно изменить
    /// путем изменения состояния переключателя.
    /// </summary>
    public interface IToggling
    {
        /// <summary>
        /// Переключатель с двумя состояниями.
        /// </summary>
        IToggler Toggler { get; set; }
    }
}