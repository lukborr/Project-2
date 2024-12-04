using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightningChain : Skillshot
{
    private float radius = 2f;
    [SerializeField] private GameObject LightningChainBeam;
    SpriteRenderer spriteRenderer;
    float chainSpriteSizeY;
    List<float> list = new List<float>();

    private void Start()
    {
        spriteRenderer = LightningChainBeam.GetComponent<SpriteRenderer>();
        chainSpriteSizeY = spriteRenderer.bounds.size.y;
    }

    private Dictionary<float, Collider2D> GetAllColliders(Collider2D collider)
    {
        Dictionary<float, Collider2D> dict = new Dictionary<float, Collider2D>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(collider.transform.position, radius, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < colliders.Length; i++)
        {
            var distance = Vector2.Distance(colliders[i].transform.position, transform.position);
            dict.Add(distance, colliders[i]);
            list.Add(distance);
            colliders[i].GetComponent<EnemyHealth>().RemoveHealth(skillDamage);
        }
        list.Sort();
        ShockStun(colliders);
        return dict;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            DoTheChainLightning(collision);
        }
    }
  
    private void SpawnBeam(Collider2D[] colliders)
    {
        Collider2D collider1 = colliders[0];
        Collider2D collider2 = colliders[1];
        float distance = Vector2.Distance(collider2.transform.position, collider1.transform.position);
        Vector2 pointBetweenColliders = new Vector2(collider1.transform.position.x + collider2.transform.position.x, collider1.transform.position.y + collider2.transform.position.y) / 2;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, collider1.transform.position - collider2.transform.position);
        GameObject go = Instantiate(LightningChainBeam, pointBetweenColliders, rotation);
        go.transform.localScale = new Vector2(1, distance / chainSpriteSizeY);
    }

    private void AssignPairsAndSpawnBeam(Dictionary<float, Collider2D> dictionary)
    {
        Collider2D[][] colliderPairsArray = new Collider2D[dictionary.Values.Count - 1][];
        int x = 0;
        for (int i = 0; i < colliderPairsArray.Length; i++)                                                     
        {          
            Collider2D[] colliderPair = new Collider2D[2];
            colliderPairsArray[i] = colliderPair;
            for (int y = 0; y < 2; y++)
            {
                float key = list[x + y];
                Collider2D collider = dictionary[key];
                colliderPairsArray[x][y] = collider;              
            }
            x++;
        }
        foreach (Collider2D[] colliderPair in colliderPairsArray)
        {
            SpawnBeam(colliderPair);
        }
    }

    private void DoTheChainLightning(Collider2D collider)
    {
        Dictionary<float, Collider2D> dict = GetAllColliders(collider);
        AssignPairsAndSpawnBeam(dict);
    }

    private void FixedUpdate()
    {
        ProjectileMoveForward();
    }

}




