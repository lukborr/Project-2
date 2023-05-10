
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataSO")]
public class EnemySO : ScriptableObject
{
    public int enemyHealth;
    public int enemyDamage;
    public float enemySpeed;

}
