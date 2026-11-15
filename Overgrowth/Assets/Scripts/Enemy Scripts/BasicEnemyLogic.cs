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
}
