using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] WarFactory warFactory = default;

    [SerializeField] Vector2Int boardSize = new Vector2Int(11, 11);

    [SerializeField] GameBoard board = default;

    [SerializeField] GameTileContentFactory tileContentFactory = default;

    [SerializeField, Range(1f, 10f)] float playSpeed = 1f;

    [SerializeField] GameController gameController = default;

    GameBehaviorCollection enemies = new GameBehaviorCollection();
    GameBehaviorCollection nonEnemies = new GameBehaviorCollection();

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    private Tool selectedTool = Tool.Laser;

    static Game instance;

    [SerializeField] GameScenario scenario = default;

    GameScenario.State activeScenario;

    const float pausedTimeScale = 0f;
    private bool paused = false;
    private Player player;

    public static Shell SpawnShell()
    {
        Shell shell = instance.warFactory.Shell;
        instance.nonEnemies.Add(shell);
        return shell;
    }

    public static Explosion SpawnExplosion()
    {
        Explosion explosion = instance.warFactory.Explosion;
        instance.nonEnemies.Add(explosion);
        return explosion;
    }

    void OnEnable()
    {
        instance = this;
    }

    void Awake()
    {
        player = new Player();
        board.Initialize(boardSize, tileContentFactory);
        board.ShowPaths = false;
        board.ShowGrid = true;
        activeScenario = scenario.Begin();
        player.health = scenario.StartingPlayerHealth;
        player.maxHealth = scenario.StartingPlayerHealth;
        player.power = scenario.StartingPlayerPower;
        player.maxPower = scenario.StartingPlayerPower;
    }

    void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }

        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }

    int GetKeys(params KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                return i;
            }
        }
        return -1;
    }

    void Update()
    {
        gameController.Select(selectedTool);
        gameController.GameUpdate(player);

        if (!paused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                HandleAlternativeTouch();
            }

            int key = GetKeys(
                KeyCode.Alpha1,
                KeyCode.Alpha2,
                KeyCode.Alpha3,
                KeyCode.Alpha4,
                KeyCode.Alpha5,
                KeyCode.Alpha6,
                KeyCode.Alpha7,
                KeyCode.Alpha8,
                KeyCode.Alpha9);
            if (Enum.IsDefined(typeof(Tool),key))
            {
                selectedTool = (Tool) key;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            board.ShowPaths = !board.ShowPaths;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            board.ShowGrid = !board.ShowGrid;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause(true);
        }
        else if (Time.timeScale > pausedTimeScale)
        {
            SetPause(false);
        }

        if (player.health <= 0 && scenario.StartingPlayerHealth > 0)
        {
            Debug.Log("Defeat!");
            BeginNewGame();
        }

        if (!activeScenario.Progress() && enemies.IsEmpty)
        {
            Debug.Log("Victory!");
            BeginNewGame();
            activeScenario.Progress();
        }

        enemies.GameUpdate();
        Physics.SyncTransforms();
        board.GameUpdate();
        nonEnemies.GameUpdate();
    }

    private void SetPause(bool paused)
    {
        if (paused)
        {
            if (!this.paused) gameController.ShowMenu();
            Time.timeScale =
                Time.timeScale > pausedTimeScale ? pausedTimeScale : playSpeed;
            this.paused = true;
        }
        else
        {
            gameController.HideMenu();
            Time.timeScale = playSpeed;
            this.paused = false;
        }
    }

    public static void EnemyReachedDestination()
    {
        instance.player.health -= 1;
    }

    public void BeginNewGame()
    {
        player.health = scenario.StartingPlayerHealth;
        player.maxHealth = scenario.StartingPlayerHealth;
        player.power = scenario.StartingPlayerPower;
        player.maxPower = scenario.StartingPlayerPower;
        enemies.Clear();
        nonEnemies.Clear();
        board.Clear();
        activeScenario = scenario.Begin();
        SetPause(false);
    }

    public static void SpawnEnemy(EnemyFactory factory, EnemyType type)
    {
        GameTile spawnPoint = instance.board.GetSpawnPoint(
            Random.Range(0, instance.board.SpawnPointCount)
        );
        Enemy enemy = factory.Get(type);
        enemy.SpawnOn(spawnPoint);
        instance.enemies.Add(enemy);
    }

    void HandleAlternativeTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                board.ToggleDestination(tile);
            }
            else
            {
                board.ToggleSpawnPoint(tile);
            }
        }
    }

    void HandleTouch()
    {
        GameTile tile = board.GetTile(TouchRay);
        if (tile != null)
        {
            switch (selectedTool)
            {
                case Tool.Laser:
                    board.BuildTower(tile, TowerType.Laser, player);
                    break;
                case Tool.Mortar:
                    board.BuildTower(tile, TowerType.Mortar, player);
                    break;       
                case Tool.Wall:
                    board.BuildWall(tile);
                    break;      
                case Tool.Trash:
                    board.Trash(tile, player);
                    break;
            }
        }

        Debug.Log(player.power);
    }
}