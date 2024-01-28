using Specifications.Ship;
using TMPro;
using UnityEngine;

namespace Dock.Interface.Info.Card.Variants.Ship.Characteristics
{
    public class DockInfoShipCharacteristicsCardView : MonoBehaviour, IDockInfoShipCharacteristicsCardView
    {
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI SpeedText;
        public TextMeshProUGUI HealthText;
        public TextMeshProUGUI BulletsText;
        public TextMeshProUGUI ReloadTimeText;
        public TextMeshProUGUI ShootRateText;
        public TextMeshProUGUI AutomaticText;
        
        public void FillData(ShipSpecification shipSpecification)
        {
            TitleText.text = shipSpecification.Name;
            SpeedText.text = shipSpecification.Speed.ToString();
            HealthText.text = shipSpecification.Health.ToString();
            BulletsText.text = shipSpecification.BulletCount.ToString();
            ReloadTimeText.text = shipSpecification.ReloadTime.ToString();
            ShootRateText.text = shipSpecification.ShootRate.ToString();
            AutomaticText.text = shipSpecification.IsAutomatic.ToString();
        }
    }
}