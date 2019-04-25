using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{

    int time;

    public GameObject objectToDrop;

	public Sprite[] gemSprites;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("DropGem", 1.0f, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.anyKey)
		{
			SceneManager.LoadScene("main");
		}
    }

    void DropGem()
    {
        Camera cam = Camera.main;

        float height = 2f * cam.orthographicSize;
        float r = 0.5f * height * cam.aspect;
        float pos = Random.Range( -r,  r);
        float rotation = Random.Range(0f, 360f);
        GameObject obj = Instantiate(objectToDrop, new Vector2(pos, height), new Quaternion(0, 0, rotation, 0));
        // obj.GetComponent<Collider2D>().enabled = true;

		int spriteIndex = Random.Range(0, gemSprites.Length);
		obj.GetComponent<SpriteRenderer>().sprite = gemSprites[spriteIndex];
    }
}
