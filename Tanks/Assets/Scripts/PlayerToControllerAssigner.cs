using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerToControllerAssigner : MonoBehaviour
{
    public TextMeshProUGUI player1_text;
    public TextMeshProUGUI player2_text;
    public TextMeshProUGUI player3_text;
    public TextMeshProUGUI player4_text;

    private List<int> assignedControllers = new List<int>();
    public int[,] player_controller_array = new int[4,3];

    private bool keyboardExists;
    private bool playersFull;

    private int tank_color = 1;
    private int tank_barrel = 1;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            player_controller_array[i,0] = 0;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Join / Remove check button
        playersFull = true;
        for (int i = 0; i < 4; i++)
        {
            if (player_controller_array[i,0] == 0) playersFull = false;
        }
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetButton("J" + i + "A") && !assignedControllers.Contains(i) && !playersFull)
            {
                AddPlayerController(i,tank_color,tank_barrel);
                break;
            }
            else if (Input.GetButton("J" + i + "B") && assignedControllers.Contains(i))
            {
                RemovePlayerController(i);
                break;
            }
        }
        if (Input.GetButton("J5A") && !assignedControllers.Contains(5) && !playersFull)
        {
            AddPlayerController(5, tank_color, tank_barrel);
        }
        else if (Input.GetButton("J5B") && assignedControllers.Contains(5))
        {
            RemovePlayerController(5);
        }
    }

    // Add player to array
    public void AddPlayerController(int controller, int color, int barrel)
    {
        assignedControllers.Add(controller);
        for (int i = 0; i < 4; i++)
        {
            if (player_controller_array[i,0] == 0)
            {
                player_controller_array[i, 0] = controller;
                player_controller_array[i, 1] = color;
                player_controller_array[i, 2] = barrel;
                switch (i + 1)
                {
                    case 1:
                        
                        player1_text.SetText("P1 joined ");
                        break;

                    case 2:
                        player2_text.SetText("P2 joined ");
                        break;

                    case 3:
                        player3_text.SetText("P3 joined ");
                        break;

                    case 4:
                        player4_text.SetText("P4 joined ");
                        break;
                }
                break;
            }
        }
    }

    // Remove player from array
    public void RemovePlayerController(int controller)
    {
        assignedControllers.Remove(controller);
        for (int i = 0; i < 4; i++)
        {
            if (player_controller_array[i,0] == controller)
            {
                player_controller_array[i,0] = 0;
                switch (i + 1)
                {
                    case 1:
                        player1_text.SetText("Player 1");
                        break;

                    case 2:
                        player2_text.SetText("Player 2");
                        break;

                    case 3:
                        player3_text.SetText("Player 3");
                        break;

                    case 4:
                        player4_text.SetText("Player 4");
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
