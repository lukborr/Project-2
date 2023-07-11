
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/OffensiveSkillSO")]
public class OffensiveSkillSO : ScriptableObject
{
    public string skillName;
    public SkillShotType skillShotType;
    public WhereSkillSpawn whereSkillSpawn;
    public float skillDuration;
    public float dotDuration;
    public int skillDamage;
    public float skillSpeed;
    public float skillCooldown;
    public float SkillRange;
    public int enemyCountBeforeDestroy;
    public Sprite skillSprite;
    public string skillDescription;
    public float stunDuration;
    public bool needsSecondarySkill;
}
