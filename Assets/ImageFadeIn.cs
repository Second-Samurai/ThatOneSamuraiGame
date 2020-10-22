using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageFadeIn : MonoBehaviour
{
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.DOFade(1, .3f);
    }

}
