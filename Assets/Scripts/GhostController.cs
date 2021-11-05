using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GhostController : MonoBehaviour
{
    //PacStudent
    public GameObject pacStudent;
    
    //Ghost game object
    public GameObject brownGhost;
    public GameObject greenGhost;
    public GameObject purpleGhost;
    public GameObject redGhost;

    private int[] tiles = { 1, 2, 3, 4, 7 };
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

    //path for ghost 4
    int[,] pathMap =
            {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
            {2,9,9,9,9,9,9,9,9,9,9,9,9,4},
            {2,9,3,4,4,3,5,3,4,4,4,3,9,4},
            {2,9,4,0,0,4,5,4,0,0,0,4,9,4},
            {2,9,3,4,4,3,5,3,4,4,4,3,9,3},
            {2,9,5,5,5,5,5,5,5,5,5,5,9,9},
            {2,9,3,4,4,3,5,3,3,5,3,4,4,4},
            {2,9,3,4,4,3,5,4,4,5,3,4,4,3},
            {2,9,9,9,9,9,9,4,4,5,5,5,5,4},
            {1,2,2,2,2,1,9,4,3,4,4,3,0,4},
            {0,0,0,0,0,2,9,4,3,4,4,3,0,3},
            {0,0,0,0,0,2,9,4,4,0,0,0,0,0},
            {0,0,0,0,0,2,9,4,4,0,3,4,4,0},
            {2,2,2,2,2,1,9,3,3,0,4,0,0,0},
            {0,0,0,0,0,0,9,0,0,0,4,0,0,0},
            };
    bool pathFound = false;

    //Unit direction vecter3
    Vector3 directionY;
    Vector3 directionX;
    public static Vector3 previousStepBrown;
    public static Vector3 previousStepGreen;
    public static Vector3 previousStepPurple;
    public static Vector3 previousStepRed;

    //tween variable
    public static Tween activeTweenBrown;
    public static Tween activeTweenGreen;
    public static Tween activeTweenPurple;
    public static Tween activeTweenRed;

    //random number;
    int ranNum;
    int minNum = 0;
    int maxNum = 2;

    //ghost status
    public enum ghostStatus { Normal, Scared, Death };
    public static ghostStatus brownStatus = ghostStatus.Normal;
    public static ghostStatus greenStatus = ghostStatus.Normal;
    public static ghostStatus purpleStatus = ghostStatus.Normal;
    public static ghostStatus redStatus = ghostStatus.Normal;
    //public static ghostStatus generalStatus = ghostStatus.Normal;

    //original position
    Vector3 brownPos = new Vector3(-0.5f, 1.5f, 0.0f);
    Vector3 greenPos = new Vector3(0.5f, 1.5f, 0.0f);
    Vector3 purplePos = new Vector3(-0.5f, -0.5f, 0.0f);
    Vector3 redPos = new Vector3(0.5f, -0.5f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ghost 1 (brown) movemnent
        if(brownStatus == ghostStatus.Normal)
        {
            
            Ghost1AddTween(brownGhost, 0.8f);
        }
        else if (brownStatus == ghostStatus.Scared)
        {
            
            Ghost1AddTween(brownGhost, 0.8f);
        }
        else
        {
            Death1AddTween(greenGhost, 3.0f);
        }
        
        //ghost 2 (green) movement
        if(greenStatus == ghostStatus.Normal)
        {
            Ghost2AddTween(greenGhost, 0.8f);
        }else if (greenStatus == ghostStatus.Scared)
        {
            Scared2AddTween(greenGhost, 0.8f);
        }
        else
        {
            Death2AddTween(greenGhost, 3.0f);
        }
        
        //ghost 3 (purple) movement
        if(purpleStatus == ghostStatus.Normal)
        {
            Ghost3AddTween(purpleGhost, 0.8f);
        }
        else if (purpleStatus == ghostStatus.Scared)
        {
            Scared3AddTween(purpleGhost, 0.8f);
        }
        else
        {
            Death3AddTween(purpleGhost, 3.0f);
        }

        //ghost 4 (red) movement
        if (redStatus == ghostStatus.Normal)
        {
            Ghost4AddTween(redGhost, 0.8f);
        }
        else if (redStatus == ghostStatus.Scared)
        {
            Scared4AddTween(redGhost, 0.8f);
        }
        else
        {
            Death4AddTween(redGhost, 3.0f);
        }


        GhostTweening();
    }

    void Ghost1AddTween(GameObject target, float duration)
    {
        float distX = target.transform.position.x - pacStudent.transform.position.x;
        float distY = target.transform.position.y - pacStudent.transform.position.y;

        //process y direction first
        if (distX != 0)
        {
            directionX = new Vector3(distX / Mathf.Abs(distX), 0.0f, 0.0f);
        }
        else
        {
            directionX = new Vector3(1.0f, 0.0f, 0.0f);
        }
        
        if(distY != 0)
        {
            directionY = new Vector3(0.0f, distY / Mathf.Abs(distY), 0.0f);
        }
        else
        {
            directionY = new Vector3(0.0f, 1.0f, 0.0f);
        }

        
        if (CheckWall(target.transform.position+directionY) & CheckArea(target.transform.position + directionY) & previousStepBrown != (target.transform.position + directionY))
        {
            if(activeTweenBrown == null)
            {
                previousStepBrown = target.transform.position;
                activeTweenBrown = new Tween(target.transform, target.transform.position, target.transform.position + directionY, Time.time, duration);
                
            }
        }else if (CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepBrown != (target.transform.position + directionX))
        {
            if (activeTweenBrown == null)
            {
                previousStepBrown = target.transform.position;
                activeTweenBrown = new Tween(target.transform, target.transform.position, target.transform.position + directionX, Time.time, duration);
                
            }
        }else if (CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepBrown != (target.transform.position - directionY))
        {
            if (activeTweenBrown == null)
            {
                previousStepBrown = target.transform.position;
                activeTweenBrown = new Tween(target.transform, target.transform.position, target.transform.position - directionY, Time.time, duration);
                
            }
        }
        else if (CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepBrown != (target.transform.position - directionX))
        {
            if (activeTweenBrown == null)
            {
                previousStepBrown = target.transform.position;
                activeTweenBrown = new Tween(target.transform, target.transform.position, target.transform.position - directionX, Time.time, duration);
                
            }
        }
        else
        {
            if (activeTweenBrown == null)
            {
                activeTweenBrown = new Tween(target.transform, target.transform.position, previousStepBrown, Time.time, duration);
                previousStepBrown = target.transform.position;
                
            }
        }
        
    }

    void Ghost2AddTween(GameObject target, float duration)
    {
        float distX = pacStudent.transform.position.x - target.transform.position.x;
        float distY = pacStudent.transform.position.y - target.transform.position.y;
        //process y direction first
        if (distX != 0)
        {
            directionX = new Vector3(distX / Mathf.Abs(distX), 0.0f, 0.0f);
        }
        else
        {
            directionX = new Vector3(1.0f, 0.0f, 0.0f);
        }

        if (distY != 0)
        {
            directionY = new Vector3(0.0f, distY / Mathf.Abs(distY), 0.0f);
        }
        else
        {
            directionY = new Vector3(0.0f, 1.0f, 0.0f);
        }


        if (CheckWall(target.transform.position + directionY) & CheckArea(target.transform.position + directionY) & previousStepGreen != (target.transform.position + directionY))
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, target.transform.position + directionY, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepGreen != (target.transform.position + directionX))
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, target.transform.position + directionX, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepGreen != (target.transform.position - directionY))
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, target.transform.position - directionY, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepGreen != (target.transform.position - directionX))
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, target.transform.position - directionX, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
        else
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, previousStepGreen, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
    }

    void Ghost3AddTween(GameObject target, float duration)
    {
        ranNum = Random.Range(minNum, maxNum);
        if(ranNum == 0)
        {
            directionX = new Vector3(1.0f, 0.0f, 0.0f);
        }
        else
        {
            directionX = new Vector3(-1.0f, 0.0f, 0.0f);
        }

        ranNum = Random.Range(minNum, maxNum);
        if (ranNum == 0)
        {
            directionY = new Vector3(0.0f, 1.0f, 0.0f);
        }
        else
        {
            directionY = new Vector3(0.0f, -1.0f, 0.0f);
        }
        

        if (CheckWall(target.transform.position + directionY) & CheckArea(target.transform.position + directionY) & previousStepPurple != (target.transform.position + directionY))
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, target.transform.position + directionY, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepPurple != (target.transform.position + directionX))
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, target.transform.position + directionX, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepPurple != (target.transform.position - directionY))
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, target.transform.position - directionY, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepPurple != (target.transform.position - directionX))
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, target.transform.position - directionX, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
        else
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, previousStepPurple, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
    }

    void Ghost4AddTween(GameObject target, float duration)
    {
        directionX = new Vector3(1.0f, 0.0f, 0.0f);
        directionY = new Vector3(0.0f, -1.0f, 0.0f);

        if(FindPath(target.transform.position + directionX))
        {
            pathFound = true;
        }else if(FindPath(target.transform.position - directionX))
        {
            pathFound = true;
        }
        else if (FindPath(target.transform.position + directionY))
        {
            pathFound = true;
        }
        else if (FindPath(target.transform.position - directionY))
        {
            pathFound = true;
        }
        else
        {
            pathFound = false;
        }



        if (pathFound)
        {
            //in these regions, start pointing downward then clockwise pointing 
            if(target.transform.position.x >= 2.0f | (target.transform.position.x <= 0 & target.transform.position.x > -2.0f))
            {
                if(FindPath(target.transform.position + directionY) & CheckWall(target.transform.position + directionY) & CheckArea(target.transform.position + directionY) & previousStepRed != (target.transform.position + directionY))
                {
                    if(activeTweenRed == null)
                    {
                        activeTweenRed = new Tween(target.transform, target.transform.position, (target.transform.position + directionY), Time.time, duration);
                        previousStepRed = target.transform.position;
                    }
                }
                else if(FindPath(target.transform.position - directionX) & CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepRed != (target.transform.position - directionX))
                {
                    if (activeTweenRed == null)
                    {
                        activeTweenRed = new Tween(target.transform, target.transform.position, (target.transform.position - directionX), Time.time, duration);
                        previousStepRed = target.transform.position;
                    }

                }
                else if (FindPath(target.transform.position - directionY) & CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepRed != (target.transform.position - directionY))
                {
                    if (activeTweenRed == null)
                    {
                        activeTweenRed = new Tween(target.transform, target.transform.position, (target.transform.position - directionY), Time.time, duration);
                        previousStepRed = target.transform.position;
                    }

                }
                else if (FindPath(target.transform.position + directionX) & CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepRed != (target.transform.position + directionX))
                {
                    if (activeTweenRed == null)
                    {
                        activeTweenRed = new Tween(target.transform, target.transform.position, (target.transform.position + directionX), Time.time, duration);
                        previousStepRed = target.transform.position;
                    }
                }
            }
            //begin with pointing upward
            else
            {
                if (FindPath(target.transform.position - directionY) & CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepRed != (target.transform.position - directionY))
                {
                    if (activeTweenRed == null)
                    {
                        activeTweenRed = new Tween(target.transform, target.transform.position, (target.transform.position - directionY), Time.time, duration);
                        previousStepRed = target.transform.position;
                    }

                }
                else if (FindPath(target.transform.position + directionX) & CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepRed != (target.transform.position + directionX))
                {
                    if (activeTweenRed == null)
                    {
                        activeTweenRed = new Tween(target.transform, target.transform.position, (target.transform.position + directionX), Time.time, duration);
                        previousStepRed = target.transform.position;
                    }

                }
                else if (FindPath(target.transform.position + directionY) & CheckWall(target.transform.position + directionY) & CheckArea(target.transform.position + directionY) & previousStepRed != (target.transform.position + directionY))
                {
                    if (activeTweenRed == null)
                    {
                        activeTweenRed = new Tween(target.transform, target.transform.position, (target.transform.position + directionY), Time.time, duration);
                        previousStepRed = target.transform.position;
                    }
                }
                else if (FindPath(target.transform.position - directionX) & CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepRed != (target.transform.position - directionX))
                {
                    if (activeTweenRed == null)
                    {
                        activeTweenRed = new Tween(target.transform, target.transform.position, (target.transform.position - directionX), Time.time, duration);
                        previousStepRed = target.transform.position;
                    }
                }
            }
        }
        //if path not found
        else
        {
            if (CheckWall(target.transform.position + directionY) & CheckArea(target.transform.position + directionY) & previousStepRed != (target.transform.position + directionY))
            {
                if (activeTweenRed == null)
                {
                    activeTweenRed = new Tween(target.transform, target.transform.position, target.transform.position + directionY, Time.time, duration);
                    previousStepRed = target.transform.position;
                }
            }
            else if (CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepRed != (target.transform.position + directionX))
            {
                if (activeTweenRed == null)
                {
                    activeTweenRed = new Tween(target.transform, target.transform.position, target.transform.position + directionX, Time.time, duration);
                    previousStepRed = target.transform.position;
                }
            }
            else if (CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepRed != (target.transform.position - directionY))
            {
                if (activeTweenRed == null)
                {
                    activeTweenRed = new Tween(target.transform, target.transform.position, target.transform.position - directionY, Time.time, duration);
                    previousStepRed = target.transform.position;
                }
            }
            else if (CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepRed != (target.transform.position - directionX))
            {
                if (activeTweenRed == null)
                {
                    activeTweenRed = new Tween(target.transform, target.transform.position, target.transform.position - directionX, Time.time, duration);
                    previousStepRed = target.transform.position;
                }
            }
            else
            {
                if (activeTweenRed == null)
                {
                    activeTweenRed = new Tween(target.transform, target.transform.position, previousStepRed, Time.time, duration);
                    previousStepRed = target.transform.position;
                }
            }
        }
    }

    void Scared2AddTween(GameObject target, float duration)
    {
        float distX = target.transform.position.x - pacStudent.transform.position.x;
        float distY = target.transform.position.y - pacStudent.transform.position.y;

        //process y direction first
        if (distX != 0)
        {
            directionX = new Vector3(distX / Mathf.Abs(distX), 0.0f, 0.0f);
        }
        else
        {
            directionX = new Vector3(1.0f, 0.0f, 0.0f);
        }

        if (distY != 0)
        {
            directionY = new Vector3(0.0f, distY / Mathf.Abs(distY), 0.0f);
        }
        else
        {
            directionY = new Vector3(0.0f, 1.0f, 0.0f);
        }


        if (CheckWall(target.transform.position + directionY) & CheckArea(target.transform.position + directionY) & previousStepGreen != (target.transform.position + directionY))
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, target.transform.position + directionY, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepGreen != (target.transform.position + directionX))
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, target.transform.position + directionX, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepGreen != (target.transform.position - directionY))
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, target.transform.position - directionY, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepGreen != (target.transform.position - directionX))
        {
            if (activeTweenGreen == null)
            {
                activeTweenBrown = new Tween(target.transform, target.transform.position, target.transform.position - directionX, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
        else
        {
            if (activeTweenGreen == null)
            {
                activeTweenGreen = new Tween(target.transform, target.transform.position, previousStepGreen, Time.time, duration);
                previousStepGreen = target.transform.position;
            }
        }
    }

    void Scared3AddTween(GameObject target, float duration)
    {
        float distX = target.transform.position.x - pacStudent.transform.position.x;
        float distY = target.transform.position.y - pacStudent.transform.position.y;

        //process y direction first
        if (distX != 0)
        {
            directionX = new Vector3(distX / Mathf.Abs(distX), 0.0f, 0.0f);
        }
        else
        {
            directionX = new Vector3(1.0f, 0.0f, 0.0f);
        }

        if (distY != 0)
        {
            directionY = new Vector3(0.0f, distY / Mathf.Abs(distY), 0.0f);
        }
        else
        {
            directionY = new Vector3(0.0f, 1.0f, 0.0f);
        }


        if (CheckWall(target.transform.position + directionY) & CheckArea(target.transform.position + directionY) & previousStepPurple != (target.transform.position + directionY))
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, target.transform.position + directionY, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepPurple != (target.transform.position + directionX))
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, target.transform.position + directionX, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepPurple != (target.transform.position - directionY))
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, target.transform.position - directionY, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepPurple != (target.transform.position - directionX))
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, target.transform.position - directionX, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
        else
        {
            if (activeTweenPurple == null)
            {
                activeTweenPurple = new Tween(target.transform, target.transform.position, previousStepPurple, Time.time, duration);
                previousStepPurple = target.transform.position;
            }
        }
    }

    void Scared4AddTween(GameObject target, float duration)
    {
        float distX = target.transform.position.x - pacStudent.transform.position.x;
        float distY = target.transform.position.y - pacStudent.transform.position.y;

        //process y direction first
        if (distX != 0)
        {
            directionX = new Vector3(distX / Mathf.Abs(distX), 0.0f, 0.0f);
        }
        else
        {
            directionX = new Vector3(1.0f, 0.0f, 0.0f);
        }

        if (distY != 0)
        {
            directionY = new Vector3(0.0f, distY / Mathf.Abs(distY), 0.0f);
        }
        else
        {
            directionY = new Vector3(0.0f, 1.0f, 0.0f);
        }


        if (CheckWall(target.transform.position + directionY) & CheckArea(target.transform.position + directionY) & previousStepRed != (target.transform.position + directionY))
        {
            if (activeTweenRed == null)
            {
                activeTweenRed = new Tween(target.transform, target.transform.position, target.transform.position + directionY, Time.time, duration);
                previousStepRed = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position + directionX) & CheckArea(target.transform.position + directionX) & previousStepRed != (target.transform.position + directionX))
        {
            if (activeTweenRed == null)
            {
                activeTweenRed = new Tween(target.transform, target.transform.position, target.transform.position + directionX, Time.time, duration);
                previousStepRed = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionY) & CheckArea(target.transform.position - directionY) & previousStepRed != (target.transform.position - directionY))
        {
            if (activeTweenRed == null)
            {
                activeTweenRed = new Tween(target.transform, target.transform.position, target.transform.position - directionY, Time.time, duration);
                previousStepRed = target.transform.position;
            }
        }
        else if (CheckWall(target.transform.position - directionX) & CheckArea(target.transform.position - directionX) & previousStepRed != (target.transform.position - directionX))
        {
            if (activeTweenRed == null)
            {
                activeTweenRed = new Tween(target.transform, target.transform.position, target.transform.position - directionX, Time.time, duration);
                previousStepRed = target.transform.position;
            }
        }
        else
        {
            if (activeTweenRed == null)
            {
                activeTweenRed = new Tween(target.transform, target.transform.position, previousStepRed, Time.time, duration);
                previousStepRed = target.transform.position;
            }
        }
    }

    void Death1AddTween(GameObject target, float duration)
    {
        if (activeTweenBrown == null)
        {
            activeTweenBrown = new Tween(target.transform, target.transform.position, brownPos, Time.time, duration);
        }
    }

    void Death2AddTween(GameObject target, float duration)
    {
        if (activeTweenGreen == null)
        {
            activeTweenGreen = new Tween(target.transform, target.transform.position, greenPos, Time.time, duration);
        }
    }

    void Death3AddTween(GameObject target, float duration)
    {
        if (activeTweenPurple == null)
        {
            activeTweenPurple = new Tween(target.transform, target.transform.position, purplePos, Time.time, duration);
        }
    }

    void Death4AddTween(GameObject target, float duration)
    {
        if (activeTweenPurple == null)
        {
            activeTweenPurple = new Tween(target.transform, target.transform.position, redPos, Time.time, duration);
        }
    }


    void GhostTweening()
    {
        if(activeTweenBrown != null)
        {
            //distance to end position
            float dist = Vector3.Distance(activeTweenBrown.Target.position, activeTweenBrown.EndPos);
            // the fraction t, using the duration divided by elapsed time
            float t = (Time.time - activeTweenBrown.StartTime) / activeTweenBrown.Duration;
            //float cubicT = t * t * t;
            if (dist > 0.1f)
            {
                activeTweenBrown.Target.position = Vector3.Lerp(activeTweenBrown.StartPos, activeTweenBrown.EndPos, t);
            }
            else if (dist <= 0.1f)
            {

                activeTweenBrown.Target.position = activeTweenBrown.EndPos;
                activeTweenBrown = null;
            }
        }

        if (activeTweenGreen != null)
        {
            //distance to end position
            float dist = Vector3.Distance(activeTweenGreen.Target.position, activeTweenGreen.EndPos);
            // the fraction t, using the duration divided by elapsed time
            float t = (Time.time - activeTweenGreen.StartTime) / activeTweenGreen.Duration;
            //float cubicT = t * t * t;
            if (dist > 0.1f)
            {
                activeTweenGreen.Target.position = Vector3.Lerp(activeTweenGreen.StartPos, activeTweenGreen.EndPos, t);
            }
            else if (dist <= 0.1f)
            {

                activeTweenGreen.Target.position = activeTweenGreen.EndPos;
                activeTweenGreen = null;
            }
        }

        if (activeTweenPurple != null)
        {
            //distance to end position
            float dist = Vector3.Distance(activeTweenPurple.Target.position, activeTweenPurple.EndPos);
            // the fraction t, using the duration divided by elapsed time
            float t = (Time.time - activeTweenPurple.StartTime) / activeTweenPurple.Duration;
            //float cubicT = t * t * t;
            if (dist > 0.1f)
            {
                activeTweenPurple.Target.position = Vector3.Lerp(activeTweenPurple.StartPos, activeTweenPurple.EndPos, t);
            }
            else if (dist <= 0.1f)
            {

                activeTweenPurple.Target.position = activeTweenPurple.EndPos;
                activeTweenPurple = null;
            }
        }

        if (activeTweenRed != null)
        {
            //distance to end position
            float dist = Vector3.Distance(activeTweenRed.Target.position, activeTweenRed.EndPos);
            // the fraction t, using the duration divided by elapsed time
            float t = (Time.time - activeTweenRed.StartTime) / activeTweenRed.Duration;
            //float cubicT = t * t * t;
            if (dist > 0.1f)
            {
                activeTweenRed.Target.position = Vector3.Lerp(activeTweenRed.StartPos, activeTweenRed.EndPos, t);
            }
            else if (dist <= 0.1f)
            {

                activeTweenRed.Target.position = activeTweenRed.EndPos;
                activeTweenRed = null;
            }
        }

    }



    bool CheckWall(Vector3 position)
    {
        //Debug.Log(position);
        if (position.y >= 0)
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
            int y = Mathf.FloorToInt(Mathf.Abs(position.y - 1)) + 1;

            if (tiles.Contains(levelMap[15 - y, 14 - x]))
            {
                return false;
            }
        }
        return true;
    }

    bool FindPath(Vector3 position)
    {
        if (position.y >= 0)
        {
            int x = Mathf.FloorToInt(Mathf.Abs(position.x)) + 1;
            int y = Mathf.FloorToInt(Mathf.Abs(position.y)) + 1;

            if (pathMap[15 - y, 14 - x] == 9)
            {
                return true;
            }
            //return true;
        }
        else
        {
            int x = Mathf.FloorToInt(Mathf.Abs(position.x)) + 1;
            int y = Mathf.FloorToInt(Mathf.Abs(position.y - 1)) + 1;

            if (pathMap[15 - y, 14 - x] == 9)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckArea(Vector3 position)
    {
        if(position.x < 3.0f & position.x > -3.0f)
        {
            if(position.y > -1.0f & position.y < 2.0f)
            {
                return false;
            }
        }

        return true;
    }
}
