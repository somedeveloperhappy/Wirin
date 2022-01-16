using CanvasSystem;
using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.UpgradeMenu
{
    public class ShineUIParticleFx : MonoBehaviour, CanvasSystem.IOnCanvasDisabled, CanvasSystem.IOnCanvasEnabled
    {
        RectTransform m_rectTransform;
        Rect m_boundry;

        public Rect boundry => m_boundry;

        public void Awake()
        {
            UpdateBoundry();
        }

        public void UpdateBoundry()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_boundry = m_rectTransform.rect;
        }

        public Sprite[] sprites;
        public int emissionCount = 10;
        public Gradient color;

        [Space(10)]
        public float restoreTime = 3;
        public float restoreTimeRange = 2;
        public float lifeTime = 3;
        public float lifeTimeRange = 2;

        [Space(10)]
        public float moveSpeed = 2;
        public float moveSpeedRange = 1;
        public float scaleSpeed = 1;
        public float scaleSpeedRange = 0.5f;
        public float startScale = 0.3f;
        public float startScaleRange = 0.2f;

        public float rotationSpeed = 5;
        public float rotationSpeedRange = 5;


        public class Emission
        {
            public Image image;
            public float startTime, endTime;
            public float scaleSpeed;
            public Vector2 moveSpeed;
            public float rotSpeed;
        }
        private Emission[] m_emissions;


        private void Start()
        {
            InitEmissions();
        }

        void InitEmissions()
        {
            // create new emissions
            m_emissions = new Emission[emissionCount];
            for (int i = 0; i < emissionCount; i++)
            {
                m_emissions[i] = new Emission();
                var img = new GameObject($"emission {i}");
                img.transform.parent = transform;
                img.AddComponent<RectTransform>();
                m_emissions[i].image = img.AddComponent<Image>();
                ReScheduleEmission(m_emissions[i]);
                m_emissions[i].image.raycastTarget = false;
            }
        }

        private void Update()
        {
            for (int i = 0; i < emissionCount; i++)
            {
                if (Time.realtimeSinceStartup >= m_emissions[i].startTime)
                {
                    if (Time.realtimeSinceStartup < m_emissions[i].endTime)
                    {
                        // in range for move!
                        MoveEmission(m_emissions[i]);
                    }
                    else
                    {
                        // times just up for this emission
                        ReScheduleEmission(m_emissions[i]);
                    }
                }
            }
        }

        private void ReScheduleEmission(Emission emission)
        {
            emission.startTime = Time.realtimeSinceStartup + Random.Range(restoreTime - restoreTimeRange, restoreTime + restoreTimeRange);
            emission.endTime = Time.realtimeSinceStartup + Random.Range(lifeTime - lifeTimeRange, lifeTime + lifeTimeRange);
            emission.moveSpeed = GetRandomNormalizedVector2() * Random.Range(moveSpeed - moveSpeedRange, moveSpeed + moveSpeedRange);
            emission.scaleSpeed = Random.Range(scaleSpeed - scaleSpeedRange, scaleSpeed + scaleSpeedRange);
            emission.rotSpeed = Random.Range(rotationSpeed - rotationSpeedRange, rotationSpeed + rotationSpeedRange);

            // sprite change
            emission.image.sprite = GetRandomSprite();
            emission.image.SetNativeSize();
            emission.image.preserveAspect = true;

            // transformation
            emission.image.rectTransform.localPosition = GetRandomPosInBoundry();
            emission.image.rectTransform.localScale = Vector3.one * Random.Range(startScale - startScaleRange, startScale + startScaleRange);
            emission.image.rectTransform.localRotation = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));

            // invisible while not it's turn
            var col = emission.image.color; col.a = 0;
            emission.image.color = col;
        }

        public void MoveEmission(Emission emission)
        {
            var t = Mathf.InverseLerp(emission.startTime, emission.endTime, Time.realtimeSinceStartup);
            emission.image.color = color.Evaluate(t);
            emission.image.rectTransform.position += (Vector3)emission.moveSpeed * Time.unscaledDeltaTime;
            emission.image.rectTransform.localScale += Vector3.one * emission.scaleSpeed * Time.unscaledDeltaTime;
            emission.image.rectTransform.Rotate(Vector3.forward * emission.rotSpeed * Time.unscaledDeltaTime * Mathf.Rad2Deg);
        }

        public Vector3 GetRandomPosInBoundry()
        {
            return (Vector3)new Vector2(
                Random.Range(m_boundry.xMin, m_boundry.xMax),
                Random.Range(m_boundry.yMin, m_boundry.yMax));
        }
        public Vector2 GetRandomNormalizedVector2()
        {
            return new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;
        }

        public Sprite GetRandomSprite() => sprites[Random.Range(0, sprites.Length)];

        void IOnCanvasEnabled.OnCanvasEnable() => this.enabled = true;
        void IOnCanvasDisabled.OnCanvasDisable() => this.enabled = false;
        private void OnDestroy()
        {
            for (int i = m_emissions.Length - 1; i >= 0; i--)
                Destroy(m_emissions[i].image.gameObject);
        }
    }
}
