using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingUI: MonoBehaviour
{
    private GameObject goScreenLoading;
    private GameObject goIconLoading;

    private Slider sliderLoading;
    private Text textLoading;
    private Image imgIconLoading;

    private LoadAnimation m_eLoadAnimation;

    void Start()
    {
        goScreenLoading = GameObject.Find("plnScreenLoading");
        goIconLoading = GameObject.Find("imgIconLoading");

        GameObject goSliderLoading = GameObject.Find("sliderLoading");
        if (goSliderLoading != null)
        {
            sliderLoading = goSliderLoading.GetComponent<Slider>();
        }
        GameObject goTextLoading = GameObject.Find("txtProgress");
        if (goTextLoading != null)
        {
            textLoading = goTextLoading.GetComponent<Text>();
        }
        if (goIconLoading != null)
        {
            imgIconLoading = goIconLoading.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (m_eLoadAnimation == LoadAnimation.LoadAnimation_WholeScreen)
        {
            textLoading.text = "正在加载资源";
            sliderLoading.value += 0.005f;
        }
        else if (m_eLoadAnimation == LoadAnimation.LoadAnimation_Icon)
        {
            //imgIconLoading.transform.ro
        }
    }

    public void SetLoadAnimation(LoadAnimation eLoadAnimation)
    {
        m_eLoadAnimation = eLoadAnimation;
        if (m_eLoadAnimation == LoadAnimation.LoadAnimation_WholeScreen)
        {
            goScreenLoading.SetActive(true);
            goIconLoading.SetActive(false);

            textLoading.text = "";
            sliderLoading.value = 0.0f;
        }
        else if (m_eLoadAnimation == LoadAnimation.LoadAnimation_Icon)
        {
            goScreenLoading.SetActive(false);
            goIconLoading.SetActive(true);

            //imgIconLoading.transform.ro
        }
    }

    public void CloseLoading()
    {
        goScreenLoading.SetActive(false);
        goIconLoading.SetActive(false);
    }
}
