using Dock.Interface.Info.Card.Variants.Ship.Characteristics;
using Dock.Interface.Info.Card.Variants.Ship.Description;
using Dock.Interface.Info.Card.Variants.Ship.Requirement;
using Dock.Interface.Info.Card.Variants.Weapon;
using UnityEngine;

namespace Dock.Interface.Info
{
    public class DockInfoView : MonoBehaviour, IDockInfoView
    {
        public DockInfoWeaponCardView WeaponCardViewGo;
        public DockInfoShipCharacteristicsCardView ShipCharacteristicsCardViewGo;
        public DockInfoShipRequirementCardView ShipRequirementCardViewGo;
        public DockInfoShipDescriptionCardView ShipDescriptionCardViewGo;

        public IDockInfoWeaponCardView WeaponCardView => WeaponCardViewGo;
        public IDockInfoShipCharacteristicsCardView ShipCharacteristicsCardView => ShipCharacteristicsCardViewGo;
        public IDockInfoShipRequirementCardView ShipRequirementCardView => ShipRequirementCardViewGo;
        public IDockInfoShipDescriptionCardView ShipDescriptionCardView => ShipDescriptionCardViewGo;
    }
}