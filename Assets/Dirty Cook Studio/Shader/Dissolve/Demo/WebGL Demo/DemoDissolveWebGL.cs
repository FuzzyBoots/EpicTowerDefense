using DirtyCookStudio.Shader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DirtyCookStudio.Demo
{
    public class DemoDissolveWebGL : MonoBehaviour
    {
        DissolveShader dShader;
        [SerializeField] private Renderer _targetRenderer;
        [SerializeField] private Material[] _materials;
        [SerializeField] private GameObject[] _noiseGameObjects, _voronoiGameObjects, _dotsGameObjects, _hexGameObjects, _houndstoothGameObjects;
        GameObject[][] _AllGroups;
        [Header("Base Settings")]
        [SerializeField] private TMP_Dropdown _patternType;
        [SerializeField] private Slider _dissolveAmtSlider;
        [SerializeField] private Slider _baseNoiseSlider;
        [Header("Noise")]
        [SerializeField] private Slider _noiseScaleSlider;
        [Header("Voronoi")]
        [SerializeField] private Slider _voronoiAngleOffsetSlider;
        [SerializeField] private Slider _voronoiCellDensitySlider;
        [Header("Dots")]
        [SerializeField] private Slider _dotsSizeSlider;
        [SerializeField] private Slider _dotsOffsetXSlider;
        [SerializeField] private Slider _dotsOffsetYSlider;
        [Header("Hex")]
        [SerializeField] private Slider _hexScaleSlider;
        [SerializeField] private Slider _hexEdgeWidthSlider;
        [Header("Houndstooth")]
        [SerializeField] private Slider _houndstoothTeethSlider;
        [SerializeField] private Slider _houndstoothTilingXSlider;
        [SerializeField] private Slider _houndstoothTilingYSlider;
        [Header("HDR")]
        [SerializeField] private Renderer _hdrImage;
        [SerializeField] private Slider _rColor;
        [SerializeField] private Slider _gColor;
        [SerializeField] private Slider _bColor;
        [SerializeField] private Slider _intensityColor;

        private Material _targetMat;
        private Material _hdrMat;
        private ShaderProperty<Color> _hdrColorProp;

        void Start()
        {
            _hdrColorProp = new(UnityEngine.Shader.PropertyToID("_Color"));
            _targetMat = _targetRenderer.material;
            _hdrMat = _hdrImage.material;
            dShader = new DissolveShader();
            _AllGroups = new GameObject[][]
                { _noiseGameObjects, _dotsGameObjects, _hexGameObjects, _voronoiGameObjects, _houndstoothGameObjects };
            TurnAllOff();
            PatternTypeChange(_patternType.value);
        }

        void OnEnable()
        {
            _patternType.onValueChanged.AddListener(PatternTypeChange);
            _dissolveAmtSlider.onValueChanged.AddListener(DissolveAmtChanged);
            _baseNoiseSlider.onValueChanged.AddListener(BaseNoiseChanged);
            _noiseScaleSlider.onValueChanged.AddListener(NoiseScaleChanged);
            _voronoiAngleOffsetSlider.onValueChanged.AddListener(VoronoiAngleChanged);
            _voronoiCellDensitySlider.onValueChanged.AddListener(VoronoiCellChanged);
            _dotsSizeSlider.onValueChanged.AddListener(DotsSizeChanged);
            _dotsOffsetXSlider.onValueChanged.AddListener(DotsOffsetChanged);
            _dotsOffsetYSlider.onValueChanged.AddListener(DotsOffsetChanged);
            _hexScaleSlider.onValueChanged.AddListener(HexScaleChanged);
            _hexEdgeWidthSlider.onValueChanged.AddListener(HexEdgeChanged);
            _houndstoothTeethSlider.onValueChanged.AddListener(HoundTeethChanged);
            _houndstoothTilingXSlider.onValueChanged.AddListener(HoundTilingChanged);
            _houndstoothTilingYSlider.onValueChanged.AddListener(HoundTilingChanged);
            _rColor.onValueChanged.AddListener(HDRColorChanged);
            _gColor.onValueChanged.AddListener(HDRColorChanged);
            _bColor.onValueChanged.AddListener(HDRColorChanged);
            _intensityColor.onValueChanged.AddListener(HDRColorChanged);
        }

        void OnDisable()
        {
            _patternType.onValueChanged.RemoveAllListeners();
            _dissolveAmtSlider.onValueChanged.RemoveAllListeners();
            _baseNoiseSlider.onValueChanged.RemoveAllListeners();
            _noiseScaleSlider.onValueChanged.RemoveAllListeners();
            _voronoiAngleOffsetSlider.onValueChanged.RemoveAllListeners();
            _voronoiCellDensitySlider.onValueChanged.RemoveAllListeners();
            _dotsSizeSlider.onValueChanged.RemoveAllListeners();
            _dotsOffsetXSlider.onValueChanged.RemoveAllListeners();
            _dotsOffsetYSlider.onValueChanged.RemoveAllListeners();
            _hexScaleSlider.onValueChanged.RemoveAllListeners();
            _hexEdgeWidthSlider.onValueChanged.RemoveAllListeners();
            _houndstoothTeethSlider.onValueChanged.RemoveAllListeners();
            _houndstoothTilingXSlider.onValueChanged.RemoveAllListeners();
            _houndstoothTilingYSlider.onValueChanged.RemoveAllListeners();
            _rColor.onValueChanged.RemoveAllListeners();
            _gColor.onValueChanged.RemoveAllListeners();
            _bColor.onValueChanged.RemoveAllListeners();
            _intensityColor.onValueChanged.RemoveAllListeners();
        }

        private void HDRColorChanged(float value)
        {
            float factor = Mathf.Pow(2, _intensityColor.value);
            Color color = new Color(_rColor.value * factor, _gColor.value * factor, _bColor.value * factor);
            ShaderHelper.SetColor(_hdrMat, _hdrColorProp, color);
            ShaderHelper.SetColor(_targetMat, dShader.EdgeColorProp, color);
        }

        private void HoundTeethChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.HoundstoothTeethProp, value);
        }

        private void HoundTilingChanged(float value)
        {
            Vector2 v = new(_houndstoothTilingXSlider.value, _houndstoothTilingYSlider.value);
            ShaderHelper.SetVector2(_targetMat, dShader.HoundstoothTilingProp, v);
        }

        private void HexEdgeChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.HexEdgeWidthProp, value);
        }

        private void HexScaleChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.HexScaleProp, value);
        }

        private void DotsOffsetChanged(float value)
        {
            Vector2 v = new(_dotsOffsetXSlider.value, _dotsOffsetYSlider.value);
            ShaderHelper.SetVector2(_targetMat, dShader.DotsOffsetProp, v);
        }

        private void DotsSizeChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.DotSizeProp, value);
        }

        private void VoronoiCellChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.VoronoiCellDensityProp, value);
        }

        private void VoronoiAngleChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.VoronoiAngleOffsetProp, value);
        }

        private void NoiseScaleChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.NoiseScaleProp, value);
        }

        private void BaseNoiseChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.BaseNoiseScaleProp, value);
        }

        private void DissolveAmtChanged(float value)
        {
            ShaderHelper.SetFloat(_targetMat, dShader.DissolveAmountProp, value);
        }

        private void PatternTypeChange(int value)
        {
            TurnAllOff();
            _targetRenderer.material = _materials[value];
            _targetMat = _targetRenderer.material;
            HDRColorChanged(0);
            BaseNoiseChanged(_baseNoiseSlider.value);
            DissolveAmtChanged(_dissolveAmtSlider.value);
            foreach (var obj in _AllGroups[value])
            {
                obj.SetActive(true);
            }
        }

        private void TurnAllOff()
        {
            for (int i = 0; i < _AllGroups.Length; i++)
            {
                for (int j = 0; j < _AllGroups[i].Length; j++)
                {
                    _AllGroups[i][j].SetActive(false);
                }
            }
        }
    }
}
