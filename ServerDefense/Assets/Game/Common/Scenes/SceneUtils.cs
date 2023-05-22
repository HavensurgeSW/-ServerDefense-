using System.Collections.Generic;

public class SceneUtils
{
    public static readonly Dictionary<string, SCENE> scenesEnumDictionary = new Dictionary<string, SCENE>
    {
        { "SplashScreen", SCENE.SPLASH_SCREEN },
        { "MainMenu", SCENE.MAIN_MENU },
        { "Level0", SCENE.LEVEL_0 },
        { "Level1", SCENE.LEVEL_1 },
        { "Tutorial", SCENE.TUTORIAL },
        { "EndScene", SCENE.END_SCENE }
    };

    public static readonly Dictionary<SCENE, string> scenesStringDictionary = new Dictionary<SCENE, string>
    {
        { SCENE.SPLASH_SCREEN, "SplashScreen" },
        { SCENE.MAIN_MENU, "MainMenu" },
        { SCENE.LEVEL_0, "Level0" },
        { SCENE.LEVEL_1, "Level1" },
        { SCENE.TUTORIAL, "Tutorial" },
        { SCENE.END_SCENE, "EndScene" }
    };
}