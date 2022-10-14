using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    // 함수와 변수를 static으로 선언해두면 LoadingScene으로 넘어오지 않아서
    // LoadSceneController가 부착된 게임오브젝트가 생성되지 않아도
    // LoadingSceneController 클래스의 이름으로 호출할 수 있다.
    static string _nextScene;

    [SerializeField]
    Image _loadingBar;

    public static void LoadScene(string sceneName)
    {
        _nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());    
    }

    // 안정적인 로딩씬 호출을 위해 LoadSceneAsync 메서드 사용
    // LoadSceneAsync 메서드가 AsyncOperation 타입을 이용하여 씬을 불러온다.
    IEnumerator LoadSceneProcess()
    {
         AsyncOperation loading = SceneManager.LoadSceneAsync(_nextScene);
        loading.allowSceneActivation = false;

        // 로딩이 끝날 때까지 제어권을 유니티 엔진에 넘긴다.
        float timer = 0.0f;
        while (!loading.isDone)
        {
            yield return null;

            // 90% 로딩이 진행될 때까지는 로딩 진행도를 보여주고
            // 90%보다 커지면 임시로딩 수행
            if (loading.progress < 0.9f)
            {
                _loadingBar.fillAmount = loading.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _loadingBar.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);
                if (_loadingBar.fillAmount >= 1.0f)
                {
                    // 로딩 바가 다 채워지고 나면 씬이 바로 전환됨
                    loading.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
    
}
