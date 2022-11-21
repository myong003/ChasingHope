using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : Piece
{
    // [Header("Stats")]
    // public int block, hopeCost;
    // public float cooldown;
    // public float range = 1f;
    // private float attackCountdown = 1f;

    // private GameObject currentTarget;

    // protected override void Start() 
    // {
    //     base.Start();
    //     InvokeRepeating("UpdateTarget", 0f, 0.5f);
    // }

    // void UpdateTarget()
    // {
    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //     GameObject king = GameObject.FindGameObjectWithTag("Player");
    //     float shortestDistance = Mathf.Infinity;
    //     GameObject nearestEnemy = null;

    //     foreach (GameObject enemy in enemies)
    //     {
    //         float distanceToEnemy = Vector3.Distance (king.transform.position, enemy.transform.position);
    //         if (distanceToEnemy < shortestDistance)
    //         {
    //             shortestDistance = distanceToEnemy;
    //             nearestEnemy = enemy;
    //         }
    //     }

    //     if (nearestEnemy != null && shortestDistance <= range)
    //     {
    //         currentTarget = nearestEnemy;
    //     }
    //     else 
    //     {
    //         currentTarget = null;
    //     }
    // }

    // protected override void Update()
    // {
    //     base.Update();
    //     if (currentTarget == null)
    //         return;

    //     if (attackCountdown <= 0f)
    //     {
    //         Attack();
    //         attackCountdown = 1f;
    //     }

    //     attackCountdown -= Time.deltaTime;

    // }

    // void Attack()
    // {
    //     // currentTarget.currHealth -= (attack - currentTarget.defense);
    // } 
}
