using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    // �Լ��� ������ static���� �����صθ� LoadingScene���� �Ѿ���� �ʾƼ�
    // LoadSceneController�� ������ ���ӿ�����Ʈ�� �������� �ʾƵ�
    // LoadingSceneController Ŭ������ �̸����� ȣ���� �� �ִ�.
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

    // �������� �ε��� ȣ���� ���� LoadSceneAsync �޼��� ���
    // LoadSceneAsync �޼��尡 AsyncOperation Ÿ���� �̿��Ͽ� ���� �ҷ��´�.
    IEnumerator LoadSceneProcess()
    {
         AsyncOperation loading = SceneManager.LoadSceneAsync(_nextScene);
        loading.allowSceneActivation = false;

        // �ε��� ���� ������ ������� ����Ƽ ������ �ѱ��.
        float timer = 0.0f;
        while (!loading.isDone)
        {
            yield return null;

            // 90% �ε��� ����� �������� �ε� ���൵�� �����ְ�
            // 90%���� Ŀ���� �ӽ÷ε� ����
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
                    // �ε� �ٰ� �� ä������ ���� ���� �ٷ� ��ȯ��
                    loading.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
    
}
