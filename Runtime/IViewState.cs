using Newtonsoft.Json.Linq;

namespace Views
{
    public interface IViewState
    {
        JObject Serialize();
        void Load(JObject json);
    }
}