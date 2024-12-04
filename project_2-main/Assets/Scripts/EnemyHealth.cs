using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealth : MonoBehaviour
{
    public int health;
    public float maxHealth = 100;
    [SerializeField] private GameObject DamageOutput;
    private int counter = 2;
    [SerializeField] EnemySO enemySO;

    void Start()
    {
        health = enemySO.enemyHealth;
    }

    public void RemoveHealth(int healthToRemove)
    {
        health -= healthToRemove;
        InstantiateDamageOutput(healthToRemove);

        if (health <= 0)
        {
            DieAndDrop();
        }
    }

    public void DieAndDrop()
    {
        Transform childTransform = transform.GetChild(0);
        childTransform.gameObject.SetActive(true);
        childTransform.transform.parent = null;
        Destroy(gameObject);
    }

    public void InstantiateDamageOutput(int damage)
    {
        counter++;
        if (counter >= 6)
        {
            counter = 2;
        }
        GameObject dmgOutput = Instantiate(DamageOutput);
        MeshRenderer meshRen = dmgOutput.GetComponent<MeshRenderer>();
        meshRen.sortingOrder = counter;
        TextMeshPro textMeshpro = dmgOutput.GetComponent<TextMeshPro>();
        textMeshpro.text = damage.ToString();
        dmgOutput.transform.position = gameObject.transform.position;
    }

}
