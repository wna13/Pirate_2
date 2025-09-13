using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvAuto : MonoBehaviour
{
    private void Start() 
    {
        StartCoroutine(BirdStart());   
        StartCoroutine(FishStart());   
    }

    [SerializeField] Animator bird, fish;
    IEnumerator BirdStart()
    {
        while ( this.gameObject.activeSelf)
        {
            float delay = Random.Range ( 7f, 25f);
            yield return new WaitForSeconds (delay);

            int rdm = Random.Range(0, 3);
            string clip = "Move"+rdm;

            bird.Play(clip);
            yield return new WaitForSeconds (6f);

            yield return null;
            if ( this.gameObject.activeSelf == false ) yield break;
        }
    }
    IEnumerator FishStart()
    {
        while ( this.gameObject.activeSelf)
        {
            float delay = Random.Range ( 5f, 20f);
            yield return new WaitForSeconds (delay);

            int rdm = Random.Range(0, 3);
            string clip = "Move"+rdm;

            fish.Play(clip);
            yield return new WaitForSeconds (6f);

            yield return null;
            if ( this.gameObject.activeSelf == false ) yield break;
        }
    }
}
