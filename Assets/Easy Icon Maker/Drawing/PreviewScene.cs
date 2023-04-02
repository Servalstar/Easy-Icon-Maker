using UnityEditor;
using UnityEngine;

namespace EasyIconMaker
{
#if UNITY_EDITOR
    public class PreviewScene
    {
        public PreviewScene()
        {
            InitPreviewScene();
        }
        
        public PreviewRenderUtility PreviewUtility { get; set; } 

        private void InitPreviewScene()
        {
            if (PreviewUtility == null)
            {
                PreviewUtility = new PreviewRenderUtility();

                SetCameraSettings();
                SetLightsSettings();
                CreateArrows();
            }
        }


        public GameObject CameraContainer { get; set; }
        public Camera PreviewCamera { get; set; }

        private void SetCameraSettings()
        {
            PreviewCamera = PreviewUtility.camera;

            GameObject tempCameraContainer = new GameObject();
            PreviewUtility.AddSingleGO(tempCameraContainer);
            CameraContainer = tempCameraContainer;
            CameraContainer.transform.localRotation = Quaternion.Euler(-19, 309, 0);
            PreviewCamera.transform.SetParent(CameraContainer.transform);

            PreviewCamera.cameraType = CameraType.Preview;
            PreviewCamera.clearFlags = CameraClearFlags.SolidColor;
            PreviewCamera.backgroundColor = Color.white;
            PreviewCamera.fieldOfView = 30.0f;
            PreviewCamera.transform.localPosition = new Vector3(0, 0, 4);
            PreviewCamera.transform.LookAt(Vector3.zero);
            PreviewCamera.nearClipPlane = 0.1f;
            PreviewCamera.farClipPlane = 100;
        }


        public Light Light1 { get; set; }
        public Light Light2 { get; set; }

        private void SetLightsSettings()
        {
            Light1 = PreviewUtility.lights[0];
            Light2 = PreviewUtility.lights[1];

            Light1.intensity = 1.0f;
            Light2.intensity = 1.0f;

            Light1.color = new Color(1f, 0.95f, 0.84f, 1f);
            Light2.color = new Color(0.5f, 0.5f, 0.5f, 1f);

            Light1.transform.rotation = Quaternion.Euler(27, 120, 0);
            Light2.transform.rotation = Quaternion.Euler(340, 218, 0);
        }


        public GameObject Light1Arrow { get; set; }
        public GameObject Light2Arrow { get; set; }
        public MeshRenderer Light1MeshRenderer { get; set; }
        public MeshRenderer Light2MeshRenderer { get; set; }

        private void CreateArrows()
        {
            GameObject arrowMesh = Resources.Load<GameObject>("Models/direction arrow");

            Light1Arrow = PreviewUtility.InstantiatePrefabInScene(arrowMesh.gameObject);
            Light2Arrow = PreviewUtility.InstantiatePrefabInScene(arrowMesh.gameObject);

            Light1Arrow.transform.position = new Vector3(0, 0.75f, 0);
            Light2Arrow.transform.position = new Vector3(0, 0.75f, 0);

            Light1MeshRenderer = Light1Arrow.GetComponentInChildren<MeshRenderer>();
            Light2MeshRenderer = Light2Arrow.GetComponentInChildren<MeshRenderer>();

            Light1MeshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
            Light2MeshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

            Light1MeshRenderer.sharedMaterial.color = Color.green;
            Light2MeshRenderer.sharedMaterial.color = Color.yellow;

            Light1MeshRenderer.enabled = false;
            Light2MeshRenderer.enabled = false;
        }


        public GameObject PreviewObjectInstance { get; set; }

        public void InitInstance(GameObject go)
        {
            InitPreviewScene();

            if (go != null && go.transform.GetComponentsInChildren<MeshRenderer>().Length > 0)
            {
                if (PreviewObjectInstance != null)
                    DestroyObject(PreviewObjectInstance);

                PreviewObjectInstance = PreviewUtility.InstantiatePrefabInScene(go);

                SetScaleGO(PreviewObjectInstance);
            }
        }


        private void DestroyObject(GameObject go)
        {
            // если выбран вложенный объект в префабе, то создаётся весь префаб и вложенный потом не получится удалить, 
            // так что нужно удалять родительский объект префаба
            while (true)
            {
                if (go.transform.parent != null)
                    go = go.transform.parent.gameObject;
                else
                    break;
            }
            Object.DestroyImmediate(go);
        }


        private Vector3 m_BasePosition;

        private void SetScaleGO(GameObject go)
        {
            if (go != null)
            {
                go.transform.position = Vector3.zero;
                Bounds bounds = GetBounds(go);
                m_BasePosition = new Vector3(bounds.center.x, bounds.center.y - bounds.size.y / 2, bounds.center.z);
                float maxSize = Mathf.Max(new float[] { bounds.size.x, bounds.size.y, bounds.size.z });
                go.transform.localScale *= 1 / maxSize;
                go.transform.position += Vector3.zero - bounds.center * (1 / maxSize);
            }
        }


        // префаб может состоять из нескольких видимых объектов и все их нужно включить в ограничивающую рамку
        private Bounds GetBounds(GameObject go)
        {
            Bounds bounds = new Bounds();
            MeshRenderer[] renderers = go.transform.GetComponentsInChildren<MeshRenderer>();
            foreach (var item in renderers)
            {
                bounds.Encapsulate(item.bounds.min);
                bounds.Encapsulate(item.bounds.max);
            }
            return bounds;
        }


        public Texture2D MakeImage(string folderPathf, string name, int size, Texture texBG)
        {
            Light1.enabled = true;
            Light2.enabled = true;

            if (folderPathf == "default")
                folderPathf = "Assets/Easy Icon Maker/";

            if (name == "default" && PreviewObjectInstance != null)
                name = PreviewObjectInstance.name;

            Texture2D image = HandlerCameraRender.MakePreview(PreviewCamera, folderPathf, name, size, texBG);

            AssetDatabase.Refresh();

            return image;
        }


        public void Cleanup()
        {
            if (PreviewUtility != null)
            {
                PreviewUtility.Cleanup();
                PreviewUtility = null;
            }
        }
    }
#endif
}