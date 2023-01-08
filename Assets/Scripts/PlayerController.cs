using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region public variables
    [Header("Jump")]
    [Range(0f, 4f)]
    public float speed;
    public LayerMask allowJumpLayer;
    [Header("Stamina")]
    [Range(0f, 4f)]
    public float staminaRecoverMultip = 2;
    public float staminaLength = 3;
    public GameObject staminaBar;
    /*[Header("Scoring")]
    public GameController gameController;*/
    #endregion

    #region private variables
    Rigidbody2D rb;
    GameController gc;

    float staminaMax = 1;
    float staminaCurrent;
    float staminaBarXScale;
    
    bool arrived = false;
    bool holding = false;

    int direction = -1; //-1 = left, 1 = right
    int score = 0;
    int yMax;
    #endregion

    #region functions
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        gc = GameObject.FindGameObjectWithTag("GameController").gameObject.GetComponent<GameController>();
        staminaCurrent = staminaMax;
        staminaBarXScale = staminaBar.gameObject.transform.localScale.x;
        score = yMax = Mathf.FloorToInt(gameObject.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        holding = Input.GetKey(KeyCode.LeftShift); //|| Input.GetMouseButton(0);
        Move();
        ScoreEvaluation();
    }

    void ScoreEvaluation()
    {
        //scoreText.GetComponent<Text>().text = (score = yMax = (Mathf.FloorToInt(gameObject.transform.position.y) + Mathf.FloorToInt(gameObject.transform.position.y / 11.28f) * 10) > score ? yMax : score).ToString();
        yMax = (Mathf.FloorToInt(gameObject.transform.position.y) + Mathf.FloorToInt(gameObject.transform.position.y / 11.28f) * 10);
        score = yMax > score ? yMax : score;
        PushScore();
    }

    void Move()
    {
        if (Time.timeScale != 0f)
        {
            Quaternion target = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 1000000);

            if (!holding)
            {
                if (staminaCurrent <= staminaMax)
                {
                    staminaCurrent += (Time.deltaTime * (1 / staminaLength)) / staminaRecoverMultip;
                }
            }

            if (gameObject.GetComponent<Collider2D>().IsTouchingLayers(allowJumpLayer))
            {
                //Debug.Log(staminaCurrent);
                //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                if (holding && staminaCurrent > 0)
                {
                    rb.gravityScale = 0;
                    rb.velocity = new Vector3(0, 0, 0);
                    staminaCurrent -= Time.deltaTime * (1 / staminaLength);
                }
                if (holding || staminaCurrent <= 0)
                {
                    rb.gravityScale = 1;
                }
                else if (Input.GetMouseButtonUp(0)) //&& gameObject.GetComponent<Collider2D>().IsTouchingLayers(layerMask))
                {
                    if (rb.velocity.y < 0)
                        rb.AddForce(new Vector2(direction * 0.75f, speed + (-rb.velocity.y / 15)), ForceMode2D.Impulse);
                    else
                        rb.AddForce(new Vector2(direction * 0.75f, speed), ForceMode2D.Impulse);
                    direction *= -1;
                }
                else if (arrived)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    arrived = false;
                }
            }
            else if (!gameObject.GetComponent<Collider2D>().IsTouchingLayers(allowJumpLayer))
            {
                if (!arrived)
                {
                    arrived = true;
                }
            }
            staminaBar.gameObject.transform.localScale = new Vector2(staminaBarXScale * staminaCurrent, staminaBar.gameObject.transform.localScale.y);
        }
        
    }

    void OnColliderEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == allowJumpLayer)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void PushScore()
    {
        gc.score = score;
    }

    public void Die()
    {
        Destroy(gameObject);
        gc.ReloadScene();
    }
    #endregion
}
