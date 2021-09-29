using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//************** use UnityOSC namespace...
using UnityOSC;
//*************

public class SpawningEnemy : MonoBehaviour
{
    //public Gameplay gameplay;
    public GameObject enemyShip;
    public Transform[] spawnPoints;
    GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckTheEnemy", 10.0f, 30.0f);
    }

    void SpawnTheEnemy(){
        int spawnInt = Random.Range(0, spawnPoints.Length);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/state", 1);
        Instantiate(enemyShip, spawnPoints[spawnInt].transform.position, spawnPoints[spawnInt].transform.rotation);
    }

    void CheckTheEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        print("checking......");
        if(enemies.Length == 0)
        {
            SpawnTheEnemy();
        }
    }

    IEnumerator WaitCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        print("waiting.....");
        
        yield return new WaitUntil(() => enemies.Length < 1);
        //print(enemies.Length);
        
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

}
