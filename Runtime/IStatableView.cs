namespace Views
{
    public interface IStatableView
    {
        IViewState State { get; }
    }

    public interface IStatableView<T> : IStatableView where T : IViewState
    {
        new T State { get; set; }
    }
}