using Dock.Interface.Info.Card.Variants.Ship.Characteristics;
using Dock.Interface.Info.Card.Variants.Ship.Description;
using Dock.Interface.Info.Card.Variants.Ship.Requirement;
using Dock.Interface.Info.Card.Variants.Weapon;

namespace Dock.Interface.Info
{
    public interface IDockInfoView
    {
        IDockInfoWeaponCardView WeaponCardView { get; }
        IDockInfoShipCharacteristicsCardView ShipCharacteristicsCardView { get; }
        IDockInfoShipRequirementCardView ShipRequirementCardView { get; }
        IDockInfoShipDescriptionCardView ShipDescriptionCardView { get; }
    }
}