using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class LightningBall : MonoBehaviour
{
    private float radius = 2f;
  [SerializeField] private GameObject LightningChain;
    SpriteRenderer spriteRenderer;
    float chainSpriteSizeY;
    List<float> list= new List<float>();

    private void Start()
    {
        spriteRenderer= LightningChain.GetComponent<SpriteRenderer>();
        chainSpriteSizeY = spriteRenderer.bounds.size.y;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
           
            DoTheThing();
        }
    }

    private Dictionary<float , Collider2D> GetAllColliders()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GetAllColliders(); 
    }

    private void SpawnBeam(Collider2D[] colliders)
    {

        Collider2D collider1 =  colliders[0];
        Collider2D collider2 = colliders[1];
        float distance =  Vector2.Distance(collider2.transform.position, collider1.transform.position);
        Vector2 pointBetweenColliders = new Vector2(collider1.transform.position.x + collider2.transform.position.x, collider1.transform.position.y + collider2.transform.position.y) / 2;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, collider1.transform.position - collider2.transform.position);
      GameObject go = Instantiate(LightningChain, pointBetweenColliders, rotation);

  
        go.transform.localScale = new Vector2(1, distance / chainSpriteSizeY);
    }

    private Collider2D[] GetColliderPair(Dictionary<float,Collider2D> dictionary, int x)
    {

        Collider2D[] colliders = new Collider2D[2];
        for (int i = x; i < x +2 ; i++)
        {
            float key = list[i];

            colliders[i] = dictionary[key];

        }
        return colliders;
    }

    private Collider2D[][] AssignPairs(Dictionary<float, Collider2D> dictionary)
    {
        Collider2D[][] arrayCollidersArray = new Collider2D[dictionary.Values.Count / 2][];
        
        for (int i = 0; i < arrayCollidersArray.Length; i++)            // tu cos jest nie tak bo for sie tylko raz odpala
        {
            Collider2D[] colliderPair =  GetColliderPair(dictionary, i);
            arrayCollidersArray[i] = colliderPair;
            Debug.Log("raz");
        }

        return arrayCollidersArray;
    }

    private void DoTheThing()
    {
       Dictionary<float,Collider2D> dict = GetAllColliders();
        AssignPairs(dict);

    }
    

    }
  



