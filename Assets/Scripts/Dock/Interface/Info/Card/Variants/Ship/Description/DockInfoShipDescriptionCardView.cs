using Specifications.Ship;
using TMPro;
using UnityEngine;

namespace Dock.Interface.Info.Card.Variants.Ship.Description
{
    public class DockInfoShipDescriptionCardView : MonoBehaviour, IDockInfoShipDescriptionCardView
    {
        public TextMeshProUGUI TitleTxt;
        public TextMeshProUGUI DescriptionTxt;
        
        public void FillData(ShipSpecification shipSpecification)
        {
            TitleTxt.text = shipSpecification.Name;
            DescriptionTxt.text = shipSpecification.Description;
        }
    }
}