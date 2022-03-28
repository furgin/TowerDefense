using System;
using UnityEngine;
using UnityEngine.Experimental.AI;
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
    private static Texture2D toTexture2D(RenderTexture renderTexture)
    {
        Texture2D tex = new Texture2D(
            renderTexture.width,
            renderTexture.height,
            TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        return tex;
    }

    public class Indicator
    {
        private RenderTexture renderTexture;
        private Image indicator;
        private Texture2D original;
        private ComputeShader indicatorShader;
        private Color fillColor;
        private Color backgroundColor;

        public Indicator(ComputeShader indicatorShader,
            Color fillColor, Color backgroundColor,
            Image indicator)
        {
            this.indicatorShader = indicatorShader;
            this.fillColor = fillColor;
            this.backgroundColor = backgroundColor;
            this.indicator = indicator;
            this.original = indicator.resolvedStyle.backgroundImage.texture;

            renderTexture = new RenderTexture(512, 512, 32);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }

        public void Update(float level, float max)
        {
            int val = (int) (level * 512f / max);
            Debug.Log(val);
            indicatorShader.SetInt("_Level", val);
            indicatorShader.SetVector("_FillColor", fillColor);
            indicatorShader.SetVector("_BackgroundColor", backgroundColor);
            indicatorShader.SetTexture(0, "_Original", original);
            indicatorShader.SetTexture(0, "Result", renderTexture);
            indicatorShader.Dispatch(0, 1 + renderTexture.width / 8, 1 + renderTexture.height / 8, 1);

            indicator.style.backgroundImage = new StyleBackground(toTexture2D(renderTexture));
        }
    }

    [SerializeField] public ComputeShader indicatorShader = default;

    [SerializeField] public Color powerFillColor, powerBackgroundColor = default;
    [SerializeField] public Color healthFillColor, healthBackgroundColor = default;

    Button exitButton;
    private Button restartButton, powerIcon;
    private Indicator healthIndicator, powerIndicator;
    private VisualElement mortarTool, wallTool, trashTool, pauseScreen, laserTool;

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

        healthIndicator = new Indicator(indicatorShader,
            healthFillColor, healthBackgroundColor,
            root.Q<Image>("health"));
        powerIndicator = new Indicator(indicatorShader,
            powerFillColor, powerBackgroundColor,
            root.Q<Image>("power")
        );
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
        powerIndicator.Update(player.power, player.maxPower);
        healthIndicator.Update(player.health, player.maxHealth);
    }
}