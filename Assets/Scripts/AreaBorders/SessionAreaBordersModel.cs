using ReactiveField;

namespace AreaBorders
{
    public class SessionAreaBordersModel : ISessionAreaBordersModel
    {
        public ReactiveField<float> ForwardBorder { get; set; } = new();
    }
}