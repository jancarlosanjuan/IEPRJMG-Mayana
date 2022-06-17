using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSignInDemo : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] string webClientId = "<your client id here>";
    [SerializeField] TMP_Text txtpro;
    [SerializeField] TMP_Text googleUserEmailTXT;
    // [SerializeField] Text infoText;

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    public TaskList list;
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
        list.tasksList.Clear();
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

            //call event for comparing
            onUserSuccessfulSignIn.Invoke();
        }
    }

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
                            task.taskId = taskJson["id"];
                            task.title = taskJson["title"];
                            task.notes = taskJson["notes"];
                            task.status = taskJson["status"];
                            task.dueDate = taskJson["due"];

                            list.tasksList.Add(task);
                            txtpro.text += $"\nTitle: {task.title}\nTask ID: {task.taskId}\nNotes: {task.notes}\nStatus: {task.status}\nDue Date: {task.dueDate}";
                        }
                        //call event for comparing
                        
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