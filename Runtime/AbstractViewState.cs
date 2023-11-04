using Newtonsoft.Json.Linq;

namespace Views
{
    public abstract class AbstractViewState : IViewState
    {
        public virtual JObject Serialize()
        {
            return null;
        }

        public virtual void Load(JObject json)
        {
            
        }
    }
}