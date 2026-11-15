using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyLogic : MonoBehaviour
{
    // Variables

    // Player variables
    public PlayerMovement pm;
    public GameObject player;

    // Enemy specific variables
    public GameObject currentTarget;
    public int health;
    public int damage;
    public int speed;
    public bool canMove = true;
    public bool canAttack;

    public List<int> resources;
    public int money;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("PlayerSprite");
        currentTarget = player;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);
        }
    }

    void attack()
    {
        
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

                Destroy(gameObject);
            }
        }
    }
}
