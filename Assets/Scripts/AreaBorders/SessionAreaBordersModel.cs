using ReactiveField;

namespace AreaBorders
{
    public class SessionAreaBordersModel : ISessionAreaBordersModel
    {
        public static float ForwardAdditionalNumber = 1000f;
        public ReactiveField<float> ForwardBorder { get; set; } = new();
    }
}