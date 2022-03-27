using UnityEngine;

[SelectionBase]
public class GameTileContent : MonoBehaviour {

    GameTileContentFactory originFactory;
    
    [SerializeField]
    GameTileContentType type = default;

    public GameTileContentType Type => type;
    
    public bool BlocksPath =>
        Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;
    
    public GameTileContentFactory OriginFactory {
        get => originFactory;
        set {
            Debug.Assert(originFactory == null, "Redefined origin factory!");
            originFactory = value;
        }
    }

    public void Recycle () {
        originFactory.Reclaim(this);
    }
    
    public virtual void GameUpdate () {}

    public virtual bool CanBuild(Player player)
    {
        return true;
    }

    public virtual void Build(Player player) { }

    public virtual void Trash(Player player) { }
}