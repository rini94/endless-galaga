using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState {

	public enum GameStates {

		PLAYING,
		PAUSED
	};

	public static GameStates currentState;
	public static int score;
	public static int currentLevel;
	public static int maxLaserLevel;
	public static int maxEnemyCount;
	public static int activeEnemyCount;

	static GameState () {

		score = 0;
		currentState = GameStates.PLAYING;
		currentLevel = 1;
		maxLaserLevel = 3;
		maxEnemyCount = 10;
		activeEnemyCount = 0;
	}
}