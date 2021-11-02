using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PacStudentController : MonoBehaviour
{
    [SerializeField]
    private GameObject pacStudent;

    public GameObject particleEffect;
    private string lastInput = string.Empty;
    private Vector3 currentInput;

    //store the wall segement 
    //public Tile[] tiles;
    private int [] tiles = { 1, 2, 3, 4, 7 };

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

    private Tween activeTween;
    Vector3Int adjPosition = Vector3Int.zero;

    public AudioSource pacNotEatPellet;
    public AudioSource pacEatPellet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Use if...else if... to prevent multiple inputs
        if (Input.GetKey(KeyCode.W))
        {
            StoreLastInput("W");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            StoreLastInput("A");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            StoreLastInput("S");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            StoreLastInput("D");
        }

        AddTween(0.8f);
        PacStudentTweening();


    }

    void StoreLastInput(string key)
    {
        //Override the variable when player press different key
        if(!string.Equals(lastInput, key))
        {
            lastInput = key;
            //Debug.Log(lastInput);
        }

    }

    

    void AddTween(float duration)
    {
        if (activeTween == null)
        {
            //activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
            if (string.Equals(lastInput, "W"))
            {
                Vector3 pacNext = pacStudent.transform.position + Vector3.up;
                
                if (CheckWall(pacNext))
                {
                    currentInput = Vector3.up;
                    activeTween = new Tween(pacStudent.transform, pacStudent.transform.position,
                        pacStudent.transform.position + currentInput, Time.time, duration);
                    pacStudent.transform.localRotation = Quaternion.Euler(0, 0, -90);
                }
                else if (CheckWall(pacStudent.transform.position+currentInput))
                {
                    activeTween = new Tween(pacStudent.transform, pacStudent.transform.position,
                        pacStudent.transform.position + currentInput, Time.time, duration);
                }
            }
            else if (string.Equals(lastInput, "A"))
            {
                Vector3 pacNext = pacStudent.transform.position + Vector3.left;
                
                if (CheckWall(pacNext))
                {
                    currentInput = Vector3.left;
                    activeTween = new Tween(pacStudent.transform, pacStudent.transform.position,
                        pacStudent.transform.position + currentInput, Time.time, duration);
                    pacStudent.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    
                }
                else if (CheckWall(pacStudent.transform.position + currentInput))
                {
                    activeTween = new Tween(pacStudent.transform, pacStudent.transform.position,
                        pacStudent.transform.position + currentInput, Time.time, duration);
                }
            }
            else if (string.Equals(lastInput, "S"))
            {
                Vector3 pacNext = pacStudent.transform.position + Vector3.down;
                
                if (CheckWall(pacNext))
                {
                    currentInput = Vector3.down;
                    activeTween = new Tween(pacStudent.transform, pacStudent.transform.position,
                        pacStudent.transform.position + currentInput, Time.time, duration);
                    pacStudent.transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
                else if (CheckWall(pacStudent.transform.position + currentInput))
                {
                    activeTween = new Tween(pacStudent.transform, pacStudent.transform.position,
                        pacStudent.transform.position + currentInput, Time.time, duration);
                }
            }
            else if (string.Equals(lastInput, "D"))
            {
                Vector3 pacNext = pacStudent.transform.position + Vector3.right;
                
                if (CheckWall(pacNext))
                {
                    currentInput = Vector3.right;
                    activeTween = new Tween(pacStudent.transform, pacStudent.transform.position,
                        pacStudent.transform.position + currentInput, Time.time, duration);
                    pacStudent.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else if (CheckWall(pacStudent.transform.position + currentInput))
                {
                    activeTween = new Tween(pacStudent.transform, pacStudent.transform.position,
                        pacStudent.transform.position + currentInput, Time.time, duration);
                }
            }


            
        }
    }

    void PacStudentTweening()
    {
        if (activeTween != null)
        {
            if (!particleEffect.activeSelf)
            {
                particleEffect.SetActive(true);
            }
            
            //distance to end position
            float dist = Vector3.Distance(activeTween.Target.position, activeTween.EndPos);
            // the fraction t, using the duration divided by elapsed time
            float t = (Time.time - activeTween.StartTime) / activeTween.Duration;
            //float cubicT = t * t * t;
            if (dist > 0.1f)
            {

                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, t);

            }
            else if (dist <= 0.1f)
            {
                int x, y;
                if(activeTween.EndPos.y >= 0)
                {
                    x = Mathf.FloorToInt(Mathf.Abs(activeTween.EndPos.x)) + 1;
                    y = Mathf.FloorToInt(Mathf.Abs(activeTween.EndPos.y)) + 1;
                }
                else
                {
                    x = Mathf.FloorToInt(Mathf.Abs(activeTween.EndPos.x)) + 1;
                    y = Mathf.FloorToInt(Mathf.Abs(activeTween.EndPos.y - 1)) + 1;
                }
                

                activeTween.Target.position = activeTween.EndPos;
                activeTween = null;
                
                if(levelMap[15 - y, 14 - x] == 5)
                {
                    pacEatPellet.Play();
                }
                else if (levelMap[15 - y, 14 - x] == 0)
                {
                    pacNotEatPellet.Play();
                }
                
                if (particleEffect.activeSelf)
                {
                    particleEffect.SetActive(false);
                }
            }
        }
    }

    bool CheckWall(Vector3 position)
    {
        if(position.y >= 0)
        {
            int x = Mathf.FloorToInt(Mathf.Abs(position.x)) + 1;
            int y = Mathf.FloorToInt(Mathf.Abs(position.y)) + 1;

            if (tiles.Contains(levelMap[15 - y, 14 - x]))
            {
                return false;
            }
            //return true;
        }
        else
        {
            int x = Mathf.FloorToInt(Mathf.Abs(position.x)) + 1;
            int y = Mathf.FloorToInt(Mathf.Abs(position.y-1))+1;

            if (tiles.Contains(levelMap[15 - y, 14 - x]))
            {
                return false;
            }
        }
        return true;
    }

    void DetectCollision()
    {
        RaycastHit2D hit2d = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), 0.6f);
        if(hit2d)
        {
            Debug.Log("2d cast: " + hit2d.collider.name);
        }
    }
}
