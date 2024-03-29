﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [HideInInspector] public int health;
    [SerializeField] private GameObject DamageOutput;
    private int counter = 2;
    [SerializeField] EnemySO enemySO;
    [SerializeField] Image image;


    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            health = 100;
        }
        else
        {
            health = enemySO.enemyHealth;
        }

    }

    public void RemoveHealth(int healthToRemove)
    {
        health -= healthToRemove;
        if (gameObject.CompareTag("Enemy"))
        {
            InstantiateDamageOutput(healthToRemove);
        }
        else if (gameObject.CompareTag("Player"))
        {
            ChangeHealthBar();
        }
        Debug.Log("remaining health is " + health);

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
        image.fillAmount = health / 100f;
    }



}
