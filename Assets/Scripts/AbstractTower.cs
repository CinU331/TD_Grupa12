
using UnityEngine;

public abstract class AbstractTower : MonoBehaviour
{
    public string TowerIdentificator;

    public float iRange = 20;
    public int iDamage = 80;

    public int iCurrentUpgradeLevel = 1;
    public int iMaximumUpgradeLevel = 3;
    public int iBaseUpgradeCost = 0;
    public BuildingSpot BuildingSpot;

    public Color rockColor;

    protected Color iUpgradeColor;
    protected GameResources iGameResources;

    public abstract void UpgradeTower();
    public abstract void ChangeColor();
    public bool IsUpgradeAvailable
    {
        get
        {
            return iCurrentUpgradeLevel < iMaximumUpgradeLevel;
        }
    }
}

public static class TransformDeepChildExtension
{
    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }
}