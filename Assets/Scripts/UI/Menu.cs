using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
	public Game game;
	public Player player;
	public GameObject canvasCover;
	public GameplayUI gameplayUI;
	public GameObject canvasGameplay;
	public GameObject canvasGameOver;
	public Button bttnPlay, bttnExit, bttnPlayAgain, bttnExit2, bttnJump;
	public UI.ThumbController thumbController;

	private void Awake()
	{
		game.evntPlayerScoreChangd = hdlGameScoreChanged;
		game.evntGameOver = hdlGameOver;
		bttnPlay.onClick.AddListener(onClickPlay);
		bttnExit.onClick.AddListener(onExit);
		bttnPlayAgain.onClick.AddListener(onClickPlay);
		bttnExit2.onClick.AddListener(onExit);
		bttnJump.onClick.AddListener(onJump);
		thumbController.enabled = false;
	}
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		switch (thumbController.getDirection())
		{
			case -1:
				player.controllerMoveLeft();
				break;
			case 1:
				player.controllerMoveRight();
				break;
			case 0:
				player.controllerMoveStop();
				break;
		}

	}
	void onJump()
	{
		player.jump();
	}
	void hdlGameOver(Game game)
	{
		canvasGameplay.gameObject.SetActive(false);
		canvasGameOver.gameObject.SetActive(true);
		thumbController.enabled = false;
	}
	void hdlGameScoreChanged(Game game)
	{
		gameplayUI.setScore(game.score);

	}
	void onClickPlay()
	{
		game.startGame();
		canvasCover.SetActive(false);
		canvasGameOver.SetActive(false);
		gameplayUI.gameObject.SetActive(true);
		thumbController.enabled = true;
		gameplayUI.init();
	}
	void onExit()
	{
		//Finish the application 

		//canvasCover.SetActive(true);
		//gameplayUI.gameObject.SetActive(false);
	}
}
