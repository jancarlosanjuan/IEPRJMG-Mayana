using Google;
using UnityEngine;


[CreateAssetMenu(fileName = "New Game Manager",
                 menuName = "Scriptable OBJ/ New Game Manager")]
public class GameManager : ScriptableObject
{
    public GoogleSignInUser GoogleUser;
    public string AccessToken;
    public string RefreshToken;
    public string filePath;
    public string fileName;
}
