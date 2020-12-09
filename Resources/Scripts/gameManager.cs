﻿using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;


// Next step today:

// A list of ships displayed next to the player grid
// Carrier - 5 blocks
// Battleship - 4 blocks
// Cruiser - 3 blocks
// Submarine - 3 blocks (different colour?)
// Destroyer - 2 blocks

// When clicked, select that ship (for example 5 blocks)
// if you left click on the player grid, 5 blocks reserved HORIZONTALLY
// if you right click on the player grid, 5 blocks reserved VERTICALLY
// if ship doesn't fit, write a debug.log stating that the ship doesn't fit and leave it selected



public class Ship
{
    
    int numberofblocks;
    Color backColor;
    
    bool vertical;
    public bool placed;
    private Button button;

    public Ship(int blocks)
    {       
        numberofblocks = blocks;
        vertical = false;
        placed = false;
        backColor = Color.red;
        
    }

    public void setBackColor(Color newcolor)
    {
        backColor = newcolor;
    }

    bool checkFree(int x, int y, BattleshipGrid g, bool orientation)
    {
        if (!orientation)
        {
            foreach (Block b in g.blocks)
            {
                if (b.indexX >= x && b.indexX < x + numberofblocks && b.indexY == y)
                {
                    if (b.filled)
                    {
                        return false;
                    }
                }

            }
        }
        else
        {
            foreach (Block b in g.blocks)
            {
                if (b.indexY >= y && b.indexY < y + numberofblocks && b.indexX == x)
                {
                    if (b.filled)
                    {
                        return false;
                    }
                }

            }
        }
        return true;

    }

    public void place(int x, int y, bool orientation, BattleshipGrid grid, Button currentbutton)
    {
        
        //does not allow me to place a ship twice
        if (!placed)
        {
            if (!orientation)
            {
                //horizontal --*
                if (x + (numberofblocks-1) <= 10)
                {
                    //should fit horizontally, if no ships in the way
                    if (checkFree(x, y, grid, false))
                    {

                        foreach (Block b in grid.blocks)
                        {
                            if (b.indexX >= x && b.indexX < x + numberofblocks && b.indexY == y)
                            {

                                b.toptile.GetComponent<SpriteRenderer>().color = Color.grey;
                                b.bottomtile.GetComponent<SpriteRenderer>().color = this.backColor;
                                b.filled = true;

                                
                            
                            }
                        }
                        placed = true;
                        
                    }

                }

            }
            else
            {
                //*
                if (y + (numberofblocks-1) <= 10)
                {
                    if (checkFree(x, y, grid, true))
                    {
                        foreach (Block b in grid.blocks)
                        {
                            //should fit vertically, if no ships in the way
                            if (b.indexY >= y && b.indexY < y + numberofblocks && b.indexX == x)
                            {

                                b.toptile.GetComponent<SpriteRenderer>().color = Color.grey;
                                b.bottomtile.GetComponent<SpriteRenderer>().color = this.backColor;
                                b.filled = true;
                            }
                        }
                        placed = true;
                    }
                }

            }
        }
        currentbutton.interactable = false;
    }
}






public class BattleshipGrid
{

    public List<Block> blocks;
    public GameObject parent;


    public BattleshipGrid()
    {
        blocks = new List<Block>();
    }


    //to place ships on the player grid using playerBoxController
    public void makeClickable()
    {
        foreach (Block b in blocks)
        {
            b.toptile.AddComponent<playerBoxController>();
            b.setClickCoordinates();

        }
    }

    //to place shots on the enemy grid using enemyBoxController
    public void makeClickableEnemy()
    {
        foreach (Block b in blocks)
        {
            b.toptile.AddComponent<enemyBoxController>();
            b.setClickCoordinates();

        }
    }


}

public class Block
{
    public GameObject toptile, bottomtile;
    public int indexX, indexY;
    public bool filled;


    public Block()
    {
        filled = false;
    }

    public void flipTile()
    {

    }

    public void setClickCoordinates()
    {
        if (toptile.GetComponent<playerBoxController>() != null)
        {
            toptile.GetComponent<playerBoxController>().indexX = indexX;
            toptile.GetComponent<playerBoxController>().indexY = indexY;
        }
    }



}

public class gameSession
{
    //has the game started?
    public bool gameStarted,isMyTurn;

    //number of shots fired
    int shotsFired;

    //blocks hit (to change color)
    List<Block> hitBlocks;

    Ship[] theShips;

   




    //for hits
    public BattleshipGrid enemyGrid;
    

    //no ships placed at the beginning of the game
    public gameSession(Ship[] allShips)
    {
        theShips = allShips;
        isMyTurn = false;
        gameStarted = false;
    }

    public bool areAllShipsPlaced()
    {
        foreach (Ship s in theShips)
        {
            if (!s.placed)
            {
                return false;
            }
        }        
        return true;
        
    }

    public void startGame()
    {
        
        isMyTurn = true;
        gameStarted = true;
    }

    

    public void fireShot()
    {

    }

    



}




public class gameManager : MonoBehaviour
{
    
    public BattleshipGrid playerGrid, enemyGrid;

    GameObject rowLabel, rowL, buttonPrefab,timerText,theTimer;

    public gameSession session;

    bool timerrunning = false;
    

    GameObject sq;

    Ship[] allships;

    public Ship currentlySelectedShip;
    public int shipid;
    public Button currentbutton;



    public IEnumerator myTurn()
    {
        
        timerText.GetComponentInChildren<Text>().text = "00:00";
        while (true)
        {
            if (session.areAllShipsPlaced())
            {
                //start rounds
                
                //update timer (running at the same time different speed)
                if (!timerrunning) 
                    {
                    
                        session.startGame();
                        StartCoroutine(updateTimer());
                    }

                //wait for player to play a shot

                //check if hit

                //if hit continue, if not stop



            }
            yield return null;
        }

    }

    //The code that runs the clock
    public IEnumerator updateTimer()
    {
        float timerValue = 0f;
     
        Text clockText = theTimer.GetComponentInChildren<Text>();

        timerrunning = true;

       // clockText.text = "00:00";
       while (true) { 
            if (session.isMyTurn)
            {
                timerValue++;

                float minutes = timerValue / 60f;
                float seconds = timerValue % 60f;

                clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


                //code that is running every second
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
        
    }



    Button createWorldButton(string label, GameObject parent, Vector3 pos)
    {
        GameObject theCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/myButton"), pos, Quaternion.identity);
        theCanvas.transform.SetParent(parent.transform);
        theCanvas.GetComponentInChildren<Text>().text = label;

        theCanvas.name = label;

        theCanvas.GetComponent<Canvas>().worldCamera = Camera.main;

        theCanvas.GetComponent<Canvas>().sortingOrder = 1;

        return theCanvas.GetComponentInChildren<Button>();


    }





    // Start is called before the first frame update
    void Start()
    {
        sq = Resources.Load<GameObject>("Prefabs/Square");

        rowLabel = Resources.Load<GameObject>("Prefabs/TextPrefab");

        buttonPrefab = Resources.Load<GameObject>("Prefabs/myButton");

        timerText = rowLabel;


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


        Button carrierButton = createWorldButton("Carrier", anchor3, new Vector3(0f, 1f));

        carrierButton.onClick.AddListener(
            () =>
            {
                Debug.Log("Carrier");
                
                //carrierButton.enabled = false;
                currentlySelectedShip = allships[4];
                
                currentbutton = carrierButton;
            }           
        );





        Button battleshipButton = createWorldButton("Battleship", anchor3, new Vector3(0f, -2f));

        battleshipButton.onClick.AddListener(
            () =>
            {
                
                Debug.Log("Battleship");

                currentlySelectedShip = allships[3];
                
                currentbutton = battleshipButton;

            }
        );


        Button submarineButton = createWorldButton("Submarine", anchor3, new Vector3(0f, -5f));

        submarineButton.onClick.AddListener(
            () =>
            {
                Debug.Log("Submarine");
                allships[2].setBackColor(Color.blue);
                currentlySelectedShip = allships[2];
               
                currentbutton = submarineButton;

            }
        );

        Button cruiserButton = createWorldButton("Cruiser", anchor3, new Vector3(0f, -8f));

        cruiserButton.onClick.AddListener(
            () =>
            {
                Debug.Log("Cruiser");

                currentlySelectedShip = allships[1];
                
                currentbutton = cruiserButton;
            }
        );

        Button destroyerButton = createWorldButton("Destroyer", anchor3, new Vector3(0f, -11f));

        destroyerButton.onClick.AddListener(
            () =>
            {
                Debug.Log("Destroyer");

                currentlySelectedShip = allships[0];                
                currentbutton = destroyerButton;
            }
        );

        Button removeships = createWorldButton("Remove Fleet", anchor3, new Vector3(0f, -14f));

        removeships.onClick.AddListener(
            () =>
            {
                Debug.Log("Color Change Active");
                foreach (Block b in playerGrid.blocks)
                {
                    b.bottomtile.GetComponent<SpriteRenderer>().color = Color.black;
                    b.toptile.GetComponent<SpriteRenderer>().color = Color.white;
                    
                    
                    

                    carrierButton.interactable = true;
                    carrier.placed = false;
                    battleshipButton.interactable = true;
                    battleship.placed = false;
                    submarineButton.interactable = true;
                    submarine.placed = false;
                    cruiserButton.interactable = true;
                    cruiser.placed = false;
                    destroyerButton.interactable = true;
                    destroyer.placed = false;

                    b.filled = false;                   
                   
                }

                
            }     
        );

     
        anchor3.transform.position = new Vector3(10f, -4f);




        enemyGrid = GenerateGrid(anchor2);
        enemyGrid.parent.transform.position = new Vector3(10f, 10f);
        enemyGrid.parent.transform.localScale = new Vector3(1.5f, 1.5f);
        enemyGrid.makeClickableEnemy();

        //at the moment, theTimer contains the word 'test'
        theTimer = Instantiate(timerText, new Vector3(-18f, 19f), Quaternion.identity);

        //gameSession, which is declared a bit further up.
        session = new gameSession(allships);

        Debug.Log("Hope this runs " + session.gameStarted);

        if (session.gameStarted)
        {
            removeships.interactable = false;
        }

        StartCoroutine(myTurn());

       


    }


    public IEnumerator waitForTurn()
    {
        yield return new WaitForSeconds(10f);
        session.isMyTurn = true;
    }

    string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };


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
                //first row at the top
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
                //setting the indexes of the blocks
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
