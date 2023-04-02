using UnityEditor;
using UnityEngine;

namespace EasyIconMaker
{
    public class PreviewWindow : EditorWindow
    {
        private static PreviewWindow window;
        private PreviewWindowDrawer previewRender;

        [MenuItem("Tools/Easy Icon Maker")]
        private static void Init()
        {
            window = (PreviewWindow)GetWindow(typeof(PreviewWindow));
            window.titleContent = new GUIContent(" Easy Icon Maker", Resources.Load<Texture>("Textures/icon-editor window"));
            window.Show();
        }


        private PreviewScene previewScene;
        private Rect previewRect;

        private void OnEnable()
        {

            previewScene = new PreviewScene();

            PreviewWindowDrawer.PreviewSceneInstance = previewScene;
            previewRender = new PreviewWindowDrawer();

            previewRect = new Rect();

            if (Selection.activeGameObject)
                previewScene.InitInstance(Selection.activeGameObject);
        }


        private void OnSelectionChange()
        {
            previewScene.InitInstance(Selection.activeGameObject);
            window.Repaint();
        }


        private const float HEIGHT_WINDOW_PERCENT = 40;
        private const float WIDTH_WINDOW_PERCENT = 57.5f;
        private const float BORDER_WINDOW_PERCENT = 2.5f;

        private void OnGUI()
        {
            previewRect.height = position.height / 100 * HEIGHT_WINDOW_PERCENT;
            previewRect.width = previewRect.height;
            previewRect.y = position.height / 100 * WIDTH_WINDOW_PERCENT;
            previewRect.x = (position.width - previewRect.width) / 2;
            previewRender.CustomOnPreviewGUI(previewRect, position, position.height / 100 * BORDER_WINDOW_PERCENT, null);
        }


        private void OnDisable()
        {
            previewScene.Cleanup();
        }
    }
}