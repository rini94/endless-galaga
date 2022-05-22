public static class GameState {

	public static GameConstants.GameStates currentState;
	public static int score;
	public static int currentLevel;
	public static int activeEnemyCount;
	public static int highestScore; //Loaded from PlayerPrefs when game is started

	static GameState () {

		highestScore = -1;
		currentState = GameConstants.GameStates.NOT_STARTED;
		ResetGameState ();
	}

	public static void ResetGameState () {
		
		score = 0;
		currentLevel = 1;
		activeEnemyCount = 0;
	}
}