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
    private Button restartButton;
    private VisualElement mortarTool, wallTool, trashTool, pauseScreen, hearts, laserTool;

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
    }
}