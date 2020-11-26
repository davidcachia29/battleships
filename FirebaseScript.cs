using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Unity.Editor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


//this corresponds to the attributes inside firebase
public class Player
{
    public string PlayerName;
    public int PlayerAge;
}

public class FirebaseScript : MonoBehaviour
{
    // Start is called before the first frame update

    //public List<KeyValuePair<string, MyUser>> Users;

    DatabaseReference reference;



    Player myplayer;

    bool displaydata = false;
    bool usercreated = false;
    bool userloggedin = false;

    bool numberofrecordsretreived = false;


    string output = "";

    int counter = 0;
    int numberOfRecords = 0;

    GameObject addUserButton, loginButton, getDataButton;

    //main data dictionary
    Dictionary<string, object> myDataDictionary;

    //a reference to the firebase authentication scheme
    FirebaseAuth auth;

    string email = "gerrysaid.test5@gmail.com";
    string password = "IamNotSupposedToSeeThis1234!";
    // string password = "123";


    IEnumerator createUser()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //this is not enough, because I have no guarantee of the user being created.
        //auth.CreateUserWithEmailAndPasswordAsync(email, password);



        Task createusertask = auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(
             //created an anonymous inner class inside continueonmainthread which is of type Task
             createUserTask =>
             {
                //if anything goes wrong
                if (createUserTask.IsCanceled)
                 {
                    //I pressed escape or cancelled the task
                    Debug.Log("Sorry, user was not created!");
                     return;
                 }
                 if (createUserTask.IsFaulted)
                 {
                    //my internet exploded or firebase exploded or some other error happened here
                    Debug.Log("Sorry, user was not created!" + createUserTask.Exception);
                     usercreated = true;
                     return;
                 }
                //if anything goes wrong, otherwise
                Firebase.Auth.FirebaseUser myNewUser = createUserTask.Result;
                 Debug.Log("Your nice new user is:" + myNewUser.DisplayName + " " + myNewUser.UserId);
                //THIS IS WHAT HAPPENS AT THE END OF THE ASYNC TASK
                usercreated = true;

             }
             );

        //while (!usercreated)
        // {

        //    yield return null;
        // }
        //this is where my user has been created
        //Debug.Log("ready!!!");

        //*a better way to wait until the end of the coroutine*//
        yield return new WaitUntil(() => createusertask.IsCompleted);



    }

    //sign in to the firebase instance so we can read some data
    //Coroutine Number 1
    IEnumerator signInToFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;


        Task signintask = auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(
             signInTask =>
             {
                 if (signInTask.IsCanceled)
                 {
                     //write cancelled in the console
                     Debug.Log("Cancelled!");
                     return;
                 }
                 if (signInTask.IsFaulted)
                 {
                     //write the actual exception in the console
                     Debug.Log("Something went wrong!" + signInTask.Exception);
                     return;
                 }

                 Firebase.Auth.FirebaseUser loggedInUser = signInTask.Result;
                 Debug.Log("User " + loggedInUser.DisplayName + " has logged in!");
                 userloggedin = true;
             }
            );
        /*
        while (!userloggedin)
        {

            yield return null;
        }*/

        yield return new WaitUntil(() => signintask.IsCompleted);

        Debug.Log("User has signed in");



    }




    //get the number of records for a child
    IEnumerator getNumberOfRecords()
    {
       Task numberofrecordstask =  reference.GetValueAsync().ContinueWithOnMainThread(
            getValueTask =>
            {
                if (getValueTask.IsFaulted)
                {
                    Debug.Log("Error getting data " + getValueTask.Exception);
                }

                if (getValueTask.IsCompleted)
                {
                    DataSnapshot snapshot = getValueTask.Result;
                    Debug.Log(snapshot.ChildrenCount);
                    numberOfRecords = (int)snapshot.ChildrenCount;
                    numberofrecordsretreived = true;
                }


            }
            );
        /*
        while (!numberofrecordsretreived)
            yield return null;*/

        yield return new WaitUntil(() => numberofrecordstask.IsCompleted);


    }

    IEnumerator getDataFromFirebase(string childLabel)
    {

        Task getdatatask = reference.Child(childLabel).GetValueAsync().ContinueWithOnMainThread(
            getValueTask =>
            {
                if (getValueTask.IsFaulted)
                {
                    Debug.Log("Error getting data " + getValueTask.Exception);
                }

                if (getValueTask.IsCompleted)
                {
                    DataSnapshot snapshot = getValueTask.Result;
                    //Debug.Log(snapshot.Value.ToString());

                    //snapshot object is casted to an instance of its type
                    myDataDictionary = (Dictionary<string, object>)snapshot.Value;


                    //    Debug.Log("Data received");
                    displaydata = true;
                }


            }
            );
        //shock absorber
        /* while (!displaydata)
         {
             //the data has NOT YET been saved to snapshot
             yield return null;

         }*/

        yield return new WaitUntil(() => getdatatask.IsCompleted);

        //the data has been saved to snapshot here
        yield return StartCoroutine(displayData());
    }


    IEnumerator getAllDataFromFirebase()
    {

        Task getdatatask = reference.GetValueAsync().ContinueWithOnMainThread(
            getValueTask =>
            {
                if (getValueTask.IsFaulted)
                {
                    Debug.Log("Error getting data " + getValueTask.Exception);
                }

                if (getValueTask.IsCompleted)
                {
                    DataSnapshot snapshot = getValueTask.Result;
                    //Debug.Log(snapshot.Value.ToString());

                    //snapshot object is casted to an instance of its type
                    myDataDictionary = (Dictionary<string, object>)snapshot.Value;


                    //    Debug.Log("Data received");
                    displaydata = true;
                }


            }
            );
        //shock absorber
        /* while (!displaydata)
         {
             //the data has NOT YET been saved to snapshot
             yield return null;

         }*/

        yield return new WaitUntil(() => getdatatask.IsCompleted);

        //the data has been saved to snapshot here
        yield return StartCoroutine(displayData());
    }

    IEnumerator displayData()
    {
        foreach (var element in myDataDictionary)
        {
            Debug.Log(element.Key.ToString() + "<->" + element.Value.ToString());
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }




    void createUserButtonClicked()
    {
        Debug.Log("Create User Here2");

    }


    void createUserButtonClicked2(string param)
    {
        Debug.Log("Create User Here3 " + param);

    }


    void loginToFirebase()
    {
        Debug.Log("Start signing in");
        StartCoroutine(signInToFirebase());
        loginButton.GetComponent<Button>().interactable = false;
    }

    void getMyData(string dataToGet)
    {
        Debug.Log(dataToGet);
        StartCoroutine(getDataFromFirebase(dataToGet));
    }

    //TASK 1: List all the players that you have listed in your firebase database.  

    //TASK 1 Answer below
    IEnumerator getAllData()
    {
        yield return getNumberOfRecords();
        Debug.Log(numberOfRecords);

        // for (int i = 0; i <= numberOfRecords; i++)
        // {
        //      string childstring = "Player" + (i + 1);
        //  Debug.Log(childstring);
        //     yield return getDataFromFirebase(childstring);
        //  }

        yield return getAllDataFromFirebase();

        Debug.Log("All records retreived");

        yield return null;
    }

    //list data from firebase
    void Start()
    {

        //the link to the database that I will be accessing
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gerry-firebase1.firebaseio.com/");

        reference = FirebaseDatabase.DefaultInstance.RootReference;

        addUserButton = GameObject.Find("CreateUserButton");

        loginButton = GameObject.Find("LoginButton");

        getDataButton = GameObject.Find("GetDataButton");


        //method 3
        getDataButton.GetComponent<Button>().onClick.AddListener(delegate {

            StartCoroutine(getAllData());

        });


        //method 2
        loginButton.GetComponent<Button>().onClick.AddListener(loginToFirebase);


        //method 1 - the quickest method with the weirdest notation
        addUserButton.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                Debug.Log("Create User Here");
                StartCoroutine(createUser());
                addUserButton.GetComponent<Button>().interactable = false;
                //we need to wait for createUser to be done
            }
        );

        //method 2 - using a method to get the listener working
        addUserButton.GetComponent<Button>().onClick.AddListener(createUserButtonClicked);

        string myString = "Hello Hello";

        //method 3 - using a method to get the listener working with parameters
        addUserButton.GetComponent<Button>().onClick.AddListener(delegate { createUserButtonClicked2(myString); });








    }



}
