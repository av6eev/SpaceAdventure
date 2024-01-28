using Specifications.Ship;

namespace Dock.Interface.Info.Card
{
    public interface IDockInfoCardView
    {
        void FillData(ShipSpecification shipSpecification);
    }
}