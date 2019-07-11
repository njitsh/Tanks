using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    //public GameObject[] partsArray = new GameObject[4];

    // Selected Part
    public GameObject selectedPart;
    public GameObject target;
    public int sLayer;

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
                int grid_width = Screen.width / 33;
                int grid_height = Screen.height / 14;
                Vector3 placeLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - (Input.mousePosition.x % grid_width) + grid_width/2, Input.mousePosition.y - (Input.mousePosition.y % grid_height) + grid_height / 2, 10));
                GameObject block = Instantiate(target, placeLocation, Quaternion.identity);
                SpriteRenderer placed_block = block.GetComponent<SpriteRenderer>() as SpriteRenderer;
                placed_block.sortingLayerName = "PlacedBlocks";
                placed_block.sortingOrder = sLayer;
                Destroy(target);
                selectedPart = null;
            }
            else if ((Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Mouse1)) && selectedPart != null)
            {
                Destroy(target);
                selectedPart = null;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                // Delete object hover
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

    public void SetSortingLayer(int sortingOrder)
    {
        sLayer = sortingOrder;
    }
}
