using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public Platform PREFAB_PLATFORM;
	public Coin PREFAB_COIN;
	public Spring PREFAB_SPRING;
	public Player player;
	List<Platform> platforms = new List<Platform>();
	List<Coin> coins = new List<Coin>();
	List<Spring> springs = new List<Spring>();

	int platformIndex = 0;
	int coinIndex = 0;
	int springIndex=0;

	int climbedFloor = 0;
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
		coin.animator.SetInteger("value", Random.Range(0, 3));
		coin.transform.position = new Vector3(coinLocation, climbedFloor, 0);
	}
	void raiseFloor()
	{
		climbedFloor++;
		if(climbedFloor % 5 == 2)
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
		if (climbedFloor < player.Y + 15) raiseFloor();
    }

	void hdlCoinTriggered(Coin coin)
	{
		coin.transform.position = new Vector3(0,-10,0);
	}
	void hdlSpringActivated(Spring spring)
	{
		player.superJump();
	}
}
