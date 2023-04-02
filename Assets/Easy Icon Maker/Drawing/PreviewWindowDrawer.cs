using UnityEditor;
using UnityEngine;

namespace EasyIconMaker
{
#if UNITY_EDITOR
    public class PreviewWindowDrawer
    {
        public static PreviewScene PreviewSceneInstance { get; set; }
        private GUIStyle createButtonStyle;
        private GUIStyle greenFont;
        private GUIStyle yellowFont;
        private GUIStyle sectionsFont;
        private GUIStyle sectionIcon;
        private GUIStyle sectionCollapseArrow;
        private Texture arrowUpButton;
        private Texture arrowDownButton;
        private Texture iconCamera;
        private Texture iconLight;
        private Texture iconBackground;
        private Texture iconFolder;

        public void SetStylesAndImages()
        {
            createButtonStyle = PreviewGUIStyles.CreateButtonStyle;
            greenFont = PreviewGUIStyles.GreenFont;
            yellowFont = PreviewGUIStyles.YellowFont;
            sectionsFont = PreviewGUIStyles.SectionHeaderFont;
            sectionIcon = PreviewGUIStyles.SectionIcon;
            sectionCollapseArrow = PreviewGUIStyles.SectionCollapseArrow;

            arrowUpButton = Resources.Load<Texture>("Textures/arrow-up");
            arrowDownButton = Resources.Load<Texture>("Textures/arrow-down");
            iconCamera = Resources.Load<Texture>("Textures/icon-camera");
            iconLight = Resources.Load<Texture>("Textures/icon-light");
            iconBackground = Resources.Load<Texture>("Textures/icon-background");
            iconFolder = Resources.Load<Texture>("Textures/icon-folder");
        }


        #region DRAWING

        Vector2 scrollPosition;

        public void CustomOnPreviewGUI(Rect preview, Rect editorWindow, float padding, GUIStyle background)
        {
            if (createButtonStyle == null)
                SetStylesAndImages();

            if (PreviewSceneInstance.PreviewObjectInstance != null)
            {
                EditorGUILayout.BeginVertical(GUILayout.MaxHeight(editorWindow.height - preview.height - padding * 2));
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
                DrawRenderSettings();
                DrawCameraSettings(preview);
                DrawLightsSettings();
                DrawBackgroungSettings();
                EditorGUILayout.EndScrollView();
                MouseHandler(preview);
                EditorGUILayout.EndVertical();

                PreviewSceneInstance.PreviewUtility.BeginPreview(preview, background);
                HandlerCameraRender.ChangeTexture(PreviewSceneInstance.PreviewUtility.camera, 256, backgroundTexture);
                PreviewSceneInstance.PreviewUtility.camera.backgroundColor = backgroundColor;
                PreviewSceneInstance.PreviewUtility.Render();
                PreviewSceneInstance.PreviewUtility.EndAndDrawPreview(preview);
            }
            else
            {
                EditorGUILayout.HelpBox("Select an object from the project window that contains the MeshRenderer component.", MessageType.Info);
            }
        }


        float sizeImagePreview = 256;
        string folderPathf = "";

        private void DrawRenderSettings()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Size image");
            sizeImagePreview = EditorGUILayout.Slider(sizeImagePreview, 64, 1024);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(" Create Icon", createButtonStyle, GUILayout.Height(24), GUILayout.Width(120)))
            {
                CheckDefaultFolderPathf();

                PreviewSceneInstance.Light1.enabled = true;
                PreviewSceneInstance.Light2.enabled = true;

                if (clearFlags == ClearFlags.Image)
                    HandlerCameraRender.MakePreview(PreviewSceneInstance.PreviewCamera, folderPathf, 
                        PreviewSceneInstance.PreviewObjectInstance.name, (int)sizeImagePreview, backgroundTexture);
                else
                    HandlerCameraRender.MakePreview(PreviewSceneInstance.PreviewCamera, folderPathf, 
                        PreviewSceneInstance.PreviewObjectInstance.name, (int)sizeImagePreview);

                AssetDatabase.Refresh();
            }

            if (GUILayout.Button(iconFolder, GUILayout.Height(24), GUILayout.Width(32)))
            {
                CheckDefaultFolderPathf();

                string newFolderPathf = EditorUtility.SaveFolderPanel("Export folder", folderPathf, "");

                if (newFolderPathf.Length > 0)
                    folderPathf = newFolderPathf + "/";
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.EndVertical();
        }

        private void CheckDefaultFolderPathf()
        {
            if (folderPathf == "")
                folderPathf = "Assets/Easy Icon Maker/";
        }


