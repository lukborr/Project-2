
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/OffensiveSkillSO")]
public class OffensiveSkillSO : ScriptableObject
{
    public bool isProjectile;
    public Vector2 spawnPlace;
    public float skillDuration;
    public int skillDamage;
    public float skillSpeed;
    public float skillCooldown;
    public int enemyCountBeforeDestroy;
}
