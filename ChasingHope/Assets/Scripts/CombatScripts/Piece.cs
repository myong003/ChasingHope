using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int coord;
    public Player player;

    [Header("Stats")]
    public int block;
    public int hopeCost;
    public float cooldown;
    public float range = 1.5f;
    public float maxHealth = 20f;
    public float attack = 10f;
    public float defense = 5f;

    private float attackCountdown = 0f;
    private float currHealth;
    private List<EnemyPiece> enemiesBlocked;

    protected virtual void Awake()
    {
        currHealth = maxHealth;
        UpdateCoords();
    }

    protected virtual void Start() 
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        enemiesBlocked = new List<EnemyPiece>();
    }

    public void UpdateCoords() 
    {
        int newX = Mathf.RoundToInt(this.gameObject.transform.position.x);
        int newY = Mathf.RoundToInt(this.gameObject.transform.position.y);
        coord = new Vector2Int(newX, newY);
    }

    public void PlacePiece()
    {
        player.reduceHope(hopeCost);
        Debug.Log("Piece placed. Current Hope: " + player.currHope + "/" + player.maxHope);
    }

    public bool CanPlace()
    {
        if (hopeCost > player.currHope)
        {
            return false;
        }
        return true;
    }

    protected virtual void Update() 
    {
        if (currHealth <= 0f)
            Destroy(gameObject);

        if (currentTarget == null)
            return;

        if (attackCountdown <= 0f)
        {
            Attack();
            attackCountdown = 1f;
        }

        attackCountdown -= Time.deltaTime;
    }

    public GameObject currentTarget;

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        float distanceToEnemy = shortestDistance;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToKing = Vector3.Distance(GameObject.FindGameObjectWithTag("King").transform.position, enemy.transform.position);
            if (distanceToKing < shortestDistance)
            {
                shortestDistance = distanceToKing;
                nearestEnemy = enemy;
                distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            }
        }

        if (nearestEnemy != null && distanceToEnemy <= range)
        {
            currentTarget = nearestEnemy;
        }
        else 
        {
            currentTarget = null;
        }
    }

    void Attack()
    {
        // Destroy(currentTarget.gameObject);
        currentTarget.GetComponent<EnemyPiece>().TakeDamage(this.attack);
    } 

    void TakeDamage(float rawDamage)
    {
        this.currHealth -= rawDamage - this.defense;
        // if (this.currHealth <= 0)
        //     // Debug.Log("dead");
        // else
        //     // Debug.Log("Health Remaining: " + this.currHealth);
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Enemy" && enemiesBlocked.Count < block) {
            EnemyPiece enemy;
            if (coll.gameObject.TryGetComponent<EnemyPiece>(out enemy)) {
                enemy.isFrozen = true;
                enemiesBlocked.Add(enemy);
            }
        }
    }
}
