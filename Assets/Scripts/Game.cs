using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public delegate void DelGame(Game game);
	public DelGame evntPlayerScoreChangd;
	public DelGame evntGameOver;

	public enum GameState {PAUSED,PLAYING,GAME_OVER}
	public Platform PREFAB_PLATFORM;
	public Coin PREFAB_COIN;
	public Spring PREFAB_SPRING;
	public Player player;
	public GameObject killLine;

	public AudioSource adoSrcScore;

	public int score = 0;

	GameState state = GameState.PAUSED;
	public GameState State {
		set
		{
			this.state = value;
			switch (state)
			{
				case GameState.PAUSED:
					break;
				case GameState.PLAYING:
					break;
				case GameState.GAME_OVER:
					break;
				default:
					break;
			}
		}
		get
		{
			return state;
		}
	}


	List<Platform> platforms = new List<Platform>();
	List<Coin> coins = new List<Coin>();
	List<Spring> springs = new List<Spring>();

	int platformIndex = 0;
	int coinIndex = 0;
	int springIndex=0;

	int climbedFloor = 0;
	float raisedFloor = -3;
	public float floorRaisingSpeed = 1.0f;
	public bool isGameOver = false;


	public void startGame()
	{
		foreach (var instant in platforms) instant.gameObject.SetActive(false);
		foreach (var instant in coins) instant.gameObject.SetActive(false);
		foreach (var instant in springs) instant.gameObject.SetActive(false);

		player.reset();
		
		isGameOver = false;
		climbedFloor = 0;
		raisedFloor = -3;
		state = GameState.PLAYING;
		score = 0;
	}
	// Start is called before the first frame update
	void Start()
    {
		int PLATFORM_COUNT = 5;
		for (int i = 0; i < PLATFORM_COUNT; i++)
		{
			platforms.Add(Instantiate(PREFAB_PLATFORM));
		}
		for (int i = 0; i < PLATFORM_COUNT; i++)
		{
			coins.Add(Instantiate(PREFAB_COIN));
		}
		for (int i = 0; i < PLATFORM_COUNT; i++)
		{
			springs.Add(Instantiate(PREFAB_SPRING));
		}
		foreach (var instant in platforms) instant.gameObject.SetActive(false);
		foreach (var instant in coins) instant.gameObject.SetActive(false);
		foreach (var instant in springs) instant.gameObject.SetActive(false);
		for (int i = 0; i < coins.Count; i++)
			coins[i].evntTriggered = hdlCoinTriggered;
		for (int i = 0; i < springs.Count; i++)
			springs[i].evntTriggered = hdlSpringActivated;
	}
	Platform getNextPlatform()
	{
		platformIndex = (platformIndex + 1) % platforms.Count;
		platforms[platformIndex].reset();
		return platforms[platformIndex];
	}
	Coin getNextCoin()
	{
		coinIndex = (coinIndex + 1) % coins.Count;
		coins[coinIndex].reset();
		return coins[coinIndex];
	}
	Spring getNextSpring()
	{
		springIndex = (springIndex + 1) % springs.Count;
		springs[springIndex].reset();
		return springs[springIndex];
	}
	void spawnCoin()
	{
		var coin = getNextCoin();
		int coinLocation = Random.Range(1, 10);
		coin.setValue(Random.Range(0, 3));
		coin.transform.position = new Vector3(coinLocation, climbedFloor, 0);
	}
	void raiseFloor()
	{
		climbedFloor++;
		if(climbedFloor > 5 && climbedFloor % 5 == 2)
		{
			//third floor has a coin
			spawnCoin();
		}
		if (climbedFloor % 5 != 0) return;
		//platfrom is either size of 2 or 4
		int platformSize = (Random.Range(0, 2) == 0) ? 2 : 4;
		bool isBroken = (Random.Range(0, 5)==0)? true : false;
		int chosenPlatformLocation = Random.Range(platformSize, 10 - platformSize/2+1);
		var platform = getNextPlatform();
		platform.transform.position = new Vector3(chosenPlatformLocation, climbedFloor, 0);
		platform.init((platformSize == 2)?Platform.PlatformSize.SMALL:Platform.PlatformSize.BIG, isBroken);

		//10% chance to spawn a spring
		if(Random.Range(0,10) < 2)
		{
			var spring = getNextSpring();
			spring.transform.position = new Vector3(chosenPlatformLocation, climbedFloor+0.8f, 0);
		}
		
	}

    // Update is called once per frame
    void Update()
    {
		switch (state)
		{
			case GameState.PAUSED:
				break;
			case GameState.PLAYING:
				while (climbedFloor < player.Y + 15) raiseFloor();
				raisedFloor += floorRaisingSpeed * Time.deltaTime;
				if(raisedFloor < player.Y - 10)
				{
					raisedFloor = player.Y - 10;
				}
				killLine.transform.position = new Vector3(5, raisedFloor, 0);
				floorRaisingSpeed = climbedFloor * 0.02f;
				if (player.Y < raisedFloor)
				{
					gameOver();
				}
				break;
			case GameState.GAME_OVER:
				break;
			default:
				break;
		}
	}
	void gameOver()
	{
		State = GameState.GAME_OVER;
		isGameOver = true;
		player.kill();

		if (evntGameOver != null) evntGameOver(this);
	}

	void hdlCoinTriggered(Coin coin)
	{
		score += 1+coin.value;
		coin.transform.position = new Vector3(0,-10,0);
		adoSrcScore.Play();
		if (evntPlayerScoreChangd != null) evntPlayerScoreChangd(this);
	}
	void hdlSpringActivated(Spring spring)
	{
		player.superJump();
	}
}
