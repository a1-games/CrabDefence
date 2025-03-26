using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerUpgradesUI : MonoBehaviour
{
    public TMP_Text cost_Penetration;
    public TMP_Text current_Penetration;
    public TMP_Text penetration_Interval;
    public int maxPenetration = 10;
    public TMP_Text cost_AttackSpeed;
    public TMP_Text current_AttackSpeed;
    public TMP_Text attackSpeed_Interval;
    public float maxAttackSpeed = 0.2f;
    public TMP_Text cost_Damage;
    public TMP_Text current_Damage;
    public TMP_Text damage_Interval;
    public float maxDamage = 100f;
    public TMP_Text cost_Range;
    public TMP_Text current_Range;
    public TMP_Text range_Interval;
    public float maxRange = 8f;
    public TMP_Text cost_ProjSpeed;
    public TMP_Text current_ProjSpeed;
    public TMP_Text projSpeed_Interval;
    public float maxProjSpeed = 8f;

    public int penetrationIncrease = 1;
    public float attackSpeedDecrease = 0.2f;
    public float damageIncrease = 10f;
    public float rangeIncrease = 0.5f;
    public float projSpeedIncrease = 1f;

    private int penetrationCost;
    private int attackSpeedCost;
    private int damageCost;
    private int rangeCost;
    private int projspeedCost;

    private Tower t;

    public void OverwriteUIinfo(Tower t)
    {
        this.t = t;

        penetrationCost = t.penetrationInt * 240;
        attackSpeedCost = (int)(600 / t.shootCooldown);
        damageCost = (int)(t.damage * 8.2);
        rangeCost = (int)(t.range * 112);
        projspeedCost = (int)((t.projectileSpeed/5) * 200);

        // penetration amount
        cost_Penetration.text = penetrationCost.ToString();
        current_Penetration.text = t.penetrationInt.ToString();
        penetration_Interval.text = "+" + penetrationIncrease.ToString();
        if (t.penetrationInt >= maxPenetration)
        {
            current_Penetration.text = t.penetrationInt.ToString();
            cost_Penetration.text = "MAX";
        }

        // attack speed
        cost_AttackSpeed.text = attackSpeedCost.ToString();
        current_AttackSpeed.text = string.Format("{0:0.0}", t.shootCooldown);
        attackSpeed_Interval.text = "-" + attackSpeedDecrease.ToString();
        if (t.shootCooldown <= maxAttackSpeed)
        {
            current_AttackSpeed.text = string.Format("{0:0.0}", t.shootCooldown);
            cost_AttackSpeed.text = "MAX";
        }

        // damage
        cost_Damage.text = damageCost.ToString();
        current_Damage.text = t.damage.ToString();
        damage_Interval.text = "+" + damageIncrease.ToString();
        if (t.damage >= maxDamage)
        {
            current_Damage.text = t.damage.ToString();
            cost_Damage.text = "MAX";
        }

        // range
        cost_Range.text = rangeCost.ToString();
        current_Range.text = t.range.ToString();
        range_Interval.text = "+" + rangeIncrease.ToString();
        if (t.range >= maxRange)
        {
            current_Range.text = t.range.ToString();
            cost_Range.text = "MAX";
        }

        // projectile speed
        cost_ProjSpeed.text = projspeedCost.ToString();
        current_ProjSpeed.text = t.projectileSpeed.ToString();
        projSpeed_Interval.text = "+" + projSpeedIncrease.ToString();
        if (t.projectileSpeed >= maxProjSpeed)
        {
            current_ProjSpeed.text = t.projectileSpeed.ToString();
            cost_ProjSpeed.text = "MAX";
        }

        GameManager.AskFor.RefreshCoinsUI();
    }

    public void UpgradePenetration()
    {
        if (GameManager.AskFor.coins < penetrationCost) return;
        if (t.penetrationInt >= maxPenetration)
        {
            current_Penetration.text = t.penetrationInt.ToString();
            cost_Penetration.text = "MAX";
            return;
        }

        GameManager.AskFor.coins -= penetrationCost;
        t.penetrationInt += penetrationIncrease;
        if (t.penetrationInt >= maxPenetration) t.penetrationInt = maxPenetration;

        OverwriteUIinfo(t);
        SoundManager.AskFor.ClickSound();
    }
    public void UpgradeAttackSpeed()
    {
        if (GameManager.AskFor.coins < attackSpeedCost) return;
        if (t.shootCooldown <= maxAttackSpeed)
        {
            t.shootCooldown = maxAttackSpeed;
            current_AttackSpeed.text = string.Format("{0:0.0}", t.shootCooldown);
            cost_AttackSpeed.text = "MAX";
            return;
        }

        GameManager.AskFor.coins -= attackSpeedCost;
        t.shootCooldown -= attackSpeedDecrease;
        if (t.shootCooldown <= maxAttackSpeed) t.shootCooldown = maxAttackSpeed;

        OverwriteUIinfo(t);
        SoundManager.AskFor.ClickSound();
    }
    public void UpgradeDamage()
    {
        if (GameManager.AskFor.coins < damageCost) return;
        if (t.damage >= maxDamage)
        {
            current_Damage.text = t.damage.ToString();
            cost_Damage.text = "MAX";
            return;
        }

        GameManager.AskFor.coins -= damageCost;
        t.damage += damageIncrease;
        if (t.damage >= maxDamage) t.damage = maxDamage;

        OverwriteUIinfo(t);
        SoundManager.AskFor.ClickSound();
    }
    public void UpgradeRange()
    {
        if (GameManager.AskFor.coins < rangeCost) return;
        if (t.range >= maxRange)
        {
            current_Range.text = t.range.ToString();
            cost_Range.text = "MAX";
            return;
        }

        GameManager.AskFor.coins -= rangeCost;
        t.range += rangeIncrease;
        if (t.range >= maxRange) t.range = maxRange;

        OverwriteUIinfo(t);
        t.RefreshRangeIndicator();
        SoundManager.AskFor.ClickSound();
    }
    public void UpgradeProjectileSpeed()
    {
        if (GameManager.AskFor.coins < projspeedCost) return;
        if (t.projectileSpeed >= maxProjSpeed)
        {
            current_ProjSpeed.text = t.projectileSpeed.ToString();
            cost_ProjSpeed.text = "MAX";
            return;
        }

        GameManager.AskFor.coins -= projspeedCost;
        t.projectileSpeed += projSpeedIncrease;
        if (t.projectileSpeed >= maxProjSpeed) t.projectileSpeed = maxProjSpeed;

        OverwriteUIinfo(t);
        SoundManager.AskFor.ClickSound();
    }
}
