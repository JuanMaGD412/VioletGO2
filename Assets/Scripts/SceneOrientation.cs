using UnityEngine;

public class SceneOrientation : MonoBehaviour
{
    public bool vertical = true;

    void Start()
    {
        if (vertical)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }
}
