using UnityEngine;
using UnityEngine.UIElements;

public enum Tool
{
    Laser,
    Mortar,
    Wall,
    Trash
}

public class GameController : MonoBehaviour
{
    public VisualTreeAsset heartReference;

    Button exitButton;
    private Button restartButton, powerIcon;
    private VisualElement mortarTool, wallTool, trashTool, pauseScreen, hearts, laserTool, level;

    [SerializeField] public Game game;

    public void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pauseScreen = root.Q<VisualElement>("pause-screen");

        exitButton = root.Q<Button>("exit-button");
        exitButton.clicked += ExitButtonPressed;

        restartButton = root.Q<Button>("restart-button");
        restartButton.clicked += RestartButtonPressed;

        laserTool = root.Q<VisualElement>("laser-tool");
        mortarTool = root.Q<VisualElement>("mortar-tool");
        wallTool = root.Q<VisualElement>("wall-tool");
        trashTool = root.Q<VisualElement>("trash-tool");
        level = root.Q<VisualElement>("level");
        powerIcon = root.Q<Button>("power-icon");

        hearts = root.Q<VisualElement>("hearts");
    }

    void ExitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    void RestartButtonPressed()
    {
        game.BeginNewGame();
    }

    public void HideMenu()
    {
        pauseScreen.visible = false;
    }

    public void ShowMenu()
    {
        pauseScreen.visible = true;
    }

    private void ClearSelection()
    {
        wallTool.RemoveFromClassList("selected");
        laserTool.RemoveFromClassList("selected");
        mortarTool.RemoveFromClassList("selected");
        trashTool.RemoveFromClassList("selected");
    }

    public void Select(Tool type)
    {
        ClearSelection();
        switch (type)
        {
            case Tool.Laser:
                laserTool.AddToClassList("selected");
                break;
            case Tool.Mortar:
                mortarTool.AddToClassList("selected");
                break;
            case Tool.Wall:
                wallTool.AddToClassList("selected");
                break;
            case Tool.Trash:
                trashTool.AddToClassList("selected");
                break;
        }
    }

    public void GameUpdate(Player player)
    {
        var offset = player.health - hearts.childCount;
        if (offset > 0)
        {
            for (int i = 0; i < offset; i++)
            {
                var newButton = new Button();
                newButton.AddToClassList("heart");
                hearts.Add(newButton);
            }
        }
        else if (offset < 0)
        {
            for (int i = 0; i > offset; i--)
            {
                hearts.RemoveAt(0);
            }
        }

        var perc = player.power * 100f / player.maxPower;
        level.RemoveFromClassList("info");
        level.RemoveFromClassList("warning");
        level.RemoveFromClassList("error");
        powerIcon.RemoveFromClassList("info");
        powerIcon.RemoveFromClassList("warning");
        powerIcon.RemoveFromClassList("error");
        if (perc < 33)
        {
            level.AddToClassList("error");
            powerIcon.AddToClassList("error");
        }
        else if (perc < 66)
        {
            level.AddToClassList("warning");
            powerIcon.AddToClassList("warning");
        }
        else
        {
            level.AddToClassList("info");   
            powerIcon.AddToClassList("info");   
        }

        level.style.width = new Length(perc, LengthUnit.Percent);
    }
}