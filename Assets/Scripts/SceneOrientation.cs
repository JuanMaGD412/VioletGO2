using UnityEngine;

public class SceneOrientation : MonoBehaviour
{
    public bool vertical = true;

    void Awake()
    {
        if (vertical)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }
}
