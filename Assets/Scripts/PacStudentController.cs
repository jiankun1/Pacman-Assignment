using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.UI;

public class PacStudentController : MonoBehaviour
{
    [SerializeField]
    private GameObject pacStudent;

    public GameObject particleEffect;
    public GameObject collideWallEffect;
    private string lastInput = string.Empty;
    private Vector3 currentInput;

    //number of pellets
    private int numPellets = 222;

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
    private Vector3 leftTeleport = new Vector3(-13.5f, 0.5f, 0.0f);
    private Vector3 rightTeleport = new Vector3(13.5f, 0.5f, 0.0f);

    //Audio
    public AudioSource pacNotEatPellet;
    public AudioSource pacEatPellet;
    public AudioSource hitWallSound;
    public AudioSource startSound;
    public AudioSource backgroundSound;
    public AudioSource ghostScaredSound;
    public AudioSource pacStuDeathSound;
    public AudioSource ghostDeathSound;

    //UI
    public GameObject ghostTimer;

    //Ghost gameobject
    private enum ghostState { Normal, Scared, Death};
    private ghostState brownState = ghostState.Normal;
    private ghostState greenState = ghostState.Normal;
    private ghostState purpleState = ghostState.Normal;
    private ghostState redState = ghostState.Normal;
    private ghostState generalState = ghostState.Normal;

    public GameObject brownGhost;
    public GameObject greenGhost;
    public GameObject purpleGhost;
    public GameObject redGhost;

    public RuntimeAnimatorController scaredGhost;
    public RuntimeAnimatorController recoveringGhost;
    public RuntimeAnimatorController ghostDeathController;

    public RuntimeAnimatorController brownNormal;
    public RuntimeAnimatorController greenNormal;
    public RuntimeAnimatorController purpleNormal;
    public RuntimeAnimatorController redNormal;

    //Ghost timer
    private float ghostDuration;

    //life indicator
    private int numLives = 3;
    public RuntimeAnimatorController pacStudentDeathController;
    public RuntimeAnimatorController pacStudentNormalController;
    public GameObject pacStuDieEffect;

    //Game stage
    private enum gameStage { Start, Finish};
    private gameStage currentGameStatus;

    //Game timer
    public Text gameTimer;
    private float gameDuration = 0.0f;

    //Save score and time
    private int scorevalue = 0;
    public SaveGameManager saver;

    public RoundStartController roundContorller;

    //power pills
    int numPowerPills = 4;


    // Start is called before the first frame update
    void Start()
    {
        currentGameStatus = gameStage.Start;
    }

    // Update is called once per frame
    void Update()
    {
        //check the game state, running or finish
        if(currentGameStatus == gameStage.Start)
        {
            gameTimer.text = "Time:\n" + TimeSpan.FromSeconds(gameDuration).ToString("mm\\:ss\\:ff");
            gameDuration += Time.deltaTime;

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

            AddTween(0.5f);
            PacStudentTweening();

            GhostTimerCountdown();
            DetectGhostCollision();

            //Backgrounf music
            if (generalState == ghostState.Normal)
            {
                StartCoroutine(BackgroundMusic());
            }
            else if (generalState == ghostState.Scared)
            {
                StartCoroutine(ScaredMusic());
            }
            else if (generalState == ghostState.Death)
            {
                StartCoroutine(GhostDeathMusic());
            }
        }
        else if (currentGameStatus == gameStage.Finish)
        {
            //save the high score and retrun to start scene
            saver.SaveScoreandTime(scorevalue, TimeSpan.FromSeconds(gameDuration).ToString("mm\\:ss\\:ff"));
            roundContorller.FinishGame();
        }
        
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
        //add to activeTween and save the currentInput, check if next grid is walkable
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
        //Tween pacstudent, in final step, check pellets, teleporters and the facing wall,
        //then play sound and make pellet diappeared.
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
                
                
                if (levelMap[15 - y, 14 - x] == 5 )
                {
                    pacEatPellet.Play();
                    numPellets--;
                    //Debug.Log(numPellets);
                }
                else if (levelMap[15 - y, 14 - x] == 0)
                {
                    pacNotEatPellet.Play();
                }else if (levelMap[15 - y, 14 - x] == 6)
                {
                    EatPowerPills();
                    numPellets--;
                }

                //if all pellets are eaten
                if(numPellets == 0)
                {
                    currentGameStatus = gameStage.Finish;
                }
                
                if (particleEffect.activeSelf)
                {
                    particleEffect.SetActive(false);
                }
                //Teleporters:
                if(pacStudent.transform.position == leftTeleport)
                {
                    pacStudent.transform.position = new Vector3(12.5f, 0.5f, 0.0f);
                }else if (pacStudent.transform.position == rightTeleport)
                {
                    pacStudent.transform.position = new Vector3(-12.5f, 0.5f, 0.0f);
                }


                //DetectPelletCollision();
                //DetectWallCollision();
                StartCoroutine(DetectPelletsandWall());
                
            }
        }
    }

    //Check if the position is walkable
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

    //Use RaycastHit2D to detect wall, set the range to 0.5 which is just the distance from pacstudent to wall
    void DetectWallCollision()
    {
        RaycastHit2D hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.left), 0.5f);
        if (currentInput == Vector3.up)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.up), 0.5f);
        }else if (currentInput == Vector3.right)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.right), 0.5f);
        }else if (currentInput == Vector3.down)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.down), 0.5f);
        }

        //RaycastHit2D hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.left), 0.5f);
        if (hit2d)
        {
            //Debug.Log("2d cast: " + hit2d.collider.name);
            if (hit2d.distance == 0.5f)
            {
                //Debug.Log("Hit wall");
                //Debug.Log("hitpoint: " + hit2d.point);
                StartCoroutine(WallCollisionEffect());
                hitWallSound.Play();
            }
        }
    }

    //Use raycast2D to detect pellets, when just finish tweening, if the collision distance is 0, it must be pellets.
    //Then make the tile to null and matrix to 0
    void DetectPelletCollision()
    {
        RaycastHit2D hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.left), 0.5f);
        if (currentInput == Vector3.up)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.up), 0.5f);
        }
        else if (currentInput == Vector3.right)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.right), 0.5f);
        }
        else if (currentInput == Vector3.down)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.down), 0.5f);
        }

        if (hit2d)
        {
            //Debug.Log("2d cast: " + hit2d.collider.name);
            if(hit2d.distance == 0.0f)
            {
                //Debug.Log("start eat pellet");
                EatPellets(hit2d.point);
            }
        }
    }

    //Make matrix location to 0 and make tile to null
    void EatPellets(Vector2 pos)
    {
        if(GameObject.Find("Grid")!= null)
        {
            //pos.y >= 0
            if (pos.y >= 0)
            {
                int x = Mathf.FloorToInt(Mathf.Abs(pos.x));
                int y = Mathf.FloorToInt(Mathf.Abs(pos.y));
                if (pos.x < 0)
                {
                    GameObject topLeft = GameObject.Find("Tilemap_TopLeft");
                    //Debug.Log("eated");
                    topLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(-x - 1, y, 0), null);
                    if (levelMap[14 - y, 13 - x] == 5)
                    {
                        levelMap[14 - y, 13 - x] = 0;
                        AddPoints(10);
                    }else if (levelMap[14 - y, 13 - x] == 6)
                    {
                        numPowerPills--;
                        if(numPowerPills == 0)
                        {
                            levelMap[14 - y, 13 - x] = 0;
                        }
                    }
                    
                }
                //pos.x >0
                else
                {
                    GameObject topRight = GameObject.Find("Tilemap_TopRight");
                    topRight.GetComponent<Tilemap>().SetTile(new Vector3Int(-x - 1, y, 0), null);
                    if (levelMap[14 - y, 13 - x] == 5)
                    {
                        levelMap[14 - y, 13 - x] = 0;
                        AddPoints(10);
                    }
                    else if (levelMap[14 - y, 13 - x] == 6)
                    {
                        numPowerPills--;
                        if (numPowerPills == 0)
                        {
                            levelMap[14 - y, 13 - x] = 0;
                        }
                    }
                }
            }
            //pos.y < 0
            else
            {
                int x = Mathf.FloorToInt(Mathf.Abs(pos.x));
                int y = Mathf.FloorToInt(Mathf.Abs(pos.y - 1));
                if(pos.x < 0)
                {
                    GameObject botLeft = GameObject.Find("Tilemap_BottomLeft");
                    botLeft.GetComponent<Tilemap>().SetTile(new Vector3Int(-x - 1, y, 0), null);
                    if (levelMap[14 - y, 13 - x] == 5)
                    {
                        levelMap[14 - y, 13 - x] = 0;
                        AddPoints(10);
                    }
                    else if (levelMap[14 - y, 13 - x] == 6)
                    {
                        numPowerPills--;
                        if (numPowerPills == 0)
                        {
                            levelMap[14 - y, 13 - x] = 0;
                        }
                    }
                }
                //pos.x > 0
                else
                {
                    GameObject botRight = GameObject.Find("Tilemap_BottomRight");
                    botRight.GetComponent<Tilemap>().SetTile(new Vector3Int(-x - 1, y, 0), null);
                    if (levelMap[14 - y, 13 - x] == 5)
                    {
                        levelMap[14 - y, 13 - x] = 0;
                        AddPoints(10);
                    }
                    else if (levelMap[14 - y, 13 - x] == 6)
                    {
                        numPowerPills--;
                        if (numPowerPills == 0)
                        {
                            levelMap[14 - y, 13 - x] = 0;
                        }
                    }
                }
            }
            
        }
        
    }

    //Add point to score in UI
    void AddPoints(int point)
    {
        scorevalue += point;
        var text = GameObject.Find("Score").GetComponent<Text>();
        string[] word = text.text.Split('\n');
        text.text = word[0] + '\n' + (scorevalue).ToString();
    }

    //detect ghost using raycast2D, pointing to the direction PacStudent facing
    void DetectGhostCollision()
    {
        RaycastHit2D hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.left), 0.5f);
        if (currentInput == Vector3.up)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.up), 0.5f);
        }
        else if (currentInput == Vector3.right)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.right), 0.5f);
        }
        else if (currentInput == Vector3.down)
        {
            hit2d = Physics2D.Raycast(pacStudent.transform.position, transform.TransformDirection(Vector2.down), 0.5f);
        }
        //if hit any normal-state ghost, set tween to null, perform particle effect, check lives
        if (hit2d)
        {
            //Hit ghost brown
            if(hit2d.collider.name == "Ghost Brown")
            {
                if (brownState == ghostState.Normal)
                {
                    activeTween = null;
                    lastInput = null;
                    currentInput = Vector3.zero;
                    StartCoroutine(PacStudentDeath());
                    CheckPacStuLives();
                }else if (brownState == ghostState.Scared)
                {
                    brownGhost.GetComponent<Animator>().runtimeAnimatorController = ghostDeathController;
                    brownState = ghostState.Death;
                    Destroy(brownGhost.GetComponent<BoxCollider2D>());
                    AddPoints(300);
                    generalState = ghostState.Death;
                    //ghost controller
                    GhostController.brownStatus = GhostController.ghostStatus.Death;
                    GhostController.activeTweenBrown = null;
                    StartCoroutine(GhostBackToNoraml());
                }
            }

            //hit ghost green
            if (hit2d.collider.name == "Ghost Green")
            {
                if (greenState == ghostState.Normal)
                {
                    activeTween = null;
                    lastInput = null;
                    currentInput = Vector3.zero;
                    StartCoroutine(PacStudentDeath());
                    CheckPacStuLives();
                }
                else if (greenState == ghostState.Scared)
                {
                    greenGhost.GetComponent<Animator>().runtimeAnimatorController = ghostDeathController;
                    greenState = ghostState.Death;
                    Destroy(greenGhost.GetComponent<BoxCollider2D>());
                    AddPoints(300);
                    generalState = ghostState.Death;
                    //ghost controller
                    GhostController.greenStatus = GhostController.ghostStatus.Death;
                    GhostController.activeTweenGreen = null;
                    StartCoroutine(GhostBackToNoraml());
                }
            }

            //hit ghost purple
            if (hit2d.collider.name == "Ghost Purple")
            {
                if (purpleState == ghostState.Normal)
                {
                    activeTween = null;
                    lastInput = null;
                    currentInput = Vector3.zero;
                    StartCoroutine(PacStudentDeath());
                    CheckPacStuLives();
                }
                else if (purpleState == ghostState.Scared)
                {
                    purpleGhost.GetComponent<Animator>().runtimeAnimatorController = ghostDeathController;
                    purpleState = ghostState.Death;
                    Destroy(purpleGhost.GetComponent<BoxCollider2D>());
                    AddPoints(300);
                    generalState = ghostState.Death;
                    //ghost controller
                    GhostController.purpleStatus = GhostController.ghostStatus.Death;
                    GhostController.activeTweenPurple = null;
                    StartCoroutine(GhostBackToNoraml());
                }
            }

            //hit ghost red
            if (hit2d.collider.name == "Ghost Red")
            {
                if (redState == ghostState.Normal)
                {
                    activeTween = null;
                    lastInput = null;
                    currentInput = Vector3.zero;
                    StartCoroutine(PacStudentDeath());
                    CheckPacStuLives();
                }
                else if (redState == ghostState.Scared)
                {
                    redGhost.GetComponent<Animator>().runtimeAnimatorController = ghostDeathController;
                    redState = ghostState.Death;
                    Destroy(redGhost.GetComponent<BoxCollider2D>());
                    AddPoints(300);
                    generalState = ghostState.Death;
                    //ghost controller
                    GhostController.redStatus = GhostController.ghostStatus.Death;
                    GhostController.activeTweenRed = null;
                    StartCoroutine(GhostBackToNoraml());
                }
            }
        }
    }

    //check pacStudent lives, if less than 0, set game status to finish
    void CheckPacStuLives()
    {
        if (numLives > 0)
        {
            string indicatorName = "Life" + numLives.ToString();
            GameObject.Find(indicatorName).SetActive(false);
            numLives -= 1;
            pacStuDeathSound.Play();
        }
        else
        {
            currentGameStatus = gameStage.Finish;
        }
    }

    //if eat power pellets, change annimator for ghost, set ghost state, and duration of scared state
    void EatPowerPills()
    {
        if (!ghostTimer.activeSelf)
        {
            ghostTimer.SetActive(true);

        }
        //change animator
        brownGhost.GetComponent<Animator>().runtimeAnimatorController = scaredGhost;
        greenGhost.GetComponent<Animator>().runtimeAnimatorController = scaredGhost;
        purpleGhost.GetComponent<Animator>().runtimeAnimatorController = scaredGhost;
        redGhost.GetComponent<Animator>().runtimeAnimatorController = scaredGhost;

        //change state
        brownState = ghostState.Scared;
        greenState = ghostState.Scared;
        purpleState = ghostState.Scared;
        redState = ghostState.Scared;
        generalState = ghostState.Scared;
        
        //set timer
        ghostDuration = 10.0f;
        //ghost controller
        GhostController.previousStepBrown = Vector3.zero;
        GhostController.previousStepGreen = Vector3.zero;
        GhostController.previousStepPurple = Vector3.zero;
        GhostController.previousStepRed = Vector3.zero;
        GhostController.brownStatus = GhostController.ghostStatus.Scared;
        GhostController.greenStatus = GhostController.ghostStatus.Scared;
        GhostController.purpleStatus = GhostController.ghostStatus.Scared;
        GhostController.redStatus = GhostController.ghostStatus.Scared;
        
    }

    //If ghost timer active, change animator in recovering state, set ghost state to normal if time is up
    void GhostTimerCountdown()
    {
        if (ghostTimer.activeSelf)
        {
            
            if (ghostDuration <= 3.0f & ghostDuration > 0.0f)
            {
                if(brownState == ghostState.Scared)
                {
                    brownGhost.GetComponent<Animator>().runtimeAnimatorController = recoveringGhost;
                }
                if(greenState == ghostState.Scared)
                {
                    greenGhost.GetComponent<Animator>().runtimeAnimatorController = recoveringGhost;
                }
                if(purpleState == ghostState.Scared)
                {
                    purpleGhost.GetComponent<Animator>().runtimeAnimatorController = recoveringGhost;
                }
                if(redState == ghostState.Scared)
                {
                    redGhost.GetComponent<Animator>().runtimeAnimatorController = recoveringGhost;
                }
                
            }
            else if(ghostDuration <= 0.0f)
            {
                if (brownState == ghostState.Scared)
                {
                    brownGhost.GetComponent<Animator>().runtimeAnimatorController = brownNormal;
                    brownState = ghostState.Normal;
                }
                if (greenState == ghostState.Scared)
                {
                    greenGhost.GetComponent<Animator>().runtimeAnimatorController = greenNormal;
                    greenState = ghostState.Normal;
                }
                if (purpleState == ghostState.Scared)
                {
                    purpleGhost.GetComponent<Animator>().runtimeAnimatorController = purpleNormal;
                    purpleState = ghostState.Normal;
                }
                if (redState == ghostState.Scared)
                {
                    redGhost.GetComponent<Animator>().runtimeAnimatorController = redNormal;
                    redState = ghostState.Normal;
                }

                //generalState = ghostState.Normal;
                //ghost controller
                GhostController.brownStatus = GhostController.ghostStatus.Normal;
                GhostController.greenStatus = GhostController.ghostStatus.Normal;
                GhostController.purpleStatus = GhostController.ghostStatus.Normal;
                GhostController.redStatus = GhostController.ghostStatus.Normal;
                GhostController.previousStepBrown = Vector3.zero;
                GhostController.previousStepGreen = Vector3.zero;
                GhostController.previousStepPurple = Vector3.zero;
                GhostController.previousStepRed = Vector3.zero;
            }

            ghostTimer.GetComponent<Text>().text = "Ghost time:\n" + TimeSpan.FromSeconds(ghostDuration).ToString("mm\\:ss\\:ff");
            if (ghostDuration > 0.0f)
            {
                ghostDuration -= Time.deltaTime;
                
            }
            else
            {
                generalState = ghostState.Normal;
                ghostTimer.SetActive(false);
            }

            
        }
    }

    //When hit the wall, play the effect
    IEnumerator WallCollisionEffect()
    {
        collideWallEffect.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        collideWallEffect.SetActive(false);
    }

    //When pacstudent die, remove the collider to prevent further raycast detection
    //reset the ghost location
    //add the collider back after 1 second
    IEnumerator PacStudentDeath()
    {
        pacStudent.GetComponent<Animator>().runtimeAnimatorController = pacStudentDeathController;
        pacStuDieEffect.SetActive(true);
        Destroy(brownGhost.GetComponent<BoxCollider2D>());
        Destroy(greenGhost.GetComponent<BoxCollider2D>());
        Destroy(purpleGhost.GetComponent<BoxCollider2D>());
        Destroy(redGhost.GetComponent<BoxCollider2D>());
        //RESET ghost
        GhostController.activeTweenBrown = null;
        GhostController.activeTweenGreen = null;
        GhostController.activeTweenPurple = null;
        GhostController.activeTweenRed = null;
        brownGhost.transform.position = new Vector3(-0.5f, 1.5f, 0.0f);
        greenGhost.transform.position = new Vector3(0.5f, 1.5f, 0.0f);
        purpleGhost.transform.position = new Vector3(-0.5f, -0.5f, 0.0f);
        redGhost.transform.position = new Vector3(0.5f, -0.5f, 0.0f);
        yield return new WaitForSeconds(1.0f);
        pacStuDieEffect.SetActive(false);
        pacStudent.transform.position = new Vector3(-12.5f, 13.5f, 0.0f);
        pacStudent.GetComponent<Animator>().runtimeAnimatorController = pacStudentNormalController;
        brownGhost.AddComponent<BoxCollider2D>();
        greenGhost.AddComponent<BoxCollider2D>();
        purpleGhost.AddComponent<BoxCollider2D>();
        redGhost.AddComponent<BoxCollider2D>();
        particleEffect.SetActive(false);
    }

    IEnumerator BackgroundMusic()
    {
        ghostScaredSound.Stop();
        ghostDeathSound.Stop();
        //yield return new WaitUntil(() => startSound.isPlaying == false);
        yield return new WaitUntil(() => backgroundSound.isPlaying == false);
        //yield return new WaitUntil(() => ghostScaredSound.isPlaying == false);
        //yield return new WaitUntil(() => ghostDeathSound.isPlaying == false);

        backgroundSound.Play();
    }

    IEnumerator ScaredMusic()
    {
        //yield return new WaitUntil(() => backgroundSound.isPlaying == false);
        backgroundSound.Stop();
        ghostDeathSound.Stop();
        yield return new WaitUntil(() => ghostScaredSound.isPlaying == false);
        //yield return new WaitUntil(() => ghostDeathSound.isPlaying == false);
        ghostScaredSound.Play();
    }

    IEnumerator GhostDeathMusic()
    {
        //yield return new WaitUntil(() => backgroundSound.isPlaying == false);
        backgroundSound.Stop();
        ghostScaredSound.Stop();
        //yield return new WaitUntil(() => ghostScaredSound.isPlaying == false);
        yield return new WaitUntil(() => ghostDeathSound.isPlaying == false);
        ghostDeathSound.Play();
    }

    //When the scared ghost is eaten, return to normal state after ghost back to spawn area
    IEnumerator GhostBackToNoraml()
    {
        
        yield return new WaitForSeconds(3.0f);
        
        if(brownState == ghostState.Death)
        {
            brownState = ghostState.Normal;
            brownGhost.GetComponent<Animator>().runtimeAnimatorController = brownNormal;
            brownGhost.AddComponent<BoxCollider2D>();
            generalState = ghostState.Normal;
            GhostController.brownStatus = GhostController.ghostStatus.Normal;
        }

        if (greenState == ghostState.Death)
        {
            greenState = ghostState.Normal;
            greenGhost.GetComponent<Animator>().runtimeAnimatorController = greenNormal;
            greenGhost.AddComponent<BoxCollider2D>();
            generalState = ghostState.Normal;
            GhostController.greenStatus = GhostController.ghostStatus.Normal;
        }

        if (purpleState == ghostState.Death)
        {
            purpleState = ghostState.Normal;
            purpleGhost.GetComponent<Animator>().runtimeAnimatorController = purpleNormal;
            purpleGhost.AddComponent<BoxCollider2D>();
            generalState = ghostState.Normal;
            GhostController.purpleStatus = GhostController.ghostStatus.Normal;
        }

        if (redState == ghostState.Death)
        {
            redState = ghostState.Normal;
            redGhost.GetComponent<Animator>().runtimeAnimatorController = redNormal;
            redGhost.AddComponent<BoxCollider2D>();
            generalState = ghostState.Normal;
            GhostController.redStatus = GhostController.ghostStatus.Normal;
        }
        //yield return null;
    }

    IEnumerator DetectPelletsandWall()
    {
        DetectPelletCollision();
        yield return new WaitForSeconds(0.3f);
        DetectWallCollision();
    }
}
