using Google;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GoogleSignInUser GoogleUser { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
