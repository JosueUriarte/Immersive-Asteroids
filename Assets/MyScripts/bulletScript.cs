using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public Transform firePoint;
	public GameObject bulletPrefab;
    public float bulletForce = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    	    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/player_laser", 1);
    	    rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        }

        //if (!GetComponent<Renderer>().isVisible){
    		//Destroy(this.gameObject);
		//}
    }
}
