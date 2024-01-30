using ReactiveField;

namespace AreaBorders
{
    public interface ISessionAreaBordersModel
    {
        ReactiveField<float> ForwardBorder { get; }
    }
}