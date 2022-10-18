using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int gameState;
    public List<GameObject> pins;
    public Vector3 startPos, freezePos;
    public float positionSpd, throwSpd;
    public Rigidbody rb;
    public GameObject ball;
    public GameObject throwDir;
    public Animator throwAnim;
    public Camera startCam;
    public Camera ballCam;
    public Camera PinCam;
    public int level;
    public int[] scores = new int[3];
    public TextMeshProUGUI[] scoreTxt;
    public TextMeshProUGUI gameOverTxt;

    private void Start()
    {
        throwAnim = throwDir.GetComponent<Animator>();
        rb = ball.GetComponent<Rigidbody>();
        startPos = ball.transform.position;
    }
    private void Update()
    {
        if(level != 2)
        {
            switch (gameState)
            {
                case 0: //positioning fucntion
                    Positioning();
                    break;
                case 1: //rotating function
                    Rotation();
                    break;
            }
        }
        else
        {
            var s = scores[0] + scores[1];
            scoreTxt[2].text = s.ToString();
        }
        
    }

    private void Positioning()
    {
        
        Debug.Log("Positioning");
        if(Input.GetKey(KeyCode.A))
        {
            ball.transform.position = new Vector3(Mathf.Clamp(ball.transform.position.x + positionSpd, 10.8f, 11.8f), startPos.y, startPos.z);
        }
        else if( Input.GetKey(KeyCode.D))
        {
            ball.transform.position = new Vector3(Mathf.Clamp(ball.transform.position.x + -positionSpd, 10.8f, 11.8f), startPos.y, startPos.z);
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            freezePos = ball.transform.position;
            gameState += 1;
        }
    }
    private void Rotation()
    {
        Debug.Log("Rotating");
        throwAnim.SetBool("Aiming", true);
        ball.transform.position = freezePos;
        ball.transform.rotation = Quaternion.identity;
        if(Input.GetKeyDown(KeyCode.Return))
        {
            gameState += 1;
            throwAnim.enabled = false;
            StartCoroutine(Throw(throwDir.transform.rotation));

        }
    }
    IEnumerator Throw(Quaternion r)
    {
        Debug.Log("Throw");
        ball.transform.rotation = r;
        yield return new WaitForSeconds(.3f);
        rb.isKinematic = false;
        throwDir.SetActive(false);
        ballCam.gameObject.SetActive(true);
        startCam.gameObject.SetActive(false);
        rb.AddForce(ball.transform.forward * -throwSpd);
        yield return null;
    }

    public IEnumerator PinCheck()
    {
        Debug.Log("Pin Check");
        ballCam.gameObject.SetActive(false);
        PinCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        foreach (var p in pins)
        {
            if(p.transform.rotation!= Quaternion.identity && p.gameObject.activeSelf)
            {
                scores[level] += 1;
                p.gameObject.SetActive(false);
            }
        }
        
        ScoreTextFunction(level);
        ResetFunction();
        yield return null;
    }

    public void ScoreTextFunction(int i)
    {
        scoreTxt[i].text = scores[i].ToString();
    }
    public void ResetFunction()
    {
        rb.isKinematic = true;
        PinCam.gameObject.SetActive(false);
        startCam.gameObject.SetActive(true);
        ball.transform.position = startPos;
        ball.transform.rotation = Quaternion.identity;
        throwDir.SetActive(true);
        throwAnim.enabled = true;
        throwAnim.SetBool("Aiming", false);
        throwDir.transform.rotation = Quaternion.identity;
        if(scores[0] == 10)
        {
            level = 2;
            GameOverText("Strike!");
        }
        else if(scores[0] + scores[1] == 10)
        {
            level = 2;
            GameOverText("Spare!");
        }
        else
        {
            level += 1;
        }
        gameState = 0;
    }

    private void GameOverText(string s)
    {
        gameOverTxt.text = s;
    }
}
