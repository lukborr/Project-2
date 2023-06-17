using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightningBall : Skillshot
{
    private float radius = 2f;
    [SerializeField] private GameObject LightningChain;
    SpriteRenderer spriteRenderer;
    float chainSpriteSizeY;
    List<float> list = new List<float>();

    private void Start()
    {
        spriteRenderer = LightningChain.GetComponent<SpriteRenderer>();
        chainSpriteSizeY = spriteRenderer.bounds.size.y;
    }

    private Dictionary<float, Collider2D> GetAllColliders()
    {
        Dictionary<float, Collider2D> dict = new Dictionary<float, Collider2D>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < colliders.Length; i++)
        {
            var distance = Vector2.Distance(colliders[i].transform.position, transform.position);
            dict.Add(distance, colliders[i]);
            list.Add(distance);
        }
        list.Sort();
        return dict;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    protected override  void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            DoTheChainLightning();
        }
    }
  
    private void SpawnBeam(Collider2D[] colliders)
    {
        Collider2D collider1 = colliders[0];
        Collider2D collider2 = colliders[1];
        float distance = Vector2.Distance(collider2.transform.position, collider1.transform.position);
        Vector2 pointBetweenColliders = new Vector2(collider1.transform.position.x + collider2.transform.position.x, collider1.transform.position.y + collider2.transform.position.y) / 2;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, collider1.transform.position - collider2.transform.position);
        GameObject go = Instantiate(LightningChain, pointBetweenColliders, rotation);
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

    private void DoTheChainLightning()
    {
        Dictionary<float, Collider2D> dict = GetAllColliders();
        AssignPairsAndSpawnBeam(dict);
    }

}




