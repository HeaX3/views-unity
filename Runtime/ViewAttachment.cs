using Essentials;
using RSG;

namespace Views
{
    public abstract class ViewAttachment<T> : View where T : ViewsContainer
    {
        public override NamespacedKey Id => default;

        public T Views { get; private set; }

        public void Initialize(T views)
        {
            Views = views;
            base.Initialize(views);
        }
    }
}