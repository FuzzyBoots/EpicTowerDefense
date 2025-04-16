using UnityEngine;

namespace DirtyCookStudio.Shader
{
    /// <summary>
    /// A helper class for interacting with the DissolveShader
    /// </summary>
    public class DissolveShader 
    {
        //PBR, unlit, Lit UI, Unlit UI
        public readonly ShaderProperty<Texture> MainTexProp;
        public readonly ShaderProperty<Color> SpecularColorProp;
        public readonly ShaderProperty<Texture> RoughnessTexProp;
        public readonly ShaderProperty<Texture> AOTexProp;
        public readonly ShaderProperty<Texture> NormalTexProp;
        public readonly ShaderProperty<float> SmoothnessProp;
        public readonly ShaderProperty<float> AOProp;
        public readonly ShaderProperty<Color> SpriteMaskProp;
        //Dissolve Texture
        public readonly ShaderProperty<Texture> DissolveTexProp;
        public readonly ShaderProperty<float> DissolveOpacityProp;
        public readonly ShaderProperty<bool> InvertDissolveProp;
        public readonly ShaderProperty<Vector2> DissolveTexTilingProp;
        public readonly ShaderProperty<Vector2> DissolveTexOffsetProp;
        //Edge Generation
        public readonly ShaderProperty<float> EdgeDepthProp;
        public readonly ShaderProperty<Color> EdgeColorProp;
        public readonly ShaderProperty<float> EdgeNoiseScaleProp;
        public readonly ShaderProperty<Vector2> EdgeDepthRangeProp;
        //Dissolve Amount
        public readonly ShaderProperty<Vector2> DissolveRangeProp;
        public readonly ShaderProperty<float> DissolveAmountProp;
        //Pattern Generation
        public readonly ShaderProperty<bool> InversePatternProp;
        public readonly ShaderProperty<float> BaseNoiseScaleProp;
        public readonly ShaderProperty<float> NoiseScaleProp;
        public readonly ShaderProperty<float> VoronoiAngleOffsetProp;
        public readonly ShaderProperty<float> VoronoiCellDensityProp;
        public readonly ShaderProperty<float> DotSizeProp;
        public readonly ShaderProperty<float> DotsOpacityProp;
        public readonly ShaderProperty<Vector2> DotsTilingProp;
        public readonly ShaderProperty<Vector2> DotsOffsetProp;
        public readonly ShaderProperty<float> HexOpactiyProp;
        public readonly ShaderProperty<Vector2> HexTilingProp;
        public readonly ShaderProperty<float> HexScaleProp;
        public readonly ShaderProperty<float> HexEdgeWidthProp;
        public readonly ShaderProperty<Vector2> HoundstoothTilingProp;
        public readonly ShaderProperty<float> HoundstoothTeethProp;
        public readonly ShaderProperty<float> HoundstoothOpacityProp;

        public DissolveShader()
        {
            MainTexProp = new(ReturnShaderId("_MainTex"));
            SpecularColorProp = new(ReturnShaderId("_SpecularColor"));
            RoughnessTexProp = new(ReturnShaderId("_Roughness"));
            AOTexProp = new(ReturnShaderId("_AmbientOcclusion"));
            NormalTexProp = new(ReturnShaderId("_Normal"));
            SmoothnessProp = new(ReturnShaderId("_Smoothness"));
            AOProp = new(ReturnShaderId("_AOSlider"));
            SpriteMaskProp = new(ReturnShaderId("_SpriteMask"));
            DissolveTexProp = new(ReturnShaderId("_DissolveTex"));
            DissolveOpacityProp = new(ReturnShaderId("_DissolveTexOpacity"));
            InvertDissolveProp = new(ReturnShaderId("_InvertDissolveTex"));
            DissolveTexTilingProp = new(ReturnShaderId("_DissolveTexTiling"));
            DissolveTexOffsetProp = new(ReturnShaderId(("_DissolveTexOffset")));
            EdgeDepthProp = new(ReturnShaderId("_EdgeDepth"));
            EdgeColorProp = new(ReturnShaderId("_EdgeTint"));
            EdgeNoiseScaleProp = new(ReturnShaderId("_EdgeNoiseScale"));
            EdgeDepthRangeProp = new(ReturnShaderId("_EdgeDepthRange"));
            DissolveRangeProp = new(ReturnShaderId("_DissolveRange"));
            DissolveAmountProp = new(ReturnShaderId("_DissolveAmount"));
            InversePatternProp = new(ReturnShaderId("_Inverse"));
            BaseNoiseScaleProp = new(ReturnShaderId("_RoughNoiseScale"));
            NoiseScaleProp = new(ReturnShaderId("_GradientNoiseScale"));
            VoronoiAngleOffsetProp = new(ReturnShaderId("_VoronoiAngleOffset"));
            VoronoiCellDensityProp = new(ReturnShaderId("_VoronoiCellDensity"));
            DotSizeProp = new(ReturnShaderId("_DotsSize"));
            DotsOpacityProp = new(ReturnShaderId("_DotsOpacity"));
            DotsTilingProp = new(ReturnShaderId("_DotsTiling"));
            DotsOffsetProp = new(ReturnShaderId("_DotsOffset"));
            HexOpactiyProp = new(ReturnShaderId("_HexOpacity"));
            HexTilingProp = new(ReturnShaderId("_HexTiling"));
            HexScaleProp = new(ReturnShaderId("_HexScale"));
            HexEdgeWidthProp = new(ReturnShaderId("_HexEdgeWidth"));
            HoundstoothTilingProp = new(ReturnShaderId("_HoundstoothTiling"));
            HoundstoothTeethProp = new(ReturnShaderId("_HoundstoothTeeth"));
            HoundstoothOpacityProp = new(ReturnShaderId("_HoundstoothOpacity"));
        }



        int ReturnShaderId(string name)
        {
            return UnityEngine.Shader.PropertyToID(name);
        }
    }
}
