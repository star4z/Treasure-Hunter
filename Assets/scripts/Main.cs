using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    int[,] grid;

    GameObject[,] tileGrid;
    public GameObject floor;
    public GameObject block;
    public GameObject wall;
    public GameObject player;

    public GameObject treasureChest;

    public Sprite openChestSprite;

    public GameObject treasure;

    public GameObject pauseScreen, instructionsScreen, winScreen;

    public AudioClip pickaxeSound;

    public AudioClip levelCompleteSound;

    public AudioClip openChestSound;

    public AudioClip buttonSound;
    public AudioClip[] stepSounds;

    private AudioSource audioSource;

    Animator animator;

    double scale = 2.0;

    P playerPos;

    GameObject player0;

    GameObject chest0, treasure0, floor0, wall0, wall1, wall2, wall3;

    Vector2 chestPos;

    public float speed = 25.0f;

    int w, h;
    P facingDir = P.up;
    bool isMoving = false;
    Vector2 targetPosition;

    public static int moveCount;

    public static bool chestOpened;
    bool isPaused = false;
    static int levelsCompleted = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        player0 = player;
        animator = player0.GetComponent<Animator>();
        pauseScreen.SetActive(false);
        NewGame();
        Pause();
    }

    public void NewGame()
    {
        chestOpened = false;
        if (tileGrid != null)
        {
            for (int i = 0; i < tileGrid.GetLength(0); i++)
            {
                for (int j = 0; j < tileGrid.GetLength(1); j++)
                {
                    Destroy(tileGrid[i, j]);
                }
            }
        }

        Destroy(chest0);
        Destroy(treasure0);
        Destroy(floor0);
        Destroy(wall0);
        Destroy(wall1);
        Destroy(wall2);
        Destroy(wall3);

        w = Random.Range(6, 9);
        h = Random.Range(6, 9);
        Debug.Log(w + "x" + h);

        int chestW = Random.Range(0, w);
        int chestH = Random.Range(0, h);
        if (chestW == 0 && chestH == 0)
        {
            chestW = w - 1;
            chestH = h - 1;
        }
        Debug.Log("chest: (" + chestW + ", " + chestH + ")");

        grid = new int[w, h];
        tileGrid = new GameObject[w, h];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (i == 0 && j == 0)
                {
                    grid[i, j] = 1;
                    playerPos = P.zero;
                    player0.transform.position = Vector2.zero;
                    facingDir = P.up;
                }
                else if (i == chestW && j == chestH)
                {
                    grid[i, j] = -2;
                    tileGrid[i, j] = Instantiate(block, new Vector2((int)(scale * i), (int)(scale * j)), Quaternion.identity);
                }
                else
                {
                    grid[i, j] = -1;
                    tileGrid[i, j] = Instantiate(block, new Vector2((int)(scale * i), (int)(scale * j)), Quaternion.identity);
                }
            }
        }

        moveCount = 0;

        int wMidScaled = (int)(((w / 2.0) - 0.5) * scale);
        int hMidScaled = (int)(((h / 2.0) - 0.5) * scale);
        int wScaled = (int)(w * scale);
        int hScaled = (int)(h * scale);

        int unitScaled = (int)(1 * scale);

        floor0 = Instantiate(floor, new Vector2(wMidScaled, hMidScaled), Quaternion.identity);
        floor0.GetComponent<SpriteRenderer>().size = new Vector2(wScaled, hScaled);

        wall0 = Instantiate(wall, new Vector2(wMidScaled, -unitScaled), Quaternion.identity);
        Vector2 wall0Size = new Vector2(wScaled, unitScaled);
        wall0.GetComponent<SpriteRenderer>().size = wall0Size;
        wall0.GetComponent<BoxCollider2D>().size = wall0Size;

        wall1 = Instantiate(wall, new Vector2(wMidScaled, hScaled), Quaternion.identity);
        Vector2 wall1Size = new Vector2(wScaled, unitScaled);
        wall1.GetComponent<SpriteRenderer>().size = wall1Size;
        wall1.GetComponent<BoxCollider2D>().size = wall1Size;

        wall2 = Instantiate(wall, new Vector2(-unitScaled, hMidScaled), Quaternion.identity);
        Vector2 wall2Size = new Vector2(unitScaled, hScaled);
        wall2.GetComponent<SpriteRenderer>().size = wall2Size;
        wall2.GetComponent<BoxCollider2D>().size = wall2Size;


        wall3 = Instantiate(wall, new Vector2(wScaled, hMidScaled), Quaternion.identity);
        Vector2 wall3Size = new Vector2(unitScaled, hScaled);
        wall3.GetComponent<SpriteRenderer>().size = wall3Size;
        wall3.GetComponent<BoxCollider2D>().size = wall3Size;

        Resume();
    }

    void Update()
    {
        animator.ResetTrigger("pickaxe");
        if (!isPaused)
        {
            if (isMoving)
            {
                float step = speed * Time.deltaTime;
                player0.transform.position = Vector2.MoveTowards(player0.transform.position, targetPosition, step);

                if (Vector2.Distance(player0.transform.position, targetPosition) < 0.001f)
                {
                    player0.transform.position = targetPosition;
                    isMoving = false;
                    if (animator.runtimeAnimatorController != null)
                    {
                        animator.SetBool("isMoving", isMoving);
                    }
                }

            }
            else
            {
                if (Input.GetButtonDown("Jump"))
                {
                    animator.SetTrigger("pickaxe");
                    audioSource.PlayOneShot(pickaxeSound);

                    dig();
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    tryToMove(P.up);
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    tryToMove(P.down);
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    tryToMove(P.left);
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    tryToMove(P.right);
                }
                else if (Input.GetButtonDown("Cancel"))
                {
                    PauseGame();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
            {
                playButtonSound();
                if (pauseScreen != null && pauseScreen.activeSelf)
                {
                    pauseScreen.SetActive(false);
                    Resume();
                }
                else if (instructionsScreen != null & instructionsScreen.activeSelf)
                {
                    instructionsScreen.SetActive(false);
                    Resume();
                }
                else if (winScreen != null && winScreen.activeSelf)
                {
                    winScreen.SetActive(false);
                    NewGame();
                }
            }
        }
    }

    void tryToMove(P dir)
    {
        this.facingDir = dir;

        UpdateDir();


        P newPos = playerPos + dir;

        if (spaceAvailable(newPos))
        {
            isMoving = true;
            if (animator.runtimeAnimatorController != null)
            {
                animator.SetBool("isMoving", isMoving);
            }
            grid[playerPos.x, playerPos.y] = 0;
            grid[newPos.x, newPos.y] = 1;
            targetPosition = new Vector3(newPos.x * (float)scale, newPos.y * (float)scale);
            playerPos = newPos;
            moveCount++;

            int r = Random.Range(0, stepSounds.Length);
            audioSource.PlayOneShot(stepSounds[r]);
        }
    }

    bool spaceAvailable(P pos)
    {
        if (withinBounds(pos))
        {
            return grid[pos.x, pos.y] == 0;
        }
        else
        {
            return false;
        }
    }

    bool withinBounds(P pos)
    {
        return pos.x >= 0 && pos.x < w && pos.y >= 0 && pos.y < h;
    }


    void dig()
    {
        P newPos = playerPos + facingDir;

        if (canDig(newPos))
        {
            bool containsChest = grid[newPos.x, newPos.y] == -2;
            if (containsChest)
            {
                chestPos = new Vector2((int)(scale * newPos.x), (int)(scale * newPos.y));
                grid[newPos.x, newPos.y] = 2;
            }
            else
            {
                grid[newPos.x, newPos.y] = 0;
            }
            Pause();
            StartCoroutine(DelayedDig(newPos, containsChest));
        }
        else
        {
            openChest(newPos);
        }
    }

    IEnumerator DelayedDig(P newPos, bool containsChest)
    {
        yield return new WaitForSeconds(0.5f);
        tileGrid[newPos.x, newPos.y].SetActive(false);
        if (containsChest)
        {
            chest0 = Instantiate(treasureChest, chestPos, Quaternion.identity);
        }
        Resume();
    }

    bool canDig(P pos)
    {
        if (withinBounds(pos))
        {
            return grid[pos.x, pos.y] < 0;
        }
        else
        {
            return false;
        }
    }

    void openChest(P p)
    {
        if (withinBounds(p) && grid[p.x, p.y] == 2)
        {
            audioSource.PlayOneShot(openChestSound);
            chest0.GetComponent<SpriteRenderer>().sprite = openChestSprite;
            treasure0 = Instantiate(treasure, chestPos + new Vector2(0, (float)(0.25 * scale)), Quaternion.identity);
            Win();
        }
    }

    IEnumerator callWinDelayed()
    {
        yield return new WaitForSeconds(1f);
        Win();
    }

    void UpdateDir()
    {
        int f = 0;
        P temp = P.up;

        //Smart-ass way to calculate dir using simplified rotation matrix
        while (!(temp.Equals(facingDir)))
        {
            temp = new P(temp.y, -temp.x);
            f++;
        }

        animator.SetInteger("dir", f);
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void PauseGame()
    {
        Pause();
        pauseScreen.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Win()
    {
        Pause();
        levelsCompleted++;
        audioSource.PlayOneShot(levelCompleteSound);
        chestOpened = true;
    }

    public void playButtonSound()
    {
        // audioSource.PlayOneShot(buttonSound);
    }
}
