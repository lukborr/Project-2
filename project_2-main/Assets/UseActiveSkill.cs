using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseActiveSkill : MonoBehaviour
{
   [SerializeField] private GameObject activeProjectile;
   [SerializeField] private GameObject handGameobject;
    Quaternion handRotation;

    private void Update()
    {
        handRotation = handGameobject.transform.rotation * Quaternion.Euler(0, 0, 45);
    }

    private void OnEnable()
    {
        EventManager.MouseButton0 += SpawnProjectile;
    }
    private void OnDisable()
    {
        EventManager.MouseButton0 -= SpawnProjectile;
    }
    private void SpawnProjectile()
    {
        Instantiate(activeProjectile, handGameobject.transform.position, handRotation);
    }
}
