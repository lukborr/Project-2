using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningChainBeam : MonoBehaviour
{
    [SerializeField] Transform startFigure;
    [SerializeField] Transform endFigure;
    private SpriteRenderer spr;

    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        var dist = Vector2.Distance(endFigure.position, startFigure.position);
        transform.localScale = new Vector2(1, dist / spr.bounds.size.y);

        Vector2 center = new Vector2(startFigure.position.x + endFigure.position.x, startFigure.position.y + endFigure.position.y ) / 2;

        var rotation = Quaternion.FromToRotation(Vector2.up, endFigure.position - startFigure.position);
        transform.position= center;
        transform.rotation= rotation;
    }
 
}
