using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehavior : MonoBehaviour
{

	Text moveCount;
	GameObject winCanvas;

    void Start()
    {
		moveCount = transform.Find("MoveCountText").GetComponent<Text>();
		UpdateMoveCount(Main.moveCount);

		winCanvas = transform.Find("WinCanvas").gameObject;
		winCanvas.SetActive(false);
    }

    void Update()
    {
			UpdateMoveCount(Main.moveCount);
			if (Main.chestOpened) {
				ShowWin();
			}
    }

    public void UpdateMoveCount(int val)
    {
		moveCount.text = val + " moves";
    }

	public void ShowWin()
	{
		moveCount.enabled = false;
		winCanvas.SetActive(true);
	}
}
