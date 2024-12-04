using UnityEngine;

public class CometCall : Skillshot
{
    [SerializeField] private GameObject cometObject;
    public Vector2 targetPosition;
    [SerializeField] private GameObject cometExplosion;
    [SerializeField] private GameObject burningGround;
    [SerializeField] private Transform cometPoint;
    private Vector2 offset;
    private float explosionRadius = 1f;

    private void Awake()
    {
        offset = new Vector2(-2, 4);
    }
    private void OnEnable()
    {
        GameObject go = Instantiate(cometObject, (Vector2)transform.position + offset, cometObject.transform.rotation);
        go.GetComponent<Comet>().targetPosition = transform.position;      
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Comet"))
        {
            Instantiate(cometExplosion, transform.position, cometExplosion.transform.rotation);          
           Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
            if(colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].GetComponent<EnemyHealth>().RemoveHealth(skillDamage * 5);
                    Debug.Log(colliders[i].name);
                }
            }          
            Instantiate(burningGround, transform.position, burningGround.transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
