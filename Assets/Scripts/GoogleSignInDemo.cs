using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using JetBrains.Annotations;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSignInDemo : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] string webClientId = "<your client id here>";
    [SerializeField] TMP_Text txtpro;
    [SerializeField] TMP_Text googleUserEmailTXT;

    [SerializeField] private ScriptableGameOBJ playerData;
    // [SerializeField] Text infoText;

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    //public TaskList list;
    //public AccountSerialized accountSerialized;
    public AccountsListSerialized accountsListSerialized;
    public Task currentLoggedInTask;
    
    //for updating the deleted stuff in json
    public List<string> calledTaskListIDs = new List<string>();
    public List<string> calledTaskIDs = new List<string>();



    public TriggerEvent onUserSuccessfulSignIn;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestEmail = true,
            RequestIdToken = true,
            RequestAuthCode = true,
            ForceTokenRefresh = true,
            AdditionalScopes = new []
            {
                "https://www.googleapis.com/auth/tasks",
                "https://www.googleapis.com/auth/tasks.readonly"
            }
        };
        
        CheckFirebaseDependencies();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        updateJSONonAction(gameManager.GoogleUser.Email);
    }

    
    void OnApplicationQuit()
    {
        updateJSONonAction(gameManager.GoogleUser.Email);
    }

    void OnApplicationPause(bool pauseStatus)
    {
        updateJSONonAction(gameManager.GoogleUser.Email);
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    auth = FirebaseAuth.DefaultInstance;
                }
                else
                {
                    // AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
                }
            }
            else
            {
                // AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        txtpro.text = "Signing in...";
        
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        //list.tasksList.Clear();
    }

    private void OnSignOut()
    {
        txtpro.text = "Signing out...";
        
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        // AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator();
            
            if (enumerator.MoveNext())
            {
                GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                // AddToInformation("Got Error: " + error.Status + " " + error.Message);
            }
            else
            {
                // AddToInformation("Got Unexpected Exception?!?" + task.Exception);
            }
        }
        else if (task.IsCanceled)
        {
            // AddToInformation("Canceled");
        }
        else
        {
            // AddToInformation("Welcome: " + task.Result.DisplayName + "!"); 
            // AddToInformation("Email = " + task.Result.Email); 
            // AddToInformation("Google ID Token = " + task.Result.IdToken); 
            // AddToInformation("Email = " + task.Result.Email);
            GoogleSignInUser googleUser = task.Result;
            gameManager.GoogleUser = googleUser;


            txtpro.text = $"Successfully logged in!\nEmail:{googleUser.Email}\nAuth code: {googleUser.AuthCode}";

            StartCoroutine(Test());

            // save email here (will serve as unique id of user)
            googleUserEmailTXT.text = googleUser.Email;

        }
    }

    //MAIN
    private IEnumerator Test()
    {
        WWWForm f = new WWWForm();
        f.AddField("code", gameManager.GoogleUser.AuthCode);
        f.AddField("client_id", "962649025840-rsj8lgl6h2ate7u8od6h1lagpn6okq50");
        f.AddField("client_secret", "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");
        f.AddField("grant_type", "authorization_code");
        f.AddField("scope", "https://www.googleapis.com/auth/tasks");
        f.AddField("scope", "https://www.googleapis.com/auth/tasks.readonly");

        using var req1 = UnityWebRequest.Post("https://accounts.google.com/o/oauth2/token", f);

        yield return req1.SendWebRequest();

        if (req1.result == UnityWebRequest.Result.Success)
        {
            txtpro.text = req1.downloadHandler.text;
            var json = SimpleJSON.JSON.Parse(req1.downloadHandler.text);
            gameManager.AccessToken = json["access_token"];
            gameManager.RefreshToken = json["refresh_token"];

            // Request all task lists from user
            string uri = "https://tasks.googleapis.com/tasks/v1/users/@me/lists";
            using var req2 = UnityWebRequest.Get(uri);
            req2.SetRequestHeader("Authorization", $"Bearer {gameManager.AccessToken}");
            req2.SetRequestHeader("Accept", "application/json");
        
            yield return req2.SendWebRequest();
        
            if (req2.result == UnityWebRequest.Result.Success)
            {
                var j2 = SimpleJSON.JSON.Parse(req2.downloadHandler.text);

                foreach (var taskListJson in j2["items"].AsArray.Children)
                {
                    string taskListId = taskListJson["id"];
                    string reqUri = "https://www.googleapis.com/tasks/v1/lists/" + taskListId + "/tasks";

                    using var req3 = UnityWebRequest.Get(reqUri);
                    req3.SetRequestHeader("Authorization", $"Bearer {gameManager.AccessToken}");
                    req3.SetRequestHeader("Accept", "application/json");

                    yield return req3.SendWebRequest();

                    if (req3.result == UnityWebRequest.Result.Success)
                    {
                        var j3 = SimpleJSON.JSON.Parse(req3.downloadHandler.text);

                        txtpro.text = "";
                        
                        foreach (var taskJson in j3["items"].AsArray.Children)
                        {
                            Task task = new Task();
                            task.taskListId = taskListId;
                            calledTaskListIDs.Add(taskListId);

                            task.taskId = taskJson["id"];
                            calledTaskIDs.Add(taskJson["id"]);

                            task.title = taskJson["title"];
                            task.notes = taskJson["notes"];
                            task.status = taskJson["status"];
                            task.dueDate = taskJson["due"];

                            /*task.taskListId = taskListId;
                            task.taskId = "taskJson[\"id\"]";
                            task.title = "taskJson[\"title\"]";
                            task.notes = "taskJson[\"notes\"]";
                            task.status = "taskJson[\"status\"]";
                            task.dueDate = "2022-06-15T17:13:34.000Z";*/

                            currentLoggedInTask = task;


                            //check to validate some stuff
                            validateSaveDataFile(gameManager.GoogleUser.Email, currentLoggedInTask.taskId);

                            txtpro.text += $"\nTitle: {task.title}\nTask ID: {task.taskId}\nNotes: {task.notes}\nStatus: {task.status}\nDue Date: {task.dueDate}";
                        }

                        //force update unused data in json
                        deleteUnusedJSON(gameManager.GoogleUser.Email, calledTaskListIDs, calledTaskIDs);
                        
                        //load stuff
                        LoadPlayerDataFromJSON(gameManager.GoogleUser.Email);
                        
                        //onUserSuccessfulSignIn.Invoke();

                    }
                    else
                    {
                        txtpro.text = $"{req3.error}\n{req3.downloadHandler.text}";
                    }
                }
            }
            else
            {
                txtpro.text = $"{req2.error}\n{req2.downloadHandler.text}";
            }
        }
        else
        {
            txtpro.text = $"{req1.error}\n{req1.downloadHandler.text}";
        }
    }

    public void deleteUnusedJSON(string email,List<string> calledtasklistids, List<string> calledtaskids)
    {
        string filePath = System.IO.Directory.GetCurrentDirectory() + "\\";
        string fileName = "data.json";
        List<string> uncalledTaskListIDs = new List<string>();
        List<string> uncalledTaskIDs = new List<string>();

        string json = File.ReadAllText(filePath + fileName);
        accountsListSerialized = JsonUtility.FromJson<AccountsListSerialized>(json);

        int emailIndex = FindEmailIndexInJSON(accountsListSerialized, email);
        if (emailIndex != -1)
        {
            AccountSerialized account = accountsListSerialized.accountsSerialized[emailIndex];
            List<TaskListSerialized> tls = account.tasksListSerialized;

            //delete tasklists
            for (int i = 0; i < tls.Count; i++)
            {
                bool isFound = false;
                string temp = tls[i].TaskListID;
                for (int j = 0; j < calledtasklistids.Count; j++)
                {
                    if (temp == calledtasklistids[j])
                    {
                        isFound = true;

                        List<Task> t = tls[i].tasks;
                        for (int k = 0; k < t.Count; k++)
                        {
                            bool isFound2 = false;
                            string temp2 = t[k].taskId;
                            for (int z = 0; z < calledtaskids.Count; z++)
                            {
                                if (temp2 == calledtaskids[z])
                                {
                                    isFound2 = true;
                                    break;
                                }
                            }

                            if (!isFound2)
                            {
                                uncalledTaskIDs.Add(temp2);
                            }
                        }

                        break;
                    }
                }

                if (!isFound)
                {
                    uncalledTaskListIDs.Add(temp);
                }
                
            }

            

            //delete the stuff
            for (int i = 0; i < uncalledTaskIDs.Count; i++)
            {
                bool isFound = false;
                string temp = uncalledTaskIDs[i];
                for (int j = 0; j < tls.Count; j++)
                {
                    List<Task> t = tls[j].tasks;
                    for (int k = 0; k < t.Count; k++)
                    {
                        if (temp == t[k].taskId)
                        {
                            t.Remove(t[k]);
                            isFound = true;
                            break;
                        }
                    }

                    if (isFound)
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < uncalledTaskListIDs.Count; i++)
            {
                string temp = uncalledTaskIDs[i];
                for (int j = 0; j < tls.Count; j++)
                {
                    if (tls[j].TaskListID == temp)
                    {
                        tls.Remove(tls[j]);
                        break;
                    }
                }
            }


            updateJSONfile(accountsListSerialized, filePath, fileName);

        }


    }

    public string[] splitString(string needle, string haystack)
    {
        //This will look for NEEDLE in HAYSTACK and return an array of split strings.
        //NOTE: If the returned array has a length of 1 (meaning it only contains
        //        element [0]) then that means NEEDLE was NOT found.

        return haystack.Split(new string[] { needle }, System.StringSplitOptions.None);

    }

    public void validateSaveDataFile(string email,string tasklistid)//TaskList tempList
    {
        string filePath = System.IO.Directory.GetCurrentDirectory() + "\\";
        string fileName = "data.json";
        //check if file exists
        bool test = System.IO.File.Exists(filePath + fileName);
        //Debug.Log("Bool: " + test + "  Name:  " + filePath+fileName);
        //TaskList tl = JsonUtility.FromJson<TaskList>()


        //if does not exist
        //create
        if (!test)
        {
            accountsListSerialized.accountsSerialized.Add(CreateNewAccountSerialized(email, tasklistid));
            string emptyJson = JsonUtility.ToJson(accountsListSerialized, true);
            Debug.Log("Created JSON data");
            File.WriteAllText(filePath + fileName, emptyJson);
        }

        



        //
        {
            //open
            string json = File.ReadAllText(filePath+fileName);
            accountsListSerialized = JsonUtility.FromJson<AccountsListSerialized>(json);
            //Debug.Log("JSON: " + jsonFileRead.Email + " " +jsonFileRead.tasksListSerialized[0].tasks[0]);

            //check if email exists inside json
            //AccountSerialized toFind = new AccountSerialized();
            //toFind.Email = "testEmail";//gameManager.GoogleUser.Email;

            int emailIndex = FindEmailIndexInJSON(accountsListSerialized, email);
            if (emailIndex != -1) 
            //if exists
            {
                Debug.Log("Email Exists");
                //check task id
                int taskListIDindex = FindTaskListIndexFromIDInJSON(accountsListSerialized, emailIndex, tasklistid);


                //if exists
                if(taskListIDindex != -1)
                {
                    Debug.Log("TaskList ID exists");

                    List<Task> tasks = accountsListSerialized.accountsSerialized[emailIndex].tasksListSerialized[taskListIDindex].tasks;
                    
                    int taskindex = FindTaskIndexFromIDInJSON(accountsListSerialized, emailIndex, taskListIDindex,
                        tasklistid); // use current task id
                    //task exists
                    if (taskindex != -1)
                    {
                        bool didUpdate = false;
                        bool didStatusChange = false;
                        //update tasks
                        if (tasks[taskindex].title != currentLoggedInTask.title)
                        {
                            tasks[taskindex].title = currentLoggedInTask.title;//currentLoggedInTask.title
                            didUpdate = true;
                        }

                        if (tasks[taskListIDindex].notes != currentLoggedInTask.notes)
                        {
                            tasks[taskindex].notes = currentLoggedInTask.notes;
                            didUpdate = true;
                        }
                     
                        if (tasks[taskindex].status != currentLoggedInTask.status)
                        {
                            tasks[taskindex].status = currentLoggedInTask.status;
                            didUpdate = true;
                            didStatusChange = true;
                        }

                        if (tasks[taskindex].dueDate != currentLoggedInTask.dueDate)
                        {
                            tasks[taskindex].dueDate = currentLoggedInTask.dueDate;
                            didUpdate = true;
                        }

                       
                        //check if overdue
                        if (currentLoggedInTask.status != "completed")
                        {
                            if (!checkIfDueDateIsLate(tasks[taskindex].dueDate))
                            {
                                //damage
                                playerData.overduedTasks++;

                            }
                        }
                        else
                        {
                            //reward
                            playerData.completedTasks++;
                        }



                        if (didUpdate)
                        {
                            

                            updateJSONfile(accountsListSerialized, filePath, fileName);
                        }
                        
                    }
                    //task does not exist
                    else
                    {
                        //create a task
                        tasks.Add(CreateNewTaskSerialized(currentLoggedInTask));
                        updateJSONfile(accountsListSerialized, filePath, fileName);
                    }


                }
                else//else not exists
                {
                    //append tasklist
                    Debug.Log("TaskList ID DOES NOT exists");
                    accountsListSerialized.accountsSerialized[emailIndex].tasksListSerialized.Add(
                        CreateTaskListSerialized(currentLoggedInTask));
                    updateJSONfile(accountsListSerialized, filePath, fileName);

                }

            }
            //does not exist
            else{
                Debug.Log("Email DOES NOT Exists");
                //append
                accountsListSerialized.accountsSerialized.Add(CreateNewAccountSerialized(email, tasklistid));
                updateJSONfile(accountsListSerialized, filePath, fileName);
            }
        }

        
    }

    public bool checkIfDueDateIsLate(string date)
    {
        if (date == null) return false;

        int year;
        int month;
        int day;
        int hour;
        int minute;
        int second;


        string[] dateSeparated = splitString("-",date );
        year = Convert.ToInt32(dateSeparated[0]);
        month = Convert.ToInt32(dateSeparated[1]);
        string[] time = splitString("T", dateSeparated[2]);
        day = Convert.ToInt32(time[0]);
        string[] timedelimeter = splitString(":", time[1]);
        hour = Convert.ToInt32(timedelimeter[0]);
        minute = Convert.ToInt32(timedelimeter[1]);
        string[] s = splitString(".", timedelimeter[2]);
        second = Convert.ToInt32(s[0]);

        string currentDate = System.DateTime.UtcNow.ToLocalTime().ToString("yyyy MM dd hh mm ss");

        string currentYearStr = currentDate.Substring(0, 4);
        string currentMonthStr = currentDate.Substring(5, 2);
        string currentDayStr = currentDate.Substring(8, 2);
        string currentHour = currentDate.Substring(11, 2);
        string currentMin = currentDate.Substring(14, 2);
        string currentSec = currentDate.Substring(17, 2);

        int currentYearInt = Int32.Parse(currentYearStr);
        int currentMonthInt = Int32.Parse(currentMonthStr);
        int currentDayInt = Int32.Parse(currentDayStr);
        int currentHourInt = Int32.Parse(currentHour);
        int currentMinuteInt = Int32.Parse(currentMin);
        int currentSecondInt = Int32.Parse(currentSec);

     

        Debug.Log(" Date:  "+ year + " " + month + " " + day + " Time:  " + hour + " " + minute + " " + second + " " );
        Debug.Log("CURRENT Date:  " + currentYearInt + " " + currentMonthInt + " " + currentDayInt + 
                  "CURRENT Time:  " + currentHourInt + " " + currentMinuteInt + " " + currentSecondInt + " ");


        // compare to current date
        if (year <= currentYearInt)
        {
            if (month <= currentMonthInt)
            {
                if (day <= currentDayInt)
                {
                    if (hour <= currentHourInt)
                    {
                        if (minute <= currentMinuteInt)
                        {
                            if (second < currentSecondInt)
                            {
                                return true;
                            }

                            return false;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }

            return false;
        }


        return false;
    }


    public AccountSerialized CreateNewAccountSerialized(string email, string ID)
    {
        AccountSerialized account = new AccountSerialized();
        account.Email = email;//gameManager.GoogleUser.Email;
        TaskListSerialized tls = CreateTaskListSerialized(currentLoggedInTask); //use currenttask instead of this 

        account.selectedPetName = "someName";
        account.selectedBGName = "someBGName";
        account.hp = 10;
        account.food = 10;
        account.money = 150;

        playerData.selectedBGName = "someBGName";
        playerData.selectedPetName = "someName";
        playerData.hp = account.hp;
        playerData.food = account.food;
        playerData.money = account.money;


        //tls.tasks.Add(currentLoggedInTask);
        account.tasksListSerialized.Add(tls);

        return account;
    }

    public void LoadPlayerDataFromJSON(string email)
    {
        string filePath = System.IO.Directory.GetCurrentDirectory() + "\\";
        string fileName = "data.json";

        string json = File.ReadAllText(filePath + fileName);
        accountsListSerialized = JsonUtility.FromJson<AccountsListSerialized>(json);


        int emailIndex = FindEmailIndexInJSON(accountsListSerialized, email);
        if (emailIndex != -1)
        {
            AccountSerialized account = accountsListSerialized.accountsSerialized[emailIndex];
            
            playerData.selectedPetName = account.selectedPetName;
            playerData.selectedBGName = account.selectedBGName;
            playerData.hp = account.hp;
            playerData.food = account.food;
            playerData.money = account.money;
            //playerData.costumeList = account.costumeList; //ALJON
            LoadPlayerTasksFromJSON(account, email);

        }

    }

    public void LoadPlayerTasksFromJSON(AccountSerialized acc,string email) //aljon
    {
        List<TaskListSerialized> tls = acc.tasksListSerialized;
        for (int i = 0; i <  tls.Count ; i++)
        {
            List<Task> t = tls[i].tasks;
            for (int j = 0; j < t.Count; j++)
            {
                playerData.filteredList.tasksList.Add(t[j]);
            }
        }
    }

    public void updateJSONonAction(string email)
    {
        string filePath = System.IO.Directory.GetCurrentDirectory() + "\\";
        string fileName = "data.json";

        string json = File.ReadAllText(filePath + fileName);
        accountsListSerialized = JsonUtility.FromJson<AccountsListSerialized>(json);


        int emailIndex = FindEmailIndexInJSON(accountsListSerialized, email);
        if (emailIndex != -1)
        {
            AccountSerialized account = accountsListSerialized.accountsSerialized[emailIndex];

            account.selectedPetName = playerData.selectedPetName;
            account.selectedBGName = playerData.selectedBGName;
            account.hp = playerData.hp;
            account.food = playerData.food;
            account.money = playerData.money;
            account.costumeList = playerData.costumeList;


            updateJSONfile(accountsListSerialized, filePath, fileName);
        }


    }

    public TaskListSerialized CreateTaskListSerialized(Task curTask) //should be currenttask lang!!!
    {
        TaskListSerialized tls = new TaskListSerialized();
        tls.TaskListID = curTask.taskListId;//currentLoggedInTask.taskListId;
        tls.tasks.Add(CreateNewTaskSerialized(curTask));
        return tls;
    }

    public Task CreateNewTaskSerialized (Task CurrentTask) //should be currenttask lang!!!
    {
        Task t = new Task();
        t.taskListId = CurrentTask.taskListId;
        t.taskId = CurrentTask.taskId;
        t.title = CurrentTask.title;
        t.notes = CurrentTask.notes;
        t.status = CurrentTask.status;
        t.dueDate = CurrentTask.dueDate;

        //t = currentLoggedInTask;


        return t;
    }

    private void updateJSONfile(AccountsListSerialized list, string filepath, string filename)
    {
        string emptyJson2 = JsonUtility.ToJson(list, true);
        Debug.Log("Created JSON data");
        File.WriteAllText(filepath + filename, emptyJson2);
    }

    private int FindEmailIndexInJSON(AccountsListSerialized list, string email)
    {
        if (list == null) return -1;
        for (int i = 0; i < list.accountsSerialized.Count; i++)
        {
            if (list.accountsSerialized[i].Email == email)
            {
                return i;
            }
        }

        return -1;
    }

    
    private int FindTaskListIndexFromIDInJSON(AccountsListSerialized list, int emailindex, string id)
    {
        List<TaskListSerialized> l = list.accountsSerialized[emailindex].tasksListSerialized;
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].TaskListID == id)
            {
                return i;
            }
        }

        return -1;
    }

    private int FindTaskIndexFromIDInJSON(AccountsListSerialized list, int emailindex, int tasklistindedx, string id)
    {
        List<Task> l = list.accountsSerialized[emailindex].tasksListSerialized[tasklistindedx].tasks;

        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].taskId == id)
            {
                return i;
            }
        }

        return -1;
    }





    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                {
                    // AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
                }
            }
            else
            {
                // AddToInformation("Sign In Successful.");
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        
        // AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        // AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    // private void AddToInformation(string str) { infoText.text += "\n" + str; }

    private GoogleSignInUser googleUser;
}