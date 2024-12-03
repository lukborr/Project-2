using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public float maxHealth = 100;
    [SerializeField] private GameObject DamageOutput;
    private int counter = 2;
    [SerializeField] EnemySO enemySO;
    [SerializeField] Image image;

    private void OnEnable()
    {
        EventManager.OnPlayerMaxHealthChanged += ChangeMaxHealth;
        EventManager.OnHeal += HealPlayer;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerMaxHealthChanged -= ChangeMaxHealth;
        EventManager.OnHeal -= HealPlayer;
    }
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            health = 90;
            maxHealth *= GlobalStats.health;
        }
        else
        {
            health = enemySO.enemyHealth;
        }
    }

    public void RemoveHealth(int healthToRemove)
    {
        if (gameObject.CompareTag("Enemy"))
        {
            health -= healthToRemove;
            InstantiateDamageOutput(healthToRemove);
        }
        else if (gameObject.CompareTag("Player"))
        {
            int finalHealthtoRemove = healthToRemove - GlobalStats.armor;
            health -= finalHealthtoRemove;
            ChangeHealthBar();
        }

        if (health <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                DieAndDrop();
            }
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

    private void ChangeHealthBar()
    {
        image.fillAmount = health / maxHealth;      
    }

    private void ChangeMaxHealth(float heathPercent)
    {
        maxHealth *= heathPercent;
        if (gameObject.CompareTag("Player"))
        {
            ChangeHealthBar();
        }
    }

    private void HealPlayer(int healthToGain)
    {
        if(health < maxHealth)
        {
            health += healthToGain;           
            if (health > maxHealth)
            {
                health = (int)maxHealth;
            }
            ChangeHealthBar();

        }       
    }
}