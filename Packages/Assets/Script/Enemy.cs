﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //EnemyMovement
    private Animator animator;
    public float speed;
    private Transform target;
    protected Vector2 direction;

    //EnemyAttack
    [SerializeField]
    GameObject bullet;
    public float firingRate;
    public float nextShot;
    public float timer;

    //RandomWalk Script
    EnemyRandomWalk EnemyRngWalk;

    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    int enemyHP = 5;
    //Getter and setter to get players position
    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        firingRate = 1f;
        nextShot = Time.time;
        EnemyRngWalk = GetComponent<EnemyRandomWalk>();

        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        animatedDirection(direction);
        timer += Time.deltaTime;

        if (target != null)
        {
            //direction = Vector2.zero;
            RunFromTarget();
        }
        else if (target == null)
        {
           EnemyRngWalk.FollowRandomDirection();
        }
	}

    //Set animation for all 4 direction using animator and variable
    private void animatedDirection(Vector2 direction)
    {
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }

    //When the player is collided with the enemy it will run the opposite way
    private void RunFromTarget()
    {
        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
            
            rb2d.velocity = -(direction)*speed;
            //transform.position = Vector2.MoveTowards(transform.position, target.position,-1* speed * Time.deltaTime);
            if (timer >= 3f)
            {
                attacking();
                timer = 0;
            }
        }
    }

    //Attack at a firing rate
    private void attacking()
    {
        if(Time.time > nextShot)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextShot = Time.time + firingRate;
        }
    }

    public int getEnemyHP()
    {
        return enemyHP;
    }
    public void reduceEnemyHP()
    {
        
        reduceEnemyHP (1);
    }

    public void reduceEnemyHP(int value){
        enemyHP -= value;
        if(enemyHP <=0)
        Die();
    }
    public void Die(){
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer ==9)//alice tools
        {
            reduceEnemyHP();
        }
    }
}
