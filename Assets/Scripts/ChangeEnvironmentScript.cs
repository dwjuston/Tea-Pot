using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnvironmentScript : MonoBehaviour
{

    private int currentEnvironmentIndex = 0;

    //private GameObject currentEnvironment;
    public GameObject environmentHolder;
    public List<GameObject> candidateEnvironments;

    public float transitionDuration = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeEnvironment()
    {
        int newIndex = (currentEnvironmentIndex + 1) % candidateEnvironments.Count;
        currentEnvironmentIndex = newIndex;
        TransitionToEnvironment(newIndex);
    }

    private void TransitionToEnvironment(int newIndex)
    {
        // Fade out
        //yield return StartCoroutine(FadeOut());

        // Destroy current environment and instantiate new one

        foreach (Transform child in environmentHolder.transform)
        {
            Destroy(child.gameObject);
        }


        GameObject newEnvironmentPrefab = candidateEnvironments[newIndex];
        // instantiate new environment and put under environment holder
        Instantiate(newEnvironmentPrefab, environmentHolder.transform, true);

        // Fade in
        //yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        /*
        float elapsedTime = 0f;
        Color color = transitionImage.color;
        while (elapsedTime < transitionDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / transitionDuration);
            transitionImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 1;
        transitionImage.color = color;
        */
        yield return null;
    }

    private IEnumerator FadeIn()
    {
        /*
        float elapsedTime = 0f;
        Color color = transitionImage.color;
        while (elapsedTime < transitionDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / transitionDuration);
            transitionImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 0;
        transitionImage.color = color;
        */
        yield return null;
    }
}
