using RSG;

namespace Views
{
    public interface IView : IGenericView
    {
        IPromise Open();
    }

    public interface IView<in T> : IGenericView
    {
        IPromise Open(T data);
    }

    public interface IView<in T1, in T2> : IGenericView
    {
        IPromise Open(T1 a, T2 b);
    }

    public interface IView<in T1, in T2, in T3> : IGenericView
    {
        IPromise Open(T1 a, T2 b, T3 c);
    }

    public interface IView<in T1, in T2, in T3, in T4> : IGenericView
    {
        IPromise Open(T1 a, T2 b, T3 c, T4 d);
    }

    public interface IView<in T1, in T2, in T3, in T4, in T5> : IGenericView
    {
        IPromise Open(T1 a, T2 b, T3 c, T4 d, T5 e);
    }

    public interface IView<in T1, in T2, in T3, in T4, in T5, in T6> : IGenericView
    {
        IPromise Open(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f);
    }
}