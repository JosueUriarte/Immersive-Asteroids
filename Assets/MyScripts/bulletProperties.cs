using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//************** use UnityOSC namespace...
using UnityOSC;
//*************

public class bulletProperties : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject explosion;

    // Update is called once per frame
    void Start()
    {
      //OSCHandler.Instance.SendMessageToClient("pd", "/unity/shoot_laser", 1);
      Destroy(this.gameObject,5);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
      if (collisionInfo.collider.name == "Player_Ship")
      {
        Destroy(this.gameObject, 0.0f);
      }

      if(collisionInfo.collider.name == "enemyBullet(clone)")
      {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/damage_enemy", 1);
        Destroy(this.gameObject, 0.0f);
      }

      else
      {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject, 0.0f);
      }
      
    }
}
