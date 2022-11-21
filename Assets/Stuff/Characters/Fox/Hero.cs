using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    GameHandler gameHandler;
    Rigidbody rb;
    Animator animator;
    [SerializeField] float acceleration = 25f;
    [SerializeField] float maxMoveSpeed = 3f;
    public float swarmCounter = 0f;
    bool isDead = false;
    [SerializeField] Slider swarmSlider;
    [SerializeField] Image sliderFillimage;

    void Start()
    {
        gameHandler = FindObjectOfType<GameHandler>();
        rb = GetComponentInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (isDead)
        {
            animator.SetBool("dead", true);
            rb.velocity = Vector3.zero;
            StartCoroutine(gameHandler.GameOver());
            return;
        }

        if (swarmCounter > 3)
        {
            isDead = true;
            swarmSlider.value = 3;
        }
        else if (swarmCounter < 0 || swarmCounter == 0)
        {
            swarmCounter = 0;
            sliderFillimage.color = Color.white;
        }
        else
        {
            swarmSlider.value = swarmCounter;
            sliderFillimage.color = Color.red;
        }
        

        if (gameHandler.levelPlaying && !gameHandler.beatLevel)
        {
            Move();
        }
    }

    void Move()
    {
        //Update movement so he follows a track
        var currentMoveSpeed = maxMoveSpeed;
        if (swarmCounter > 0 && swarmCounter < 3)
        {
            currentMoveSpeed /= swarmCounter;
        }

        currentMoveSpeed = (currentMoveSpeed > maxMoveSpeed) ? maxMoveSpeed : currentMoveSpeed;

        if(rb.velocity.magnitude < currentMoveSpeed && currentMoveSpeed > 0)
        {
            //rb.AddForce(new Vector3(-1 * acceleration, 0, 0));
            rb.velocity = new Vector3(-currentMoveSpeed, 0, 0);
        }
        animator.SetFloat("speed", rb.velocity.magnitude);
    }

}
