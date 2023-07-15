using UnityEngine;
using System.Collections;

public class Idle : MonoBehaviour
{
    private float timer;
    public GameObject eyes;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 15f)
        {
            StartCoroutine(Blink());
            
            timer = 0f;
        }
    }

    private IEnumerator Blink() {

        eyes.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        eyes.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        eyes.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        eyes.SetActive(true);



    }
}
