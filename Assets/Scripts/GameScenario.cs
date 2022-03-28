using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class GameScenario : ScriptableObject
{
    public Tilemap tilemap;

    [SerializeField, Range(1, 10)] int cycles = 1;

    [SerializeField, Range(0f, 1f)] float cycleSpeedUp = 0.5f;

    [SerializeField] EnemyWave[] waves = { };

    [SerializeField] public int StartingPlayerHealth = 10;
    [SerializeField] public int StartingPlayerPower = 5;

    [SerializeField] public Vector2Int BoardSize = new Vector2Int(11, 11);

    public State Begin() => new State(this);

    [System.Serializable]
    public struct State
    {
        GameScenario scenario;
        int cycle, index;
        float timeScale;
        EnemyWave.State wave;

        public State(GameScenario scenario)
        {
            this.scenario = scenario;
            cycle = 0;
            index = 0;
            timeScale = 1.0f;
            Debug.Assert(scenario.waves.Length > 0, "Empty scenario!");
            wave = scenario.waves[0].Begin();
        }

        public bool Progress()
        {
            float deltaTime = wave.Progress(timeScale * Time.deltaTime);
            while (deltaTime >= 0f)
            {
                if (++index >= scenario.waves.Length)
                {
                    if (++cycle >= scenario.cycles && scenario.cycles > 0)
                    {
                        return false;
                    }

                    index = 0;
                    timeScale += scenario.cycleSpeedUp;
                }

                wave = scenario.waves[index].Begin();
                deltaTime = wave.Progress(deltaTime);
            }

            return true;
        }
    }
}