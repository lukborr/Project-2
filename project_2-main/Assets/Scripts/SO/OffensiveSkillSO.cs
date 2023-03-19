
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/OffensiveSkillSO")]
public class OffensiveSkillSO : ScriptableObject
{
    public string skillName;
    public SkillShotType skillShotType;
    public WhereSkillSpawn whereSkillSpawn;
    public float skillDuration;
    public int skillDamage;
    public float skillSpeed;
    public float skillCooldown;
    public int enemyCountBeforeDestroy;
}
