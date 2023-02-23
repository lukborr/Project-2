
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/OffensiveSkillSO")]
public class OffensiveSkillSO : ScriptableObject
{
    public SkillShotType skillShotType;
    public bool spawnFromHand;
    public float skillDuration;
    public int skillDamage;
    public float skillSpeed;
    public float skillCooldown;
    public int enemyCountBeforeDestroy;
}
