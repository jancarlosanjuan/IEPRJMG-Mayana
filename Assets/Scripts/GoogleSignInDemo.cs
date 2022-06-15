using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    // [SerializeField] Text infoText;

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    private List<string> listIDs = new List<string>();
    public List<Task> taskList = new List<Task>();

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
        listIDs.Clear();
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

            // SignInWithGoogleOnFirebase(task.Result.IdToken);
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

            // Requesting all calendar events in PRIMARY calendar
            //string uri = "https://www.googleapis.com/calendar/v3/calendars/calendarId/events?";
            string uri = "https://tasks.googleapis.com/tasks/v1/users/@me/lists";
            //uri += "calendarId=primary";
            using var req2 = UnityWebRequest.Get(uri);
            req2.SetRequestHeader("Authorization", $"Bearer {gameManager.AccessToken}");
            req2.SetRequestHeader("Accept", "application/json");
        
            yield return req2.SendWebRequest();
        
            if (req2.result == UnityWebRequest.Result.Success)
            {
                txtpro.text = "ID's: \n";
                var j2 = SimpleJSON.JSON.Parse(req2.downloadHandler.text);
                int n = j2.Count;

                for (int i = 0; i < n; i++)
                {
                    listIDs.Add(j2["items"][i]["id"]);
                    txtpro.text = txtpro.text + "\n" + j2["items"][i]["id"] + "\n";

                    string bruh = j2["items"][i]["id"];
                    string ts = "https://www.googleapis.com/tasks/v1/lists/" + bruh + "/tasks";
                    using var req3 = UnityWebRequest.Get(ts);
                    req3.SetRequestHeader("Authorization", $"Bearer {gameManager.AccessToken}");
                    req3.SetRequestHeader("Accept", "application/json");

                    yield return req3.SendWebRequest();

                    if (req3.result == UnityWebRequest.Result.Success)
                    {
                        var j3 = SimpleJSON.JSON.Parse(req3.downloadHandler.text);
                        int n2 = j3.Count;

                        for (int j = 0; j < n2; j++)
                        {
                            Task temp = new Task();
                            temp.title = j3["items"][j]["title"];
                            temp.notes = j3["items"][j]["notes"];
                            temp.status = j3["items"][j]["status"];
                            temp.due = j3["items"][j]["due"];
                            taskList.Add(temp);

                            txtpro.text = txtpro.text + "\n" + temp.title + "\n" + temp.notes + "\n" + temp.status + "\n" + temp.due + "\n";
                        }
                        
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