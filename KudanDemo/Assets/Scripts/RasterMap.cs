using UnityEngine;
using Mapbox.Map;
using Mapbox.Unity;
using Mapbox.Utils;
using System;

public class RasterMap : MonoBehaviour, Mapbox.Utils.IObserver<RasterTile>
{
    public float zoom = 5f;
    public float tileCoordinateX = 0f;
    public float tileCoordinateY = 51f;

    void Start()
    {
        var map = new Map<RasterTile>(MapboxAccess.Instance);
        map.Zoom = 2;
        map.Vector2dBounds = Vector2dBounds.World();
        map.MapId = "mapbox://styles/mapbox/satellite-streets-v10";
        map.Subscribe(this);
        map.Update();
    }

    public void OnNext(RasterTile tile)
    {
        if (tile.CurrentState == Tile.State.Loaded)
        {
            if (tile.HasError)
            {
                Debug.Log("RasterMap: " + tile.ExceptionsAsString);
                return;
            }

            var tileQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            tileQuad.transform.SetParent(transform);
            tileQuad.name = tile.Id.ToString();
            tileQuad.transform.position = new Vector3(tile.Id.X, -tile.Id.Y, 0);
            var texture = new Texture2D(0, 0);
            texture.LoadImage(tile.Data);
            var material = new Material(Shader.Find("Unlit/Texture"));
            material.mainTexture = texture;
            tileQuad.GetComponent<MeshRenderer>().sharedMaterial = material;
        }
    }

    //public void Update()
    //{
    //    var parameters = new Tile.Parameters();
    //    parameters.Fs = MapboxAccess.Instance;
    //    parameters.Id = new CanonicalTileId((int)zoom, (int)tileCoordinateX, (int)tileCoordinateY);
    //    parameters.MapId = "mapbox://styles/mapbox/satellite-v9";
    //    var rasterTile = new RasterTile();

    //    // Make the request.
    //    rasterTile.Initialize(parameters, (Action)(() =>
    //    {
    //        if (!string.IsNullOrEmpty(rasterTile.Error))
    //        {
    //            // Handle the error.
    //        }

    //        // Consume the Data.
    //    }));

    //}
}