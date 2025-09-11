using UnityEngine;

public class F11Handler : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Se mantiene en todas las escenas
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
                Screen.SetResolution(800, 600, false); 
            }
            else
            {
                Screen.fullScreen = true;
            }
        }
    }
}
