using System.Collections;
using DirtyCookStudio.Shader;
using UnityEngine.UI;
using UnityEngine;

namespace DirtyCookStudio.Demo
{
    public class DemoDissolve : MonoBehaviour
    {
        public float dissolveTime;
        public float delayTime;

        public DissolveType dissolveType;

        DissolveShader dissolveShader;

        private Material mat;

        void Start()
        {
            dissolveShader = new DissolveShader();
            //We initialize the shader, For simplicity I'm just grabbing
            // either the Image's material or any Renderer's material
            // that covers sprites and meshes
            if (TryGetComponent<Image>(out var image))
                mat = image.material;
            else if (TryGetComponent<Renderer>(out var rend))
                mat = rend.material;

            switch (dissolveType)
            {
                case DissolveType.BASIC:
                    StartCoroutine(DissolveBasic());
                    break;
                case DissolveType.TWOSTEP:
                    StartCoroutine(DissolveTwoStep());
                    break;
            }
        }

        //These two coroutines are very basic lerps to show potential behavior

        IEnumerator DissolveBasic()
        {
            while (true)
            {
                float elapsedTime = 0f;
                while (elapsedTime < dissolveTime)
                {
                    float t = elapsedTime / dissolveTime;
                    float dissolveAmount = Mathf.Lerp(0f, 1f, t);
                    ShaderHelper.SetFloat(mat, dissolveShader.DissolveAmountProp, dissolveAmount);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                ShaderHelper.SetFloat(mat, dissolveShader.DissolveAmountProp, 1.0f);

                //Flip Back
                yield return new WaitForSeconds(delayTime);

                //Phase 2 Back
                elapsedTime = 0f;
                while (elapsedTime < dissolveTime)
                {
                    float t = elapsedTime / dissolveTime;
                    float value = Mathf.Lerp(1f, 0f, t);
                    ShaderHelper.SetFloat(mat, dissolveShader.DissolveAmountProp, value);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                ShaderHelper.SetFloat(mat, dissolveShader.DissolveAmountProp, 0.0f);

                yield return null;
            }
        }

        private IEnumerator DissolveTwoStep()
        {
            while (true)
            {
                float phase1Time = dissolveTime * 0.5f; // First half for Variable1 (0 to 0.55)
                float phase2Time = dissolveTime * 0.5f; // Second half for Variable2 (0 to 1)

                //Phase 1
                float elapsedTime = 0f;
                while (elapsedTime < phase1Time)
                {
                    float t = elapsedTime / phase1Time;
                    float edgeDepth = Mathf.Lerp(0f, 0.55f, t);
                    ShaderHelper.SetFloat(mat, dissolveShader.EdgeDepthProp, edgeDepth);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                ShaderHelper.SetFloat(mat, dissolveShader.EdgeDepthProp, 0.55f);


                //Phase 2
                elapsedTime = 0f;
                while (elapsedTime < phase2Time)
                {
                    float t = elapsedTime / phase2Time;
                    float dissolveAmount = Mathf.Lerp(0f, 1f, t);
                    ShaderHelper.SetFloat(mat, dissolveShader.DissolveAmountProp, dissolveAmount);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                ShaderHelper.SetFloat(mat, dissolveShader.DissolveAmountProp, 1.0f);

                //Flip Back
                yield return new WaitForSeconds(delayTime);

                //Phase 2 Back
                elapsedTime = 0f;
                while (elapsedTime < phase2Time)
                {
                    float t = elapsedTime / phase2Time;
                    float dissolveAmount = Mathf.Lerp(1f, 0f, t);
                    ShaderHelper.SetFloat(mat, dissolveShader.DissolveAmountProp, dissolveAmount);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                ShaderHelper.SetFloat(mat, dissolveShader.DissolveAmountProp, 0.0f);

                //Phase 1 Back
                elapsedTime = 0f;
                while (elapsedTime < phase1Time)
                {
                    float t = elapsedTime / phase1Time;
                    float edgeDepth = Mathf.Lerp(0.55f, 0, t);
                    ShaderHelper.SetFloat(mat, dissolveShader.EdgeDepthProp, edgeDepth);

                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                ShaderHelper.SetFloat(mat, dissolveShader.EdgeDepthProp, 0.0f);

                yield return null;
            }
        }

    }

    public enum DissolveType
    {
        BASIC,
        TWOSTEP
    }

}