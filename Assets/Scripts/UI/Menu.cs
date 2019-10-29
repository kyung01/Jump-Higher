﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
	public Game game;
	public GameObject canvasCover;
	public GameplayUI gameplayUI;
	public GameObject canvasGameOver;
	public Button bttnPlay, bttnExit;

	private void Awake()
	{
		game.evntPlayerScoreChangd = hdlGameScoreChanged;
		game.evntGameOver = hdlGameOver;
		bttnPlay.onClick.AddListener(onClickPlay);
		bttnExit.onClick.AddListener(onExit);
	}
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	void hdlGameOver(Game game)
	{
		canvasGameOver.gameObject.SetActive(true);
	}
	void hdlGameScoreChanged(Game game)
	{
		gameplayUI.setScore(game.score);

	}
	void onClickPlay()
	{
		game.startGame();
		canvasCover.SetActive(false);
		gameplayUI.gameObject.SetActive(true);
		gameplayUI.init();
	}
	void onExit()
	{
		canvasCover.SetActive(true);
		gameplayUI.gameObject.SetActive(false);
	}
}