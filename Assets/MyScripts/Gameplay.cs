/*

   .-------.                             .--.    .-------.     .--.            .--.     .--.        
   |       |--.--.--------.-----.-----.--|  |    |_     _|--.--|  |_.-----.----|__|---.-|  |-----.
   |   -   |_   _|        |  _  |     |  _  |      |   | |  |  |   _|  _  |   _|  |  _  |  |__ --|
   |_______|__.__|__|__|__|_____|__|__|_____|      |___| |_____|____|_____|__| |__|___._|__|_____|
   © OXMOND / www.oxmond.com 

*/
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//************** use UnityOSC namespace...
using UnityOSC;
//*************


public class Gameplay : MonoBehaviour
{
    [SerializeField] FlashImage _flashImage = null;
    [SerializeField] Color _newColor = Color.red;

    public GameObject asteroid;
    public GameObject rocket;
    //public GameObject enemyRocket;

    public GameObject explosion;

    private int _startLevelAsteroidsNum;
    private bool _allAsteroidsOffScreen;
    private int levelAsteroidNum;
    private Camera mainCam;
    private int asteroidLife;

    public Text countText;

    public GameObject retryButton;

    //************* Need to setup this server dictionary...
	Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog> ();
	//*************

    private void Start()
    {
        retryButton.SetActive(false);
        asteroid.SetActive(false);
        mainCam = Camera.main;
        _startLevelAsteroidsNum = 2;
        CreateAsteroids(_startLevelAsteroidsNum);

        Application.runInBackground = true; //allows unity to update when not in focuss

		//************* Instantiate the OSC Handler...
	    OSCHandler.Instance.Init ();
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start", 1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/state", 0);
        //OSCHandler.Instance.SendMessageToClient("pd", "/unity/lvl_0", 1);
        //*************
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.8f);

        //print(asteroidLife);
        
        if (asteroidLife <= 0)
        {
            asteroidLife = 6;
            CreateAsteroids(1);
        }
        
        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;
        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        _allAsteroidsOffScreen = true;
        
    }

    void FixedUpdate()
	{
        checkState();
        int result = -1;
		//************* Routine for receiving the OSC...
		OSCHandler.Instance.UpdateLogs();
		Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
		servers = OSCHandler.Instance.Servers;

		foreach (KeyValuePair<string, ServerLog> item in servers) {
			// If we have received at least one packet,
			// show the last received from the log in the Debug console
			if (item.Value.log.Count > 0) {
				int lastPacketIndex = item.Value.packets.Count - 1;

				//get address and data packet
				countText.text = item.Value.packets [lastPacketIndex].Address.ToString ();
				countText.text += item.Value.packets [lastPacketIndex].Data [0].ToString ();

			}
		}
		//*************

        if(countText.text.Contains("0") || countText.text.Contains("1"))
        {
            bool boli = int.TryParse((countText.text).Substring(11), out result);
            //print(result);
        }

        if(result == 1)
        {
            _flashImage.Flash();
        }
        
	}

    void checkState(){
	    // if (globalArray.Count == 0 && level != 0){
	    //     OSCHandler.Instance.SendMessageToClient("pd", "/unity/lvl_0", 1);
	    //     level = 0;
	    // }
	}

    void OnApplicationQuit() {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start", 0);
    }

    void Awake () {
	    QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    }

    private void CreateAsteroids(float asteroidsNum)
    {
        for (int i = 1; i <= asteroidsNum; i++)
        {
            GameObject AsteroidClone = Instantiate(asteroid, new Vector2(Random.Range(-10, 10), 6f), transform.rotation);
            AsteroidClone.GetComponent<Asteroid>().SetGeneration(1);
            AsteroidClone.SetActive(true);
        }
    }

    public void RocketFail()
    {
        Cursor.visible = true;
        retryButton.SetActive(true);
        Instantiate(explosion, rocket.transform.position, transform.rotation);
        Destroy(rocket);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/blow_up", 1);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/start", 0);
        print("GAME OVER");
    }

    public void EnemyDied()
    {
        _flashImage.StopFlashLoop();
    }

    public void asterodDestroyed()
    {
        asteroidLife--;
    }

    public int startLevelAsteroidsNum
    {
        get { return _startLevelAsteroidsNum; }
    }

    public bool allAsteroidsOffScreen
    {
        get { return _allAsteroidsOffScreen; }
    }

}