using ReactiveField;
using Specifications.Ship;

namespace Dock.Interface.Slider.Card
{
    public class DockSliderShipCardModel
    {
        public readonly int Index;
        public readonly ShipSpecification Specification;
        public readonly ReactiveField<SliderCardState> State = new(SliderCardState.Unselected);
        public readonly ReactiveField<SliderCardPreviewState> PreviewState = new(SliderCardPreviewState.Unpreviewed);

        public DockSliderShipCardModel(ShipSpecification specification, int index)
        {
            Index = index;
            Specification = specification;
        }
    }
}