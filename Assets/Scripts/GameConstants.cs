public static class GameConstants {

    public enum GameStates {

        NOT_STARTED,
        PLAYING,
        PAUSED,
        ENDED
    };

    public enum PooledObject {

        ENEMY,
        ENEMY_LASER,
        PLAYER_LASER,
        EXPLOSION,
        POWER_UP_POPUP,
        POWER_UP,
        LIFE,
        BONUS,
        ASTEROID
    }

    public enum PowerUpType {

        BONUS, //Double the amount of points for a short time
        HEALTH, //Recover health
        POWER_UP //Power up laser
    }

    public enum LaserType {

        PLAYER,
        ENEMY
    }

    //Gameplay constants
    //Player
    public const int MAX_LASER_LEVEL = 3;
    public const int MAX_PLAYER_HEALTH = 30;
    public const int BONUS_POINTS_DURATION = 10;
    public const float PLAYER_LASER_SPEED = 10f;
    public const float PLAYER_MOVEMENT_SPEED = 8f;
    public const float PLAYER_LASER_RATE = 0.3f;

    //Enemy
    public const int MAX_ENEMY_COUNT = 10;
    public const int MAX_ENEMY_HEALTH = 30;
    public const float ENEMY_LASER_SPEED = 7f;

    //Messages
    public const string MESSAGE_LASER_POWER_UP = "Laser Level Up!";
    public const string MESSAGE_HEALTH_UP = "Health +1";
    public const string MESSAGE_POINTS_UP = "Points x2";

    //PlayerPrefs constants
    public const string HIGHEST_SCORE = "highestscore";
}
