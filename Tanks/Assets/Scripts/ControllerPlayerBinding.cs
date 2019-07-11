using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerBinding : MonoBehaviour
{
    public static int[,] player_info = new int[4,3];
    // Controllernumber
    // Color


    // Start is called before the first frame update
    public static void TempControllerBinding()
    {
        GameObject Player_To_Controller_Assigner = GameObject.Find("Player_To_Controller_Assigner");
        PlayerToControllerAssigner ptocAssigner = Player_To_Controller_Assigner.GetComponent<PlayerToControllerAssigner>();
        player_info = ptocAssigner.player_controller_array;
    }

    public int getControllerBinding(int number)
    {
        return player_info[number - 1, 0];
    }

    public int[,] getPlayerInfo()
    {
        return player_info;
    }
}
