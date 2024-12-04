using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public float maxHealth = 100;
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
        health = 90;
        maxHealth *= GlobalStats.health;
    }

    public void RemoveHealth(int healthToRemove)
    {

        int finalHealthtoRemove = healthToRemove - GlobalStats.armor;
        health -= finalHealthtoRemove;
        ChangeHealthBar();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void DieAndDrop()
    {
        Transform childTransform = transform.GetChild(0);
        childTransform.gameObject.SetActive(true);
        childTransform.transform.parent = null;
        Destroy(gameObject);
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
        if (health < maxHealth)
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
