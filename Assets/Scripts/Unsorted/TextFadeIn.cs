using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextFadeIn : MonoBehaviour
{
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.DOFade(1, 1.5f);
    }
     
}
