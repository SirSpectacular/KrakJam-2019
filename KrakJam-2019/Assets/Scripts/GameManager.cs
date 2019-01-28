using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static string firstSceneName = "Home";
    static string secoundSceneName = "Manager";

    public float eventRate;                                 //Will occure 'eventRate' events on avarge
    [Range(0f, 1f)]
    public float eventVariance;
    public const int amountOfEventTypes = 3;
    public float fireProbabilityModifier;
    public float floodProbabilityModifier;
    public float enemySpawnProbabilityModifier;
    private float nextEventTime;

    public int amountOfLocators;
    public int maxLocators;

    private HomeController home;
    private PlayerControler player;

    public float dayLength;
    private float currentTime;
    private bool isDayFinished = false;
    public bool allLocatorsAreMad { set; get; }
    public bool allRoomsDestroyed { set; get; }


    public static GameManager instance = null;

    void Awake() {
        allLocatorsAreMad = false;
        allLocatorsAreMad = false;

        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        initGame();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    static private void OnSceneLoaded(Scene scene,LoadSceneMode loadSceneMode) {
        if(scene.name == "Home") {
            instance.initGame();
        }
        else
            instance.initManagmentPhase();
    }

    void initGame()
    {
        player = GetComponentInChildren<PlayerControler>();
        home = GetComponentInChildren<HomeController>();
        currentTime = 0.0f;
        nextEventTime = genEventTimer(); 
        isDayFinished = false;
        home.prepareRooms(amountOfLocators);
    }

    float genEventTimer() 
    {
        return currentTime + (dayLength / eventRate) * Random.Range(1 - eventVariance, 1 + eventVariance);
    }


    void Start()
    {

    }


    void Update()
    {
        if(!isDayFinished) {

            currentTime += Time.deltaTime;
            if(false) {
                
            }
            else if(currentTime >= nextEventTime) {
                generateEvent();
                nextEventTime = genEventTimer();
            }
        }
    }

    void levelOver() 
    {
        SceneManager.LoadScene(secoundSceneName,LoadSceneMode.Additive);
    }

    void initManagmentPhase() 
    {
        //Ohh jeez
    }

    void generateEvent() { //Never look back
        Room room = home.rooms[Random.Range(0, home.rooms.Length)];
    
        float roll = Random.Range(0,amountOfEventTypes + fireProbabilityModifier + floodProbabilityModifier + enemySpawnProbabilityModifier);
        if(roll < 1 + fireProbabilityModifier)
            room.makeFire();
        else if(roll < 2 + fireProbabilityModifier + floodProbabilityModifier)
            room.makeFlood();
        else
           home.spawnEnemy(room, player);
    }
}
