using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class IntroManager : MonoBehaviour
{

    [SerializeField] Image mainScene;
    [SerializeField] Image studioLogo;
    [SerializeField] Text quoteFr;
    [SerializeField] Text quoteKr;
    bool skipQuote = false;
    bool start = false;
    [SerializeField] GameObject openingCanvas;
    [SerializeField] GameObject homeCanvas;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] Image homeImage;

    void Start()
    {
        openingCanvas.SetActive(true);
        mainScene.color = new Color(1, 1, 1, 0);
        studioLogo.color = new Color(1, 1, 1, 0);
        quoteFr.color = new Color(1, 1, 1, 0);
        quoteKr.color = new Color(1, 1, 1, 0);
        homeCanvas.SetActive(false);
        mainCanvas.SetActive(false);
        StartCoroutine("Opening");
    }

    private void Update()
    {
        if(skipQuote && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            StartCoroutine("MainScene");
            //quoteFr.color = new Color(1, 1, 1, 0);
            //quoteKr.color = new Color(1, 1, 1, 0);
        }
        if(start && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            StopCoroutine("MainScene");
            StartCoroutine("Home");
        }
    }

    public IEnumerator Opening()
    {
        // Studio Name
        while (studioLogo.color.a < 1.0f)
        {
            studioLogo.color = new Color(studioLogo.color.r, studioLogo.color.g, studioLogo.color.b, studioLogo.color.a + (Time.deltaTime / 1f));
            yield return null;
        }
        studioLogo.color = new Color(studioLogo.color.r, studioLogo.color.g, studioLogo.color.b, 1);
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        while (studioLogo.color.a > 0.0f)
        {
            studioLogo.color = new Color(studioLogo.color.r, studioLogo.color.g, studioLogo.color.b, studioLogo.color.a - (Time.deltaTime / 1f));
            yield return null;
        }
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        skipQuote = true;
        // Quote FR
        while (quoteFr.color.a < 1.0f)
        {
            quoteFr.color = new Color(quoteFr.color.r, quoteFr.color.g, quoteFr.color.b, quoteFr.color.a + (Time.deltaTime / 2f));
            yield return null;
        }
        quoteFr.color = new Color(quoteFr.color.r, quoteFr.color.g, quoteFr.color.b, 1);
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        while (quoteFr.color.a > 0.0f)
        {
            quoteFr.color = new Color(quoteFr.color.r, quoteFr.color.g, quoteFr.color.b, quoteFr.color.a - (Time.deltaTime / 1f));
            yield return null;
        }


        // Quote KR
        while (quoteKr.color.a < 1.0f)
        {
            quoteKr.color = new Color(quoteKr.color.r, quoteKr.color.g, quoteKr.color.b, quoteKr.color.a + (Time.deltaTime / 1f));
            yield return null;
        }
        quoteKr.color = new Color(quoteKr.color.r, quoteKr.color.g, quoteKr.color.b, 1);
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        while (quoteKr.color.a > 0.0f)
        {
            quoteKr.color = new Color(quoteKr.color.r, quoteKr.color.g, quoteKr.color.b, quoteKr.color.a - (Time.deltaTime / 2f));
            yield return null;
        }

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine("MainScene");
    }

    public IEnumerator MainScene()
    {
        StopCoroutine("Opening");
        openingCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        // MainScene
        while (mainScene.color.a < 1.0f)
        {
            mainScene.color = new Color(mainScene.color.r, mainScene.color.g, mainScene.color.b, mainScene.color.a + (Time.deltaTime / 3f));
            yield return null;
        }
        mainScene.color = new Color(mainScene.color.r, mainScene.color.g, mainScene.color.b, 1);
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        start = true;
    }

    public IEnumerator Home()
    {
        //show home
        while (mainScene.color.a > 0.0f)
        {
            mainScene.color = new Color(mainScene.color.r, mainScene.color.g, mainScene.color.b, mainScene.color.a - (Time.deltaTime / 3f));
            yield return null;
        }
        mainCanvas.SetActive(false);
        homeCanvas.SetActive(true);
    }
}