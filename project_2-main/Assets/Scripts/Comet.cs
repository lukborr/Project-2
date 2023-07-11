using UnityEngine;

public class Comet : MonoBehaviour
{
    public Vector2 targetPosition;
    [SerializeField] private GameObject cometExplosion;
    [SerializeField] private GameObject burningGround;
    [SerializeField] private Transform cometPoint;
    private float speed = 8f;
    private Vector2 offset;

    private void Start()
    {
        offset = new Vector2(-0.1f, 0.4f);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CometCall"))
        {
            Vector2 position = (Vector2)cometPoint.position + offset;
            Instantiate(cometExplosion, position, cometExplosion.transform.rotation);
            Instantiate(burningGround, position, burningGround.transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
