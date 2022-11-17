using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    GameHandler gameHandler;
    
    private void Start()
    {
        gameHandler = FindObjectOfType<GameHandler>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        var hero = collision.gameObject.GetComponent<Hero>();
        if (hero != null)
        {
            gameHandler.beatLevel = true;
            hero.GetComponent<Animator>().SetBool("beatLevel", true);
            StartCoroutine(DelayNextLevel());
        }
    }

    IEnumerator DelayNextLevel()
    {
        var sec = Application.isEditor ? 1f : 5f;
        yield return new WaitForSeconds(sec);
        gameHandler.LoadNextLevel();
    }
}
