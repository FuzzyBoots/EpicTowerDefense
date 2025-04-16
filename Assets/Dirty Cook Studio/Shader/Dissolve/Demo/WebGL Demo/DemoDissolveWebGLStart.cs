using System.Collections;
using DirtyCookStudio.Shader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DirtyCookStudio.Demo
{
    public class DemoDissolveWebGLStart : MonoBehaviour
    {
        [SerializeField] private Image _unityLogo, _dCSLogo;
        [SerializeField] private TMP_Text _welcomeText;
        [SerializeField] private Button _startButton;
        [SerializeField] private float _textDuration, _logo1Duration, _logo2Duration;

        private DissolveShader _dShader;

        void OnEnable()
        {
            _dShader = new();
            _startButton.onClick.AddListener(OnStartButton);

            StartCoroutine(SplashScreen());
        }

        private void OnStartButton()
        {
            StartCoroutine(StartDemo());
        }

        void OnDisable()
        {
            _startButton.onClick.RemoveAllListeners();
        }

        IEnumerator SplashScreen()
        {
            yield return new WaitForSeconds(0.3f);
            _startButton.gameObject.SetActive(false);
            Color color = _welcomeText.color;
            color.a = 0;
            _welcomeText.color = color;

            Material dcsMat = _dCSLogo.material;
            float elapsedTime = 0;
            while (elapsedTime < _logo2Duration)
            {
                float t = elapsedTime / _logo2Duration;
                float value = Mathf.Lerp(0.0f, 1.0f, t);

                ShaderHelper.SetFloat(dcsMat, _dShader.DissolveAmountProp, value);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            GameObject.Destroy(_dCSLogo.gameObject, 0.1f);
            
            elapsedTime = 0f;
            while (elapsedTime < _textDuration)
            {
                float t = elapsedTime / _textDuration;
                float value = Mathf.Lerp(0, 1.0f, t);
                color = _welcomeText.color;
                color.a = value;
                _welcomeText.color = color;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _startButton.gameObject.SetActive(true);
        }

        IEnumerator StartDemo()
        {//Phase 1
            GameObject.Destroy(_startButton.gameObject);
            float elapsedTime = 0f;
            while (elapsedTime < _textDuration)
            {
                float t = elapsedTime / _textDuration;
                float value = Mathf.Lerp(1.0f, 0.0f, t);
                Color color = _welcomeText.color;
                color.a = value;
                _welcomeText.color = color;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            GameObject.Destroy(_welcomeText.gameObject);

            Material unityMat = _unityLogo.material;
            elapsedTime = 0;
            while (elapsedTime < _logo1Duration)
            {
                float t = elapsedTime / _logo1Duration;
                float value = Mathf.Lerp(0.0f, 1.0f, t);

                ShaderHelper.SetFloat(unityMat, _dShader.DissolveAmountProp, value);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            GameObject.Destroy(_unityLogo.gameObject);

            Material dcsMat = _dCSLogo.material;
            elapsedTime = 0;
            while (elapsedTime < _logo2Duration)
            {
                float t = elapsedTime / _logo2Duration;
                float value = Mathf.Lerp(0.0f, 1.0f, t);

                ShaderHelper.SetFloat(dcsMat, _dShader.DissolveAmountProp, value);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            GameObject.Destroy(this,0.1f);

            yield return null;
        }
    }
}
