using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyLogic : MonoBehaviour
{
    // Variables

    // Player variables
    public PlayerMovement pm;
    public GameObject player;

    // Enemy specific variables
    public Transform currentTarget;
    public int health;
    public int damage;
    public int speed;
    public bool canMove = true;
    public bool canAttack;
    public float attackTimer;
    public float attackCooldown;

    public List<int> resources;
    public int money;

    public int range;
    public LayerMask goodLayer;
    public SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("PlayerSprite");
        currentTarget = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            attackTimer -= Time.deltaTime;

            Transform newTarget = getNewTarget();
            if(newTarget == null)
            {
                currentTarget = player.transform;
            } else
            {
                currentTarget = newTarget;
            }

            float curDistance = Vector3.Distance(transform.position, currentTarget.position);

            if(curDistance > 2)
            {
                float py = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, py, transform.position.z);
            } else
            {
                if(attackTimer <= 0)
                {
                    attack();
                    attackTimer = attackCooldown;
                }
            }

            if(player.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true;
            } else
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    void attack()
    {
        if(currentTarget.tag == "player")
        {
            player.GetComponent<PlayerMovement>().health -= damage;
        } else
        {
            currentTarget.gameObject.GetComponent<PlaceableWrapper>().health -= damage;

            if(currentTarget.gameObject.GetComponent<PlaceableWrapper>().health <= 0)
            {
                Destroy(currentTarget.gameObject);
            }
        }
    }

    Transform getNewTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, goodLayer);

        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach(Collider col in hitColliders)
        {
            Vector3 directionToTarget = col.transform.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = col.transform;
            }
        }

        return closestEnemy;
    }

    public void OnTriggerEnter(UnityEngine.Collider col)
    {
        if(col.gameObject.tag == "Bullet")
        {
            health -= col.gameObject.GetComponent<BulletMove>().damage;
            Destroy(col.gameObject);

            if(health <= 0)
            {
                for(int i = 0; i < 3; i++)
                {
                    GameData.Instance.resources[i] += resources[i];
                    GameData.Instance.money += money;
                }

                player.GetComponent<PlayerMovement>().updateInventory();
                Destroy(gameObject);
            }
        }
    }
}
