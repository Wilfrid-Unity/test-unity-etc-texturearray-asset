using UnityEngine;
using UnityEditor;

public class Texture2DArrayAssetCreator : MonoBehaviour
{
    [MenuItem("Texture2DArrayAssetCreator/Generate ETC2 Texture2DArray asset")]
    public static void GenerateETC2Texture2DArrayAsset()
    {
        Texture2DArray textureArray = new Texture2DArray( 128, 128, 2, TextureFormat.ETC2_RGBA8, false, false );
        textureArray.Apply(false);

        RenderTexture previousRt = RenderTexture.active;

        {
            RenderTexture rt = new RenderTexture( 128, 128, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default );
            rt.Create();
            RenderTexture.active = rt;

            // Copy texture 00 into textureArray slice 0
            {
                Texture2D sourceTexture00 = new Texture2D(128, 128);
                sourceTexture00.LoadImage(System.IO.File.ReadAllBytes("Assets/Editor/sourceTexture00.png"));
                sourceTexture00.Apply();
                Graphics.Blit(sourceTexture00, rt);

                Texture2D sourceTexture00compressed = new Texture2D( 128, 128, TextureFormat.RGBAFloat, false, false );
                sourceTexture00compressed.ReadPixels(new Rect(0, 0, 128, 128), 0, 0, false);
                RenderTexture.active = null;

                EditorUtility.CompressTexture(sourceTexture00compressed, TextureFormat.ETC2_RGBA8, 100);
                sourceTexture00compressed.Apply(false);

                Graphics.CopyTexture(sourceTexture00compressed, 0, 0, textureArray, 0 /*arrayIndex*/, 0);
            }

            // Copy texture 01 into textureArray slice 1
            {
                Texture2D sourceTexture01 = new Texture2D(128, 128);
                sourceTexture01.LoadImage(System.IO.File.ReadAllBytes("Assets/Editor/sourceTexture01.png"));
                sourceTexture01.Apply();
                Graphics.Blit(sourceTexture01, rt);

                Texture2D sourceTexture01compressed = new Texture2D( 128, 128, TextureFormat.RGBAFloat, false, false );
                sourceTexture01compressed.ReadPixels(new Rect(0, 0, 128, 128), 0, 0, false);
                RenderTexture.active = null;

                EditorUtility.CompressTexture(sourceTexture01compressed, TextureFormat.ETC2_RGBA8, 100);
                sourceTexture01compressed.Apply(false);

                Graphics.CopyTexture(sourceTexture01compressed, 0, 0, textureArray, 1 /*arrayIndex*/, 0);
            }

            rt.Release();
        }

        RenderTexture.active = previousRt;

        Texture2DArray outfile = AssetDatabase.LoadMainAssetAtPath( "Assets/ETCTextureArray.asset" ) as Texture2DArray;
        if (outfile != null)
        {
            EditorUtility.CopySerialized(textureArray, outfile);
            AssetDatabase.SaveAssets();
            EditorGUIUtility.PingObject(outfile);
        }
        else
        {
            AssetDatabase.CreateAsset(textureArray, "Assets/ETCTextureArray.asset");
            EditorGUIUtility.PingObject(textureArray);
        }
    }
}
