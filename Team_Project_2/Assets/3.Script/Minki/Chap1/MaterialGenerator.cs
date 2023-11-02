using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialGenerator
{
    public static void SetTexture(Material material, string textureName, Texture2D texture)
    {
        if (material != null && texture != null)
        {
            material.SetTexture(textureName, texture);
        }
    }
}
