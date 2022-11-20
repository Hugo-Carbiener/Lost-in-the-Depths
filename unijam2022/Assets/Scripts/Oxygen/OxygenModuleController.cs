using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 *  Component of the Player GameObject that will handle its oxygen consumption
 */
public class OxygenModuleController : MonoBehaviour
{
    public float maxOxygen;
    [HideInInspector] public float curOxygen;
    public float consumptionRate;
    public bool isRecharging;//boolean put at true if player connected to oxygen system (hence recharging)
    [SerializeField] private AudioSource deathAudio;
    private AudioSource audioLaser;
    [SerializeField] private GameObject mesh;
    [SerializeField] private GameObject laser;
    private bool isDying;
    private Animator animator;

    private void Start()
    {
        curOxygen = maxOxygen;
        isRecharging = false;
        StartCoroutine(ConsumeOxygen());
        isDying = false;
        animator = mesh.GetComponent<Animator>();
        audioLaser = GetComponent<AudioSource>();
    }

    /**
     *  Loop that consumes oxygen
     */
    private IEnumerator ConsumeOxygen()
    {
        yield return new WaitForSeconds(1);
        curOxygen-=consumptionRate;
        if (curOxygen <= 0 && !isDying)
        {
            curOxygen = 0;
            isDying = true;
            StartCoroutine(OxygenDeath());
        }
        StartCoroutine(ConsumeOxygen());
    }

    /**
     *  Loop that adds oxygen if player connected to oxygen system
     *  To trigger recharge : start coroutine AND put boolean isRecharging=true if connected
     */
    public IEnumerator AddOxygen(float rate)
    {
        yield return new WaitForSeconds(1);
        curOxygen += rate; //the recharge goes faster than the downsize as to increase by 1 increment per tick of the coroutines
        if (curOxygen >= maxOxygen)
        {
            curOxygen = maxOxygen;
        }
        if (isRecharging) //if the player is still connected to oxygen system, then its oxygen levels continue recharging
        {
            StartCoroutine(AddOxygen(rate));
        }
    }

    /**
    *   Death of the player
    */
    private IEnumerator OxygenDeath()
    {
        Debug.Log("DYING");
        GetComponent<PlayerMovement>().enabled = false;
        audioLaser.Stop();
        deathAudio.Play();
        laser.SetActive(false);
        animator.SetBool("IsKneeling", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsShooting", false);
        animator.SetBool("IsDying", true);
        yield return new WaitForSeconds(1);
        GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>().TriggerDeath();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }
}
