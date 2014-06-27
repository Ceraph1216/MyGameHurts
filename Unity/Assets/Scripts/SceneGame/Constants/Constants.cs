using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour 
{
	// Layer IDs
	public static int PLAYER_LAYER_ID = 8;
	public static int GROUND_LAYER_ID = 9;
	public static int ENEMY_LAYER = 10;

	// Game Dimensions
	public static int SCREEN_WIDTH = 900;
	public static int SCREEN_HEIGHT = 600;

	// Player Movement
	public static float LANDING_STUN_TIME = 0.05f;
	public static float RUN_SPEED = 30f;
	public static float JUMP_FORCE = 58f;
}