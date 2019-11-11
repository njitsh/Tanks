using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerBinding : MonoBehaviour
{
    public static int[,] player_info = new int[4,3];
    public static string level_file_path;
    // Controllernumber
    // Color


    // Start is called before the first frame update
    public static void TempControllerBinding()
    {
        player_info = GameObject.Find("Player_To_Controller_Assigner").GetComponent<PlayerToControllerAssigner>().player_controller_array;
        level_file_path = GameObject.Find("LevelSelectorCanvas").GetComponent<LevelSelector>().level_path;
    }

    public int getControllerBinding(int number)
    {
        return player_info[number - 1, 0];
    }

    public string getLevelPath()
    {
        return level_file_path;
    }

    public int[,] getPlayerInfo()
    {
        return player_info;
    }
}
