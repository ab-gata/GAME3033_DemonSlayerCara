using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum UpgradeType
{ DAMAGE, DEFENSE, HEALTH}


public class UpgradeBehaviour : MonoBehaviour
{
    public UpgradeType upgrade;

    public int value;

    public Light light;

    public Color healthColor;
    public Color damageColor;
    public Color defenseColor;

    private void Start()
    {
        switch (upgrade)
        {
            case UpgradeType.DAMAGE:
                light.color = damageColor;
                break;
            case UpgradeType.DEFENSE:
                light.color = defenseColor;
                break;
            case UpgradeType.HEALTH:
                light.color = healthColor;
                break;
        }
    }
}
