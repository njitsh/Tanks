using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToControllerAssigner : MonoBehaviour
{
    private List<int> assignedControllers = new List<int>();
    public int[] player_controller_array = new int[4];

    private void Start()
    {
        for (int i = 0; i < player_controller_array.Length; i++)
        {
            player_controller_array[i] = 0;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 1; i <= player_controller_array.Length; i++)
        {
            if (assignedControllers.Contains(i))
                continue;

            if (Input.GetButton("J" + i + "A"))
            {
                AddPlayerController(i);
                break;
            }
        }
    }

    public void AddPlayerController(int controller)
    {
        assignedControllers.Add(controller);
        for (int i = 0; i < player_controller_array.Length; i++)
        {
            if (player_controller_array[i] != 0)
            {
                player_controller_array[i] = controller;
                Application.Quit();
            }
        }
    }
}
