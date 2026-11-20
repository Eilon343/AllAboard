using UnityEngine;

/// <summary>
/// Reuses a small grid of ocean sprites around an orthographic camera.
/// The world does not have screen boundaries, and the player can keep sailing.
/// Requires a visually seamless sea sprite to avoid visible tile seams.
/// </summary>
public sealed class EndlessOcean2D : MonoBehaviour
{
    [Header("Ocean")]
    [Tooltip("An imported, seamless Sprite used to fill the visible sea.")]
    [SerializeField] private Sprite _seaTile;

    [Tooltip("The orthographic Main Camera following the player ship.")]
    [SerializeField] private Camera _camera;

    private Transform[] _tiles;
    private int _columns;
    private int _rows;
    private float _tileWidth;
    private float _tileHeight;
    private int _lastCenterX = int.MinValue;
    private int _lastCenterY = int.MinValue;

    /// <summary>
    /// Allocate only enough tiles to cover the visible camera area plus one
    /// extra tile on each side. Tiles are reused rather than recreated per frame.
    /// </summary>
    private void Start()
    {
        if (_seaTile == null || _camera == null || !_camera.orthographic)
        {
            Debug.LogError("EndlessOcean2D needs a seamless sprite and an orthographic camera.", this);
            enabled = false;
            return;
        }

        // Keep the parent at scale (1,1,1); sprite bounds are in world units.
        _tileWidth = _seaTile.bounds.size.x;
        _tileHeight = _seaTile.bounds.size.y;
        if (_tileWidth <= 0f || _tileHeight <= 0f)
        {
            Debug.LogError("EndlessOcean2D: invalid sprite dimensions.", this);
            enabled = false;
            return;
        }

        float visibleHeight = 2f * _camera.orthographicSize;
        float visibleWidth = visibleHeight * _camera.aspect;
        _columns = 2 * Mathf.CeilToInt(visibleWidth / (2f * _tileWidth)) + 3;
        _rows = 2 * Mathf.CeilToInt(visibleHeight / (2f * _tileHeight)) + 3;
        _tiles = new Transform[_columns * _rows];

        for (int i = 0; i < _tiles.Length; i++)
        {
            GameObject tile = new GameObject($"SeaTile_{i:00}");
            tile.transform.SetParent(transform, false);
            SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
            renderer.sprite = _seaTile;
            renderer.sortingOrder = -100;
            _tiles[i] = tile.transform;
        }

        PositionTiles();
    }

    /// <summary>Reposition the existing grid only when the camera crosses a tile.</summary>
    private void LateUpdate()
    {
        if (_tiles == null) return;
        PositionTiles();
    }

    /// <summary>
    /// Snap grid centers to exact sprite multiples so repeat textures line up.
    /// No new GameObjects are created while the player sails.
    /// </summary>
    private void PositionTiles()
    {
        int centerX = Mathf.RoundToInt(_camera.transform.position.x / _tileWidth);
        int centerY = Mathf.RoundToInt(_camera.transform.position.y / _tileHeight);
        if (centerX == _lastCenterX && centerY == _lastCenterY) return;
        _lastCenterX = centerX;
        _lastCenterY = centerY;

        int index = 0;
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _columns; x++)
            {
                _tiles[index++].position = new Vector3(
                    (centerX + x - _columns / 2) * _tileWidth,
                    (centerY + y - _rows / 2) * _tileHeight,
                    0f);
            }
        }
    }
}
