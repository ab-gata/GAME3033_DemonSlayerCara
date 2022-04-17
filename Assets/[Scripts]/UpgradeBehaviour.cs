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
}
