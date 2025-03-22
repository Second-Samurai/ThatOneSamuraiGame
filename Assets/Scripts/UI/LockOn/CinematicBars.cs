using UnityEngine;
using UnityEngine.UI;

public class CinematicBars : MonoBehaviour
{
    RectTransform _topBar, _bottomBar;

    float changeSizeAmount, targetSize;
    bool _isActive = false;

    private void Awake()
    {
        InitializeBars(); 
    }

    private void InitializeBars()
    {
        GameObject gameObject = new GameObject("topBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        _topBar = gameObject.GetComponent<RectTransform>();
        _topBar.anchorMin = new Vector2(0, 1);
        _topBar.anchorMax = new Vector2(1, 1);
        _topBar.sizeDelta = new Vector2(0, 0);

        gameObject = new GameObject("bottomBar", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().color = Color.black;
        _bottomBar = gameObject.GetComponent<RectTransform>();
        _bottomBar.anchorMin = new Vector2(0, 0);
        _bottomBar.anchorMax = new Vector2(1, 0);
        _bottomBar.sizeDelta = new Vector2(0, 0);
    }

    public void ShowBars(float targetSize, float time)
    {
        this.targetSize = targetSize;
        changeSizeAmount = (targetSize - _topBar.sizeDelta.y) / time;
        _isActive = true;
    }

    public void HideBars(float time)
    {
        targetSize = 0;
        changeSizeAmount = (targetSize - _topBar.sizeDelta.y) / time;
        _isActive = true;
    }

    private void Update()
    {
        AnimateBars();
    }

    private void AnimateBars()
    {
        if (_isActive)
        {
            Vector2 sizeDelta = _topBar.sizeDelta;
            sizeDelta.y += changeSizeAmount * Time.deltaTime;
            _topBar.sizeDelta = sizeDelta;
            _bottomBar.sizeDelta = sizeDelta;
            if (changeSizeAmount > 0)
            {
                if (sizeDelta.y >= targetSize)
                {
                    sizeDelta.y = targetSize;
                    _isActive = false;
                }
            }
            else
            {
                if (sizeDelta.y <= targetSize)
                {
                    sizeDelta.y = targetSize;
                    _isActive = false;
                }
            }
        }
    }
}
