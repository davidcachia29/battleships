using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;



class Ship
{
    int numberofblocks;
  
    bool vertical;
    bool placed;


    public Ship(int blocks)
    {
        numberofblocks = blocks;
        vertical = false;
        placed = false;

    }

    public void place(int x,int y,bool orientation)
    {

    }
}






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

    GameObject rowLabel, rowL, buttonPrefab;


    GameObject sq;

    Ship[] allships;


    Button createWorldButton(string label,GameObject parent,Vector3 pos)
    {
        GameObject theButton = Instantiate(Resources.Load<GameObject>("Prefabs/myButton"), pos, Quaternion.identity);
        theButton.transform.SetParent(parent.transform);
        theButton.GetComponentInChildren<Text>().text = label;

        theButton.name = label;

        theButton.GetComponent<Canvas>().worldCamera = Camera.main;

        theButton.GetComponent<Canvas>().sortingOrder = 1;

        return theButton.GetComponent<Button>();

       
    }



    // Start is called before the first frame update
    void Start()
    {
        sq = Resources.Load<GameObject>("Prefabs/Square");

        rowLabel = Resources.Load<GameObject>("Prefabs/TextPrefab");

        buttonPrefab = Resources.Load<GameObject>("Prefabs/myButton");


        allships = new Ship[5];

        Ship carrier = new Ship(5);
        Ship battleship = new Ship(4);
        Ship cruiser = new Ship(3);
        Ship submarine = new Ship(3);
        Ship destroyer = new Ship(2);


        allships[4] = carrier;
        allships[3] = battleship;
        allships[2] = cruiser;
        allships[1] = submarine;
        allships[0] = destroyer;



        GameObject anchor = new GameObject("playergrid");
        GameObject anchor2 = new GameObject("enemygrid");
        GameObject anchor3 = new GameObject("shipselectiongrid");



        //draw player grid
        playerGrid = GenerateGrid(anchor);
        playerGrid.parent.transform.position = new Vector3(-10f, -10f);
        playerGrid.parent.transform.localScale = new Vector3(1.5f, 1.5f);
        playerGrid.makeClickable();

        //ship selection grid


        Button carrierButton = createWorldButton("Carrier", anchor3,new Vector3(0f,0f));


        Button battleshipButton = createWorldButton("Battleship", anchor3, new Vector3(0f, -3f));

        Button submarineButton = createWorldButton("Submarine", anchor3, new Vector3(0f, -6f));


        Button cruiserButton = createWorldButton("Cruiser", anchor3, new Vector3(0f, -9f));

        Button destroyerButton = createWorldButton("Destroyer", anchor3, new Vector3(0f, -12f));

       



       
      
        anchor3.transform.position = new Vector3(10f, -4f);








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
