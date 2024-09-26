using UnityEngine;

public class ElectricBall : Skillshot
{
    [SerializeField] private GameObject rotationPoint;

    private void Start()
    {
        skillDamage= offensiveSkillSO.skillDamage;
    }

    private void FixedUpdate()
    {
       rotationPoint.transform.Rotate(new Vector3(0, 0, 1) * skillSpeed * Time.fixedDeltaTime);
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        ShockStun(collision);
    }


}
