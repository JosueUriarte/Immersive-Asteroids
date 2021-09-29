using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//************** use UnityOSC namespace...
using UnityOSC;
//*************

public class EnemyControls : MonoBehaviour
{
    //public Gameplay gameplay;
    public float enemySpeed;
    public float lineOfSight;
    public float shootingRange;
    public float fireRate = 1f;

    public float health;

    private float nextFireTime;

    public GameObject bullet;
    public GameObject bulletParent;
    public GameObject explosion;

    private Rigidbody2D rb;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //if(player != null)
        //{
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //}
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;

            float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            if(distanceFromPlayer < lineOfSight && distanceFromPlayer > shootingRange)
            {
                transform.position = Vector2.MoveTowards(this.transform.position,player.position,enemySpeed*Time.deltaTime);
            }

            else if(distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
            {
                OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot_laser", 1);
                Instantiate(bullet,bulletParent.transform.position,Quaternion.identity);
                nextFireTime = Time.time + fireRate;
            }
        }

        if(health <= 0)
        {
            destroyEnemy();
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "asteroid(Clone)")
        {
            destroyEnemy();
        }

        if (collision.gameObject.name == "asteroid(Clone)(Clone)")
        {
            Destroy(collision.gameObject);
            Instantiate(explosion, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/blow_up_rock", 1);
        }

        if (collision.gameObject.name == "asteroid(Clone)(Clone)(Clone)")
        {
            Destroy(collision.gameObject);
            Instantiate(explosion, collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/blow_up_rock", 1);
        }

        if (collision.gameObject.name == "Bullet(Clone)")
        {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/damage_enemy", 1);
            health -= 3;
        }
    }

    void destroyEnemy()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/blow_up", 1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/state", 0);
        Instantiate(explosion, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject, 0.0f);
        //gameplay.EnemyDied();
    }

}
