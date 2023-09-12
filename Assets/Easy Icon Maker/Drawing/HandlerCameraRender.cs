using System.IO;
using UnityEditor;
using UnityEngine;

namespace EasyIconMaker
{
#if UNITY_EDITOR
    public class HandlerCameraRender
    {
        public static Texture2D MakePreview(Camera captureCamera, string folderPathf, string name, int size = 256, Texture texBG = null)
        {
            Texture2D preview = GetTexture(captureCamera, size, texBG); 
            if (folderPathf != null)
            {
                string iconPath = folderPathf + name + ".png";
                File.WriteAllBytes(iconPath, preview.EncodeToPNG()); 
                AssetDatabase.ImportAsset(iconPath, ImportAssetOptions.ForceUpdate);
            }

            return preview;
        }


        private static Texture2D GetTexture(Camera captureCamera, int size, Texture texBG)
        {
            Texture2D texture = new Texture2D(size, size);

            RenderTexture oldTexture = captureCamera.targetTexture;
            RenderTexture targetTexture = RenderTexture.GetTemporary(size, size, 16, RenderTextureFormat.ARGB32);

            if (texBG != null)
                Graphics.Blit(texBG, targetTexture);

            captureCamera.rect = new Rect(new Vector2(0, 0), new Vector2(256, 256));
            captureCamera.targetTexture = targetTexture;
            captureCamera.forceIntoRenderTexture = true;
            captureCamera.Render();

            RenderTexture tempActive = RenderTexture.active;
            RenderTexture.active = targetTexture;

            Rect rect = new Rect(0, 0, size, size);
            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            RenderTexture.active = tempActive;

            captureCamera.targetTexture = oldTexture;
            RenderTexture.ReleaseTemporary(targetTexture);

            return texture;
        }

        public static void ChangeTexture(Camera captureCamera, int size, Texture texBG)
        {
            Graphics.Blit(texBG, captureCamera.targetTexture);
        }
    }
#endif
}