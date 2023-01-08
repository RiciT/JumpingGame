using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgGenerator : MonoBehaviour
{
    public GameObject[] tileVariations;
    public GameObject parent;
    public GameObject player;
    int numberOfTiles = 0;
    public int tileCap = 5;
    GameObject[] tiles = new GameObject[5];
    int nonNullElementsInTiles = 0;
    bool needRespawn = false;
    int last;
    int current;

    private void Start()
    {
        tiles = new GameObject[5];
        current = 2;
        for (int i = 0; i < tileCap; i++)
        {
            tiles[numberOfTiles % tileCap] = Instantiate(tileVariations[current], new Vector3(0, 11.28f * numberOfTiles, 0),
                Quaternion.identity, parent.transform) as GameObject;
            numberOfTiles += 1;
            last = current;
            while (current == last)
            {
                current = Random.Range(0, tileVariations.Length);// - 1);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        nonNullElementsInTiles = 0;

        //Debug.Log()

        foreach (var tile in tiles)
        {
            if (tile != null)
            {
                nonNullElementsInTiles += 1;
            }
        }
        try
        {
            if (needRespawn)
            {
                while (current == last)
                {
                    current = Random.Range(0, tileVariations.Length); //- 1);
                }
                last = current;
                tiles[4] = Instantiate(tileVariations[current], new Vector3(0, 11.28f * numberOfTiles, 0), Quaternion.identity, parent.transform) as GameObject;

                numberOfTiles += 1;
                needRespawn = false;
            }
            else if (tiles.Length == tileCap && player.transform.position.y >= ((numberOfTiles - 3) * 11.28f))
            {
                Destroy(tiles[0]);
                tiles[0] = null;

                for (int i = 1; i < tileCap; i++)
                {
                    tiles[i - 1] = tiles[i];
                }
                needRespawn = true;
            }
        }
        catch (MissingReferenceException) { }
    }
}
