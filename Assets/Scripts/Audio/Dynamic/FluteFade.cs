using UnityEngine;

public class FluteFade : MonoBehaviour
{
    [SerializeField]
    private TrackManager _trackManager;
    [SerializeField]
    private BackgroundAudio _backgroundAudio;

    public bool drumsActive, violinLeadActive, violinHarmonyActive, fluteActive, lowStringActive, shakaActive;

    // Start is called before the first frame update
    void Start()
    {
        _trackManager = AudioManager.instance.trackManager;
        _backgroundAudio = AudioManager.instance.GetComponent<BackgroundAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
                if (drumsActive == true) _trackManager.DrumsFade(true);
                if (violinLeadActive == true) _trackManager.ViolinLeadFade(true);
                if (violinHarmonyActive == true) _trackManager.ViolinHarmonyFade(true);
                if (fluteActive == true) _trackManager.FluteFade(true);
                if (lowStringActive == true) _trackManager.LowStringFade(true);
                if (shakaActive == true) _trackManager.Shakafade(true);           
        }
    }
}