        private float cameraRotX = PreviewSceneInstance.CameraContainer.transform.localEulerAngles.x - 360f;
        private float cameraRotY = PreviewSceneInstance.CameraContainer.transform.localEulerAngles.y;
        private bool cameraOrt = PreviewSceneInstance.PreviewCamera.orthographic;
        private float cameraPosX = PreviewSceneInstance.PreviewCamera.transform.localPosition.x;
        private float cameraPosY = PreviewSceneInstance.PreviewCamera.transform.localPosition.y;
        private float cameraPosZ = PreviewSceneInstance.PreviewCamera.transform.localPosition.z;
        private bool collapseCameraSettings = true;
        private bool lookInCenter = true;

        private void DrawCameraSettings(Rect r)
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal(GUILayout.Height(36));
            EditorGUILayout.Space();

            if (GUILayout.Button(iconCamera, sectionIcon, GUILayout.Width(28), GUILayout.Height(28)) ||
                GUILayout.Button("CAMERA", sectionsFont, GUILayout.MaxWidth(float.MaxValue)) ||
                GUILayout.Button(collapseCameraSettings ? arrowDownButton : arrowUpButton, sectionCollapseArrow, GUILayout.Width(24), GUILayout.Height(24)))
                collapseCameraSettings = !collapseCameraSettings;

            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();

            if (!collapseCameraSettings)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Camera Rotation X");
                cameraRotX = EditorGUILayout.Slider(cameraRotX, 89, -89);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Camera Rotation Y");
                cameraRotY = EditorGUILayout.Slider(cameraRotY, 0, 360);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Camera Distance  ");
                cameraPosZ = EditorGUILayout.Slider(cameraPosZ, 0.1f, 5);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                cameraOrt = EditorGUILayout.Toggle("Projection Orthographic", cameraOrt);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Look At Center");
                lookInCenter = EditorGUILayout.Toggle("", lookInCenter);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (!lookInCenter)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Camera Position X");
                    cameraPosX = EditorGUILayout.Slider(cameraPosX, -2, 2);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Camera Position Y");
                    cameraPosY = EditorGUILayout.Slider(cameraPosY, -2, 2);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                }
                else
                {
                    cameraPosX = 0;
                    cameraPosY = 0;
                }
            }

            EditorGUILayout.EndVertical();
        }


        private bool isMousDown = false;

        private void MouseHandler(Rect r)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (r.Contains(Event.current.mousePosition))
                    isMousDown = true;
            }

            if (Event.current.type == EventType.MouseUp)
                isMousDown = false;

            if (Event.current.type == EventType.MouseDrag)
            {
                if (isMousDown)
                {
                    cameraRotX = Mathf.Clamp(cameraRotX - Event.current.delta.y * 0.5f, -89, 89);
                    cameraRotY = cameraRotY + Event.current.delta.x * 0.5f;
                    if (cameraRotY > 360)
                        cameraRotY -= 360;
                    else if (cameraRotY < 0)
                        cameraRotY += 360;
                    GUI.changed = true;
                }
            }

            if (Event.current.type == EventType.ScrollWheel)
            {
                if (r.Contains(Event.current.mousePosition))
                {
                    float deltaZoomMultiplier = 0.1f;
                    cameraPosZ = Mathf.Clamp(cameraPosZ -= HandleUtility.niceMouseDeltaZoom * deltaZoomMultiplier, 0.1f, 10);
                    GUI.changed = true;
                }
            }

            MoveCamera();
        }

        private void MoveCamera()
        {
            PreviewSceneInstance.CameraContainer.transform.localRotation = Quaternion.Euler(cameraRotX, cameraRotY, 0);
            PreviewSceneInstance.PreviewCamera.orthographic = cameraOrt;
            float SCALE_CAMERA_CORRECTION = 4f;

            if (cameraOrt)
                PreviewSceneInstance.PreviewCamera.orthographicSize = cameraPosZ / SCALE_CAMERA_CORRECTION;

            PreviewSceneInstance.PreviewCamera.transform.localPosition = new Vector3(cameraPosX, cameraPosY, cameraPosZ);

            if (lookInCenter)
                PreviewSceneInstance.PreviewCamera.transform.LookAt(Vector3.zero);
        }


        private bool collapseLightsSettings = true;
        private bool visualiseLightsDirection = true;
        private Color light1Color = PreviewSceneInstance.Light1.color;
        private Color light2Color = PreviewSceneInstance.Light2.color;
        private float light1RotX = 27;
        private float light1RotY = 120;
        private float light2RotX = 340;
        private float light2RotY = 218;

        private void DrawLightsSettings()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal(GUILayout.Height(36));
            EditorGUILayout.Space();

            if (GUILayout.Button(iconLight, sectionIcon, GUILayout.Width(28), GUILayout.Height(28)) ||
                GUILayout.Button("LIGHT", sectionsFont, GUILayout.MaxWidth(float.MaxValue)) ||
                GUILayout.Button(collapseLightsSettings ? arrowDownButton : arrowUpButton, sectionCollapseArrow, GUILayout.Width(24), GUILayout.Height(24)))
                collapseLightsSettings = !collapseLightsSettings;

            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();

            if (!collapseLightsSettings)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Light 1 Color");
                light1Color = EditorGUILayout.ColorField(light1Color);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Light 2 Color");
                light2Color = EditorGUILayout.ColorField(light2Color);
                EditorGUILayout.EndHorizontal();

                PreviewSceneInstance.Light1.color = light1Color;
                PreviewSceneInstance.Light2.color = light2Color;

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Visualise Lights Directions");
                visualiseLightsDirection = EditorGUILayout.Toggle("", visualiseLightsDirection);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                if (visualiseLightsDirection)
                {
                    PreviewSceneInstance.Light1MeshRenderer.enabled = true;
                    PreviewSceneInstance.Light2MeshRenderer.enabled = true;
                }
                else
                {
                    PreviewSceneInstance.Light1MeshRenderer.enabled = false;
                    PreviewSceneInstance.Light2MeshRenderer.enabled = false;
                }

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Light 1 Rotation X", greenFont);
                light1RotX = EditorGUILayout.Slider(light1RotX, 0, 360);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Light 1 Rotation Y", greenFont);
                light1RotY = EditorGUILayout.Slider(light1RotY, 0, 360);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Light 2 Rotation X", yellowFont);
                light2RotX = EditorGUILayout.Slider(light2RotX, 0, 360);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Light 2 Rotation Y", yellowFont);
                light2RotY = EditorGUILayout.Slider(light2RotY, 0, 360);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                PreviewSceneInstance.Light1.transform.rotation = Quaternion.Euler(light1RotX, light1RotY, 0);
                PreviewSceneInstance.Light2.transform.rotation = Quaternion.Euler(light2RotX, light2RotY, 0);

                PreviewSceneInstance.Light1Arrow.transform.rotation = Quaternion.Euler(light1RotX, light1RotY, 0);
                PreviewSceneInstance.Light2Arrow.transform.rotation = Quaternion.Euler(light2RotX, light2RotY, 0);
            }

            EditorGUILayout.EndVertical();
        }


        private bool collapseBackgroungSettings = true;
        private enum ClearFlags
        {
            SolidColor = 2,
            Image = 4
        };
        private ClearFlags clearFlags = (ClearFlags)PreviewSceneInstance.PreviewCamera.clearFlags;
        private Color backgroundColor = PreviewSceneInstance.PreviewCamera.backgroundColor;
        private Texture backgroundTexture;

        private void DrawBackgroungSettings()
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal(GUILayout.Height(36));
            EditorGUILayout.Space();

            if (GUILayout.Button(iconBackground, sectionIcon, GUILayout.Width(28), GUILayout.Height(28)) ||
                GUILayout.Button("BACKGROUND", sectionsFont, GUILayout.MaxWidth(float.MaxValue)) ||
                GUILayout.Button(collapseBackgroungSettings ? arrowDownButton : arrowUpButton, sectionCollapseArrow, GUILayout.Width(24), GUILayout.Height(24)))
                collapseBackgroungSettings = !collapseBackgroungSettings;

            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();

            if (!collapseBackgroungSettings)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                clearFlags = (ClearFlags)EditorGUILayout.EnumPopup("Background Type", clearFlags);

                EditorGUILayout.Space();

                switch ((int)clearFlags)
                {
                    case 2:
                        PreviewSceneInstance.PreviewCamera.clearFlags = (CameraClearFlags)clearFlags;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Background Color");
                        backgroundColor = EditorGUILayout.ColorField(backgroundColor);
                        PreviewSceneInstance.PreviewCamera.backgroundColor = backgroundColor;
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                        break;
                    case 4:
                        PreviewSceneInstance.PreviewCamera.clearFlags = (CameraClearFlags)clearFlags;
                        backgroundTexture = (Texture)EditorGUILayout.ObjectField("Image", backgroundTexture, typeof(Texture), false);
                        EditorGUILayout.Space();
                        break;
                }
            }

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
#endif
}