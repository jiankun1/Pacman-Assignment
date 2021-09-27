using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelGenerator : MonoBehaviour
{
    int[,] levelMap =
            {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
            {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
            {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
            {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
            };
    
    //array for 6 different tiles and one animated tile
    public Tile[] tiles;
    public AnimatedTile anTile;
    //Tilemap topLeft = new Tilemap();
    //GameObject autoGrid = new GameObject("autoGrid");
    private Vector3Int startPos = new Vector3Int(-14, 14, 0);
    //the variables are used for saving 7 types of tile base and then used for rotating the tiles
    private TileBase set1, set2, set3, set4, set5, set6, set7;
    
    // Start is called before the first frame update
    void Start()
    {
        //Deactive the manual level 
        GameObject.Find("ManualGrid").SetActive(false);
        //create new gameobject grid
        var autoGrid = new GameObject("Grid");
        autoGrid.AddComponent<Grid>();
        var topLeft = new GameObject("Tilemap_TopLeft");
        //make autoGrid as parent of topLeft
        topLeft.transform.parent = autoGrid.transform;
        //Add tilemap and tilemap renderer
        topLeft.AddComponent<Tilemap>();
        topLeft.AddComponent<TilemapRenderer>();
        //set the topright cornor tile which is the first tile to be set
        topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(-13, 14, 0), tiles[0]);

        Tilemap topLeftTileMap = topLeft.GetComponent<Tilemap>();
        //get the dimension of levelmap array
        int col = levelMap.GetLength(0);
        int row = levelMap.GetLength(1);

        //Loop through the level map matrix and set the corresponding tiles 
        for (int y = 0; y < col; y++)
        {
            for (int x= 0; x < row; x++)
            {
                switch (levelMap[y, x])
                {
                    case 0:
                        
                        break;
                    case 1:
                        topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0), tiles[0]);
                        set1 = topLeftTileMap.GetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0));
                        break;
                    case 2:
                        topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0), tiles[1]);
                        set2 = topLeftTileMap.GetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0));
                        break;
                    case 3:
                        topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0), tiles[2]);
                        set3 = topLeftTileMap.GetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0));
                        break;
                    case 4:
                        topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0), tiles[3]);
                        set4 = topLeftTileMap.GetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0));
                        break;
                    case 5:
                        topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0), tiles[4]);
                        set5 = topLeftTileMap.GetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0));
                        break;
                    case 6:
                        topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0), anTile);
                        set6 = topLeftTileMap.GetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0));
                        break;
                    case 7:
                        topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0), tiles[5]);
                        set7 = topLeftTileMap.GetTile(new Vector3Int(startPos.x + x, startPos.y - y, 0));
                        break;
                }

            }
        }

        //variables up, right, down and left are used to detect the neibour of the current tile
        Vector3Int up = new Vector3Int(0, 1, 0);
        Vector3Int right = new Vector3Int(1, 0, 0);
        Vector3Int down = new Vector3Int(0, -1, 0);
        Vector3Int left = new Vector3Int(-1, 0, 0);
        //the following three variables are used to add correct rotation to each tile
        Matrix4x4 antiClock90 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 90.0f), Vector3.one);
        Matrix4x4 antiClock180 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 180.0f), Vector3.one);
        Matrix4x4 clockWise90 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, -90.0f), Vector3.one);
        //the list is used for testing the tile 3
        List<TileBase> checkFor3 = new List<TileBase>();
        checkFor3.Add(set3);
        checkFor3.Add(set4);


        for (int y = 0; y < col; y++)
        {
            for (int x = 0; x < row; x++)
            {

                Vector3Int currentPos = new Vector3Int(x, -y, 0);
                switch (levelMap[y, x])
                {
                    case 0:
                        break;
                    case 1:
                        //up and right has the second segment
                        if (topLeftTileMap.GetTile(startPos + currentPos + up) == set2 & topLeftTileMap.GetTile(startPos + currentPos + right) == set2)
                        {

                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, antiClock90);
                        }
                        //if left and down has wall segment
                        else if (topLeftTileMap.GetTile(startPos + currentPos + left) == set2 & topLeftTileMap.GetTile(startPos + currentPos + down) == set2)
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, clockWise90);
                            //topLeft.GetComponent<Tilemap>().SetTransformMatrix(new Vector3Int(startPos.x + x, startPos.y - y, 0), clockWise90);
                        }
                        //if left and up direction has wall segment
                        else if (topLeftTileMap.GetTile(startPos + currentPos + left) == set2 & topLeftTileMap.GetTile(startPos + currentPos + up))
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, antiClock180);
                        }
                        break;
                    case 2:
                        //if up and down has wall segment
                        if (topLeftTileMap.HasTile(startPos + currentPos + up) && topLeftTileMap.HasTile(startPos + currentPos + down))
                        {

                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, antiClock90);
                        }
                        break;
                    case 3:
                        //up and right has wall segment 4 and top-right direction has a pellet or empty space
                        if (topLeftTileMap.GetTile(startPos + currentPos + up) == set4 & topLeftTileMap.GetTile(startPos + currentPos + right) == set4 & (checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + new Vector3Int(1,1,0))) == false))
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, antiClock90);
                        }
                        // right and down has wall segment 4 and bottom-right direction has a pellet or empty space
                        else if (topLeftTileMap.GetTile(startPos + currentPos + right) == set4 & topLeftTileMap.GetTile(startPos + currentPos + down) == set4 & (checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + new Vector3Int(1, -1, 0))) == false))
                        {
                            //do nothing
                        }
                        //left and down has wall segment 4 and bottom-left has a pellet or empty space
                        else if (topLeftTileMap.GetTile(startPos + currentPos + left) == set4 & topLeftTileMap.GetTile(startPos + currentPos + down) == set4 & (checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + new Vector3Int(-1, -1, 0))) == false))
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, clockWise90);
                        }
                        //left and up has wall segment 4 and top-left has a pellet or empty space
                        else if (topLeftTileMap.GetTile(startPos + currentPos + left) == set4 & topLeftTileMap.GetTile(startPos + currentPos + up) == set4 & (checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + new Vector3Int(-1, 1, 0))) == false))
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, antiClock180);
                        }
                        //if left and down has wall segment 3 or 4
                        else if (checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + left)) & checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + down))) 
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, clockWise90);
                        }
                        //if up and right has wall segments
                        else if(checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + up)) & checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + right)))
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, antiClock90);
                        }
                        //if up and left has wall segments
                        else if (checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + up)) & checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + left)))
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, antiClock180);
                        }
                        //if up has wall segment but left diraction doesn't have
                        else if (checkFor3.Contains(topLeftTileMap.GetTile(startPos+currentPos+up)) & (checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + left)) == false))
                        {
                            topLeftTileMap.SetTransformMatrix(startPos + currentPos, antiClock90);
                        }
                        break;
                    case 4:
                        //up and down has wall segment 3 or 4, or up has wall segment but left and right direction doesn't have
                        if ((checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + up)) & checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + down)) )| topLeftTileMap.GetTile(startPos + currentPos + up) == set7 | (checkFor3.Contains(topLeftTileMap.GetTile(startPos + currentPos + up)) & (topLeftTileMap.GetTile(startPos+currentPos+right) == false) & (topLeftTileMap.GetTile(startPos + currentPos + left) == false)))
                        {
                            topLeftTileMap.SetTransformMatrix(startPos+currentPos, clockWise90);
                        }

                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                }


            }


        }


        //duplicate the top left quadrant
        //clone the topleft gameobject to top tight
        GameObject topRight = Instantiate(topLeft, autoGrid.transform);
        //rename it
        topRight.name = "Tilemap_TopRight";
        //rotate 180 degree around y axis
        topRight.transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);

        //copy the gameobject to bottom left
        GameObject botLeft = Instantiate(topLeft, autoGrid.transform);
        botLeft.name = "Tilemap_BottomLeft";
        botLeft.transform.rotation = new Quaternion(180.0f, 0.0f, 0.0f, 0.0f);
        botLeft.transform.position = new Vector3(0.0f, 1.0f, 0.0f);

        //copy the gameobject to bottom right
        GameObject botRight = Instantiate(topLeft, autoGrid.transform);
        botRight.name = "Tilemap_BottomRight";
        botRight.transform.rotation = new Quaternion(0.0f, 0.0f, 180.0f, 0.0f);
        botRight.transform.position = new Vector3(0.0f, 1.0f, 0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
