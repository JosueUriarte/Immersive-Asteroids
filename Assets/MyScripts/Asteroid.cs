/*

   .-------.                             .--.    .-------.     .--.            .--.     .--.        
   |       |--.--.--------.-----.-----.--|  |    |_     _|--.--|  |_.-----.----|__|---.-|  |-----.
   |   -   |_   _|        |  _  |     |  _  |      |   | |  |  |   _|  _  |   _|  |  _  |  |__ --|
   |_______|__.__|__|__|__|_____|__|__|_____|      |___| |_____|____|_____|__| |__|___._|__|_____|
   © OXMOND / www.oxmond.com 

*/

using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public GameObject rock;
    public Gameplay gameplay;
    private float maxRotation;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    private Rigidbody2D rb;
    private Camera mainCam;
    private float maxSpeed;
    private int _generation;

    public float screenTOP;
    public float screenBOTTOM;
    public float screenLEFT;
    public float screenRIGHT;


    void Start()
    {

        mainCam = Camera.main;

        maxRotation = 10f;
        rotationZ = Random.Range(-maxRotation, maxRotation);

        rb = rock.GetComponent<Rigidbody2D>();

        float speedX = Random.Range(200f, 800f);
        int selectorX = Random.Range(0, 2);
        float dirX = 0;
        if (selectorX == 1) { dirX = -1; }
        else { dirX = 1; }
        float finalSpeedX = speedX * dirX;
        rb.AddForce(transform.right * finalSpeedX);

        float speedY = Random.Range(200f, 800f);
        int selectorY = Random.Range(0, 2);
        float dirY = 0;
        if (selectorY == 1) { dirY = -1; }
        else { dirY = 1; }
        float finalSpeedY = speedY * dirY;
        rb.AddForce(transform.up * finalSpeedY);

    }

    public void SetGeneration(int generation)
    {
        _generation = generation;
    }

    void Update()
    {
        rock.transform.Rotate(new Vector3(rotationX, rotationY, rotationZ) * Time.deltaTime);
        CheckPosition();
        float dynamicMaxSpeed = 0.8f;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -dynamicMaxSpeed, dynamicMaxSpeed), Mathf.Clamp(rb.velocity.y, -dynamicMaxSpeed, dynamicMaxSpeed));
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.name == "Bullet(Clone)")
        {
            if (_generation < 3)
            {
                CreateSmallAsteriods(2);
            }
            Destroy();
        }

        if (collisionInfo.collider.name == "Player_Ship")
        {
            gameplay.RocketFail();
        }
    }

    void CreateSmallAsteriods(int asteroidsNum)
    {
        int newGeneration = _generation + 1;
        for (int i = 1; i <= asteroidsNum; i++)
        {
            float scaleSize = 0.5f;
            GameObject AsteroidClone = Instantiate(rock, new Vector3(transform.position.x, transform.position.y, 0f), transform.rotation);
            AsteroidClone.transform.localScale = new Vector3(AsteroidClone.transform.localScale.x * scaleSize, AsteroidClone.transform.localScale.y * scaleSize, AsteroidClone.transform.localScale.z * scaleSize);
            AsteroidClone.GetComponent<Asteroid>().SetGeneration(newGeneration);
            AsteroidClone.SetActive(true);
        }
    }

    private void CheckPosition()
    {

        float rockOffset = 1;

        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;

        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        if (rock.transform.position.x > sceneRightEdge + rockOffset)
        {
            rock.transform.position = new Vector2(sceneLeftEdge - rockOffset, rock.transform.position.y);
        }

        if (rock.transform.position.x < sceneLeftEdge - rockOffset)
        {
            rock.transform.position = new Vector2(sceneRightEdge + rockOffset, rock.transform.position.y);
        }

        if (rock.transform.position.y > sceneTopEdge + rockOffset)
        {
            rock.transform.position = new Vector2(rock.transform.position.x, sceneBottomEdge - rockOffset);
        }

        if (rock.transform.position.y < sceneBottomEdge - rockOffset)
        {
            rock.transform.position = new Vector2(rock.transform.position.x, sceneTopEdge + rockOffset);
        }
    }

    public void Destroy()
    {
        //print("DESTROYED");
        gameplay.asterodDestroyed();
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/blow_up_rock", 1);
        Destroy(gameObject, 0.01f);
    }

    public void DestroySilent()
    {
        Destroy(gameObject, 0.00f);
    }

}