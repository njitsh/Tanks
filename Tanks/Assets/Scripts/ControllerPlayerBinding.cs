using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerBinding : MonoBehaviour
{
    public static int[] bindingsControllers = new int[4];

    // Start is called before the first frame update
    public static void TempControllerBinding()
    {
        GameObject Player_To_Controller_Assigner = GameObject.Find("Player_To_Controller_Assigner");
        PlayerToControllerAssigner ptocAssigner = Player_To_Controller_Assigner.GetComponent<PlayerToControllerAssigner>();
        bindingsControllers = ptocAssigner.player_controller_array;
    }

    public int getControllerBinding(int number)
    {
        return bindingsControllers[number - 1];
    }
}
