using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;



class BattleshipGrid
{

    public List<Block> blocks;
    public GameObject parent;


    public BattleshipGrid()
    {
        blocks = new List<Block>();
    }

    public void makeClickable()
    {
        foreach (Block b in blocks)
        {
            b.toptile.AddComponent<playerBoxController>();
            b.setClickCoordinates();
           
        }
    }
}

class Block
{
    public GameObject toptile,bottomtile;
    public int indexX, indexY;

    public void flipTile()
    {

    }

    public void setClickCoordinates()
    {
        if (toptile.GetComponent<playerBoxController>()!=null)
        {
            toptile.GetComponent<playerBoxController>().indexX = indexX;
            toptile.GetComponent<playerBoxController>().indexY = indexY;
        }
    }

  

}




public class gameManager : MonoBehaviour
{

    BattleshipGrid playerGrid, enemyGrid;

    GameObject rowLabel, rowL;


    GameObject sq;



    // Start is called before the first frame update
    void Start()
    {
        sq = Resources.Load<GameObject>("Prefabs/Square");

        rowLabel = Resources.Load<GameObject>("Prefabs/TextPrefab");




        GameObject anchor = new GameObject("playergrid");
        GameObject anchor2 = new GameObject("enemygrid");




        playerGrid = GenerateGrid(anchor);
        playerGrid.parent.transform.position = new Vector3(-10f, -10f);
        playerGrid.parent.transform.localScale = new Vector3(1.5f, 1.5f);
        playerGrid.makeClickable();

        enemyGrid = GenerateGrid(anchor2);
        enemyGrid.parent.transform.position = new Vector3(10f, 10f);
        enemyGrid.parent.transform.localScale = new Vector3(1.5f, 1.5f);
    }


    string [] letters = {"A","B","C","D","E","F","G","H","I","J"};


    BattleshipGrid GenerateGrid(GameObject parentObject)
    {
        int rowcounter = 0;
        int columncounter = 0;
        int lettercounter = 0;
        BattleshipGrid grid = new BattleshipGrid();

        for (float ycoord = -4.5f; ycoord <= 4.5f; ycoord++)
        {
            //for each row

            rowcounter++;
            rowL = Instantiate(rowLabel, new Vector3(-5.5f, ycoord), Quaternion.identity);
            rowL.GetComponentInChildren<Text>().text = rowcounter.ToString();
            rowL.transform.SetParent(parentObject.transform);
            //rowL.transform.GetChild(0).transform.position = new Vector3(-2f, ycoord));

            
            



            for (float xcoord = -4.5f; xcoord <= 4.5f; xcoord++)
            {
                //first row
                if (ycoord == 4.5f)
                {
                    
                    rowL = Instantiate(rowLabel, new Vector3(xcoord, 5.5f), Quaternion.identity);
                    rowL.GetComponentInChildren<Text>().text = letters[lettercounter];
                    rowL.transform.SetParent(parentObject.transform);
                    lettercounter++;
                }

                columncounter++;
                Block b = new Block();
                b.bottomtile = Instantiate(sq, new Vector3(xcoord, ycoord), Quaternion.identity);
                b.toptile = Instantiate(sq, new Vector3(xcoord, ycoord), Quaternion.identity);
                b.toptile.transform.localScale = new Vector3(0.8f, 0.8f);
                b.toptile.name = "TopTile";
                b.toptile.AddComponent<BoxCollider2D>();
                b.toptile.GetComponent<BoxCollider2D>().isTrigger = true;
                b.bottomtile.GetComponent<SpriteRenderer>().color = Color.black;
                b.toptile.transform.SetParent(parentObject.transform);
                b.bottomtile.transform.SetParent(parentObject.transform);
                b.bottomtile.name = "BottomTile";
                b.indexX = columncounter;
                b.indexY = rowcounter;
                
                grid.blocks.Add(b);

            }
            columncounter = 0;
        }
        grid.parent = parentObject;
        return grid;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
