using UnityEngine;
using UnityEngine.SceneManagement;

public interface IScenePlatformLoader
{
    void Initialise(Transform cameraTransform);
}

public class ScenePlatformLoader : MonoBehaviour, IScenePlatformLoader
{
    public enum CheckMethod
    {
        Distance,
        Trigger
    }

    public CheckMethod checkMethod;
    public Transform cameraTransform;
    public float loadRange;

    [Header("Tile Chain")]
    [Tooltip("Only required when backtracking is disabled")]
    public ScenePlatformLoader backSceneTile;
    [Tooltip("Only required when backtracking is disabled")]
    public ScenePlatformLoader forwardSceneTile;

    [Header("Loading Switches")]
    [Tooltip("Only enable when this scene tile is able to be backtracked on travel")]
    public bool canBacktrack = true;

    //Scene state
    private bool _isLoaded;
    private bool _shouldLoad;
    private bool _loadOnBacktrack = true;

    /// <summary>
    /// Sets reference camera transform and checks whether exists in build settings.
    /// </summary>
    public void Initialise(Transform cameraTransform)
    {
        this.cameraTransform = cameraTransform;

        if (SceneManager.sceneCount > 0)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name)
                {
                    _isLoaded = true;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (checkMethod == CheckMethod.Distance)
        {
            DistanceCheck();
        }
        else
        {
            TriggerCheck();
        }
    }

    public bool CheckIsLoaded()
    {
        return _isLoaded;
    }

    // Summary: Performs distance check and either loads or unloads scene
    //
    private void DistanceCheck()
    {
        if (Vector3.Magnitude(cameraTransform.position - transform.position) < loadRange)
        {
            LoadScene();
        }
        else
        {
            UnloadScene();
        }
    }

    // Summary: Loads scene additively
    //
    private void LoadScene()
    {
        if (!_isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            _isLoaded = true;
        }
    }

    // Summary: Unloads the specified scene
    //
    private void UnloadScene()
    {
        if (_isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            _isLoaded = false;
        }
    }

    private void TriggerCheck()
    {
        if (_shouldLoad)
        {
            LoadScene();
        }
        else
        {
            UnloadScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (checkMethod != CheckMethod.Trigger) return;
        if (other.GetComponent<PlayerController>() != null)
        {
            //if(_loadOnBacktrack)
            _shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (checkMethod != CheckMethod.Trigger) return;
        if (other.GetComponent<PlayerController>() != null)
        {
            _shouldLoad = false;
            if (!canBacktrack) CheckCanBacktrack();
        }
    }

    private void CheckCanBacktrack()
    {
        if (forwardSceneTile.CheckIsLoaded())
        {
            _loadOnBacktrack = false;
            return;
        }
        else if (backSceneTile.CheckIsLoaded())
        {
            _loadOnBacktrack = true;
            return;
        }
    }

    private void OnDrawGizmos()
    {
        if (checkMethod != CheckMethod.Distance) return;
        Gizmos.DrawWireCube(transform.position, new Vector3(loadRange*2, loadRange*2, loadRange * 2));
    }
}
