using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Scene_Login : MonoBehaviour {
    
    [Header("Image")]
    public Image mainBG;

    [Header("Buttons")]
    public Button btMain;

    [Header("Setting")]
    public float imgMovingDuration = 2.0f;
    public float btFadeInDuration = 1.0f;

    private void Awake()
    {
        btMain.gameObject.SetActive(false);
        btMain.GetComponent<Image>().DOFade(0.0f, 0.0f);

        mainBG.rectTransform.DOLocalMoveX(-3160.0f, imgMovingDuration).OnComplete(() =>
        {
            btMain.gameObject.SetActive(true);
            btMain.GetComponent<Image>().DOFade(1.0f, btFadeInDuration);
        });
    }

    public void LoadMenuScene()
    {
        MyGameManager.Get().LoadMenuScene();
    }
}