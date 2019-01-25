using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EventController : MonoBehaviour
{

    static string firstSceneName = "Home";
    static string secoundSceneName = "Manager";

    public float eventRate;                                 //Will occure 'eventRate' events on avarge
    [Range(0f, 1f)]
    public float eventVariance;
    public const int amountOfEventTypes = 3;
    public float fireProbabilityModifier { get; set; }
    public float floodProbabilityModifier { set; get; }
    public float enemySpawnProbabilityModifier { set; get; }
    private float nextEventTime;


    private HomeController home;

    public float dayLength;
    private float currentTime;
    private bool isDayFinished = false;

    public static EventController instance = null;

    void Awake() {
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
        currentTime = 0.0f;
        nextEventTime = genEventTimer(); 
        isDayFinished = false;
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
            if(currentTime >= dayLength) {
                levelOver();
            }
            else if(currentTime >= nextEventTime) {
                generateEvent();

            }
        }
        else {

        }
    }

    void levelOver() 
    {
        SceneManager.LoadScene(secoundSceneName,LoadSceneMode.Additive);
    }

    void initManagmentPhase() 
    {
        
    }

    void generateEvent() { //Never look back
        RoomController room = home.rooms[Random.Range(0, home.rooms.Length)];
        float roll = Random.Range(0,amountOfEventTypes + fireProbabilityModifier + floodProbabilityModifier + enemySpawnProbabilityModifier);
        if(roll < 1 + fireProbabilityModifier)
            room.makeFire();
        else if(roll < 2 + fireProbabilityModifier + floodProbabilityModifier)
            room.makeFlood();
        else
           room.spawnEnemy();
    }
}
