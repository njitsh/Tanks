using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    //public GameObject[] partsArray = new GameObject[4];

    // Selected Part
    public GameObject selectedPart;
    public GameObject target;

    private Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        selectedPart = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (selectedPart != null) Cursor.visible = false;
            else Cursor.visible = true;
            if (Input.GetKeyDown(KeyCode.Mouse0) && selectedPart != null)
            {
                int grid_width = Screen.width / 16;
                int grid_height = Screen.height / 9;
                Vector3 placeLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - (Input.mousePosition.x % grid_width), Input.mousePosition.y - (Input.mousePosition.y % grid_height), 10));
                Instantiate(target, placeLocation, Quaternion.identity);
                Destroy(target);
                selectedPart = null;
            }
            else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Mouse1)) && selectedPart != null)
            {
                Destroy(target);
                selectedPart = null;
            }
            mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            if (selectedPart != null) target.transform.position = mousePos;
        }
    }

    void FindSelectedPart(GameObject lookforpart)
    {
        /*for (int i = 0; i < partsArray.Length; i++)
        {
            if (partsArray[i] == lookforpart)
            {
                selectedPart = i;
                break;
            }
        }*/
    }

    public void Select_Object(GameObject Selected_Object)
    {
        selectedPart = Selected_Object;
        Destroy(target);
        target = (GameObject)Instantiate(Selected_Object);
    }
}
