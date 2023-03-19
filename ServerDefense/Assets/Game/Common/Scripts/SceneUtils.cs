using System.Collections.Generic;

public enum SCENE
{
    NONE = -1,
    SPLASH_SCREEN = 0,
    MAIN_MENU = 1,
    LEVEL_0 = 2,
    LEVEL_1 = 3,
    TUTORIAL = 4,
    END_SCENE = 5
}

public class SceneUtils
{
    public static readonly Dictionary<string, SCENE> scenesEnumDictionary = new Dictionary<string, SCENE>
    {
        { "SplashScreen", SCENE.SPLASH_SCREEN },
        { "Menu", SCENE.MAIN_MENU },
        { "Level0", SCENE.LEVEL_0 },
        { "Level1", SCENE.LEVEL_1 },
        { "Tutorial", SCENE.TUTORIAL },
        { "EndScene", SCENE.END_SCENE }
    };

    public static readonly Dictionary<SCENE, string> scenesStringDictionary = new Dictionary<SCENE, string>
    {
        { SCENE.SPLASH_SCREEN, "SplashScreen" },
        { SCENE.MAIN_MENU, "Menu" },
        { SCENE.LEVEL_0, "Level0" },
        { SCENE.LEVEL_1, "Level1" },
        { SCENE.TUTORIAL, "Tutorial" },
        { SCENE.END_SCENE, "EndScene" }
    };
}
