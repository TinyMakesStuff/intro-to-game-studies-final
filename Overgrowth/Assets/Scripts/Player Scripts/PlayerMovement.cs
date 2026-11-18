using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Vars
    public bool canMove = true;
    public bool canAttack = true;
    public bool canBuild = true;
    public int health = 100;
    public int maxHealth = 100;
    public int speed = 10;
    public int damage = 2;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody rb;

    // Attacking specific vars
    public float attackCoolDown = 2f;
    public float coolDownTimer = 0;
    public int range = 10;
    public LayerMask enemyLayer;
    public LayerMask goodLayer;
    public GameObject bullet;

    // Hotbar and Building vars
    public int selected = 0;
    public List<UnityEngine.UI.Image> hotbarIcons;
    public List<GameObject> placeables;
    public Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameData.Instance.startGame();
        GameData.Instance.addPlant(0, placeables[1].GetComponent<PlaceableWrapper>().plantData);
        RoundManager.Instance.startRound();
    }

    void FixedUpdate()
    {
        if(canMove) {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");

            if(hor == 0 && ver == 0)
            {
                animator.SetBool("IsWalking", false);
            } else
            {
                animator.SetBool("IsWalking", true);

                if(hor < 0)
                {
                    spriteRenderer.flipX = false;
                } else
                {
                    spriteRenderer.flipX = true;
                }
            }

            Vector3 move = new Vector3(hor, 0, ver);
            // transform.Translate(move * -speed * Time.deltaTime);
            // rb.MovePosition(move * -speed * Time.deltaTime);
            rb.linearVelocity = (move * -speed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if(canAttack)
        {
            coolDownTimer -= Time.deltaTime;
            if(coolDownTimer <= 0f)
            {
                Transform target = getClosestEnemy();
                if(target != null)
                {
                    attack(target);
                    coolDownTimer = attackCoolDown;
                }
            }
        }

        if(canBuild)
        {
            moveHotbar();
            place();
        }
    }

    // ATTACKING FUNCTIONS
    Transform getClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, enemyLayer);

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

    void attack(Transform target)
    {
        // Debug.Log(target);
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.position = transform.position;

        newBullet.GetComponent<BulletMove>().target = target;
        newBullet.GetComponent<BulletMove>().life = 1f;
        newBullet.GetComponent<BulletMove>().damage = damage;
    }

    // BUILDING FUNTIONS
    void moveHotbar()
    {
        if(Mouse.current.scroll.ReadValue().y > 0)
        {
            hotbarIcons[selected].color = new Color(hotbarIcons[selected].color.r, hotbarIcons[selected].color.b, hotbarIcons[selected].color.g, 0.5f);
            selected++;

            if(selected > 1)
            {
                selected = 0;
            }

            hotbarIcons[selected].color = new Color(hotbarIcons[selected].color.r, hotbarIcons[selected].color.b, hotbarIcons[selected].color.g, 1);
        }

        if(Mouse.current.scroll.ReadValue().y < 0)
        {
            hotbarIcons[selected].color = new Color(hotbarIcons[selected].color.r, hotbarIcons[selected].color.b, hotbarIcons[selected].color.g, 0.5f);
            selected--;

            if(selected < 0)
            {
                selected = 1;
            }

            hotbarIcons[selected].color = new Color(hotbarIcons[selected].color.r, hotbarIcons[selected].color.b, hotbarIcons[selected].color.g, 1);
        }
    }

    void place()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, math.INFINITY, ~goodLayer))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("HIT");
                
                if(clickedObject.tag == "clickable")
                {
                    Vector3 position = hit.point;
                    Placeable on = placeables[selected].GetComponent<PlaceableWrapper>().placeable;
                    
                    
                    if(on.type == "turret")
                    {
                        if(on.cost[0] <= GameData.Instance.resources[0] && on.cost[1] <= GameData.Instance.resources[1] && on.cost[2] <= GameData.Instance.resources[2])
                        {
                            for(int i = 0; i < 3; i++)
                            {
                                GameData.Instance.resources[i] -= on.cost[i];
                            }

                            GameObject newTurret = GameObject.Instantiate(placeables[selected]);
                            newTurret.transform.position = new Vector3(position.x, 1, position.z);
                        }
                    } else
                    {
                        if(GameData.Instance.plantCount[on.cost[0]] > 0)
                        {
                            PlantData pd = placeables[selected].GetComponent<PlaceableWrapper>().plantData;

                            // Remove personal buff, place plant
                            GameData.Instance.removePlant(pd.index, pd);
                            GameObject newPlant = GameObject.Instantiate(placeables[selected]);
                            newPlant.transform.position = new Vector3(position.x, 1, position.z);
                        }
                    }
                }
            }
        }
    }
}
