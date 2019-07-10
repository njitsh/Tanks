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
            if (Input.GetButton("J" + i + "A") && !assignedControllers.Contains(i))
            {
                AddPlayerController(i);
                break;
            }
            if (Input.GetButton("J" + i + "B") && assignedControllers.Contains(i))
            {
                RemovePlayerController(i);
                break;
            }
        }
    }

    public void AddPlayerController(int controller)
    {
        assignedControllers.Add(controller);
        for (int i = 0; i < player_controller_array.Length; i++)
        {
            if (player_controller_array[i] == 0)
            {
                player_controller_array[i] = controller;
                switch (i + 1)
                {
                    case 1:
                        player1_text.SetText("P1 joined");
                        break;

                    case 2:
                        player2_text.SetText("P2 joined");
                        break;

                    case 3:
                        player3_text.SetText("P3 joined");
                        break;

                    case 4:
                        player4_text.SetText("P4 joined");
                        break;
                }
                break;
            }
        }
    }

    public void RemovePlayerController(int controller)
    {
        assignedControllers.Remove(controller);
        for (int i = 0; i < player_controller_array.Length; i++)
        {
            if (player_controller_array[i] == controller)
            {
                player_controller_array[i] = 0;
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
