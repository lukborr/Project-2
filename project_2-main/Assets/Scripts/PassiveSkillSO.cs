using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PassiveSkillSO")]
public class PassiveSkillSO : ScriptableObject
{
    public Sprite skillSprite;
    public string skillName;
    public string skillDescription;
    public float cooldownReduction;
    public float healthIncrease;
    public float projectileSizeIncrease;
    public float armorIncrease;
    public float movementSpeedIncrease;
    public float attackDamageIncrease;
}
