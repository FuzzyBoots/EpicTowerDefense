using UnityEngine;

namespace DirtyCookStudio.Shader
{
    /// <summary>
    /// A helper class for type safe shader properties
    /// </summary>
    public class ShaderProperty<T>
    {
        public int ShaderID { get; }

        public ShaderProperty(int shaderID)
        {
            this.ShaderID = shaderID;
        }
    }

    /// <summary>
    /// A helper class for type safe shader properties
    /// </summary>
    public static class ShaderHelper
    {
        /// <summary>
        /// Attempts to update the float value of the given material
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public static void SetFloat(Material mat, ShaderProperty<float> prop, float value)
        {
            mat.SetFloat(prop.ShaderID, value);
        }

        /// <summary>
        /// Attempts to return the float value of the given material
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static float GetFloat(Material mat, ShaderProperty<float> prop)
        {
            return mat.GetFloat(prop.ShaderID);
        }

        /// <summary>
        /// Attempts to update the Color value of the given material
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public static void SetColor(Material mat, ShaderProperty<Color> prop, Color value)
        {
            mat.SetColor(prop.ShaderID, value);
        }

        /// <summary>
        /// Returns a float4 color value from the given material
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static Color GetColor(Material mat, ShaderProperty<Color> prop)
        {
            return mat.GetColor(prop.ShaderID);
        }

        /// <summary>
        /// Attempts to update the texture value of the given material
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public static void SetTexture(Material mat, ShaderProperty<Texture> prop, Texture value)
        {
            mat.SetTexture(prop.ShaderID, value);
        }

        /// <summary>
        /// Returns a float4 texture
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static Texture GetTexture(Material mat, ShaderProperty<Texture> prop)
        {
            return mat.GetTexture(prop.ShaderID);
        }

        /// <summary>
        /// Attempts to update the xy value of a vector4 for the given material
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public static void SetVector2(Material mat, ShaderProperty<Vector2> prop, Vector2 value)
        {
            mat.SetVector(prop.ShaderID, value);
        }

        /// <summary>
        /// Returns just the xy coordinates of a Vector value from the given material
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static Vector2 GetVector2(Material mat, ShaderProperty<Vector2> prop)
        {
            return mat.GetVector(prop.ShaderID);
        }
    }
}
