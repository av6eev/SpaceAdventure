using Specifications.Ship;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dock.Interface.Info.Card.Variants.Weapon
{
    public class DockInfoWeaponCardView : MonoBehaviour, IDockInfoWeaponCardView
    {
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI DescriptionText;
        public TextMeshProUGUI PriceTxt;
        public TextMeshProUGUI BulletsTypeText;
        public Image Image;
        
        public void FillData(ShipSpecification shipSpecification)
        {
        }
    }
}