using System.Xml.Serialization;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        name="Gear "+data.itemId;
        transform.parent=GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type=data.itemType;
        rate = data.damages[0];

        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }

    }
    void RateUp()
    {
            Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
            foreach (Weapon weapon in weapons)
            {
                switch (weapon.id)
                {
                    case 0:
                        weapon.speed = 150 + (150 * rate);
                        break;
                    case 1:
                        weapon.speed = 0.5f * (1f - rate);
                        break;
                case 2:
                    weapon.speed =5f*(1f-rate);
                    break;
                    default:
                        
                        break;
                }
            }
    }
    void SpeedUp()
    {
            float speed = 7;
            GameManager.instance.player.speed = speed + speed * rate;
    }
}
