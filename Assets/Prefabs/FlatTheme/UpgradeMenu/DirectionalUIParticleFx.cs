using CanvasSystem;
using SimpleScripts;
using UnityEngine;
using UnityEngine.UI;

namespace FlatTheme.UpgradeMenu
{
    public class DirectionalUIParticleFx : MonoBehaviour, IOnCanvasEnabled, IOnCanvasDisabled
    {
        Rect m_boundry;
        RectTransform m_rectTransform;

        public Sprite[] sprites;
        public int emissionCount = 10;
        public Gradient color;

        [System.Serializable]
        public class GradientColorAnimator
        {
            public Color[] cols;
            public float speed = 0.2f;
            public float stayDuration = 2;
            public int keyIndex;

            [HideInInspector] public float lastChangeTime = 0;
            [HideInInspector] public Color nextCol;
            [HideInInspector] public bool isChanging = false;
        }
        public GradientColorAnimator m_gradientColorAnimator;

        [Space(10)]
        public MinMax restoreTimeRange = new MinMax(0, 3);
        public MinMax lifeTimeRange = new MinMax(3, 7);

        [Space(10)]
        public MinMax moveSpeedRange = new MinMax(5, 20);
        public MinMax scaleSpeedRange = new MinMax(0.01f, 0.4f);
        public MinMax rotationSpeedRange = new MinMax(30, 30);

        [Space(10)]
        public MinMax startScaleRange = new MinMax(0.6f, 1.2f);
        public MinMax startRotationRange = new MinMax(0, 0);



        public class Emission
        {
            public Image image;
            public float startTime, endTime;
            public float scaleSpeed;
            public Vector2 moveSpeed;
            public float rotateSpeed;
        }

        private Emission[] m_emissions;

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_boundry = m_rectTransform.rect;
        }


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
                m_emissions[i].image.raycastTarget = false;
                ReScheduleEmission(m_emissions[i]);
            }

            // reset the color changer
            m_gradientColorAnimator.lastChangeTime = 0;
            m_gradientColorAnimator.isChanging = false;
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

            UpdateGradientColorChange();
        }

        private void UpdateGradientColorChange()
        {
            if (m_gradientColorAnimator.isChanging)
            {
                var colkeys = color.colorKeys;
                var col = colkeys[m_gradientColorAnimator.keyIndex].color;
                if (col == m_gradientColorAnimator.nextCol)
                {
                    // transition is done
                    m_gradientColorAnimator.isChanging = false;
                    m_gradientColorAnimator.lastChangeTime = Time.realtimeSinceStartup;
                }
                else
                {
                    // transition updates
                    col.r = Mathf.MoveTowards(col.r, m_gradientColorAnimator.nextCol.r, m_gradientColorAnimator.speed * Time.unscaledDeltaTime);
                    col.g = Mathf.MoveTowards(col.g, m_gradientColorAnimator.nextCol.g, m_gradientColorAnimator.speed * Time.unscaledDeltaTime);
                    col.b = Mathf.MoveTowards(col.b, m_gradientColorAnimator.nextCol.b, m_gradientColorAnimator.speed * Time.unscaledDeltaTime);
                    colkeys[m_gradientColorAnimator.keyIndex].color = col;

                    color.SetKeys(colkeys, color.alphaKeys);

                }
            }
            else if (Time.realtimeSinceStartup - m_gradientColorAnimator.lastChangeTime >= m_gradientColorAnimator.stayDuration)
            {
                // transition starts
                m_gradientColorAnimator.nextCol = m_gradientColorAnimator.cols[Random.Range(0, m_gradientColorAnimator.cols.Length)];
                m_gradientColorAnimator.isChanging = true;
            }
        }

        private void ReScheduleEmission(Emission emission)
        {
            emission.startTime = Time.realtimeSinceStartup + Random.Range(restoreTimeRange.min, restoreTimeRange.max);
            emission.endTime = Time.realtimeSinceStartup + Random.Range(lifeTimeRange.min, lifeTimeRange.max);
            emission.moveSpeed = GetRandomNormalizedVector2() * Random.Range(moveSpeedRange.min, moveSpeedRange.max);
            emission.scaleSpeed = Random.Range(scaleSpeedRange.min, scaleSpeedRange.max);
            emission.rotateSpeed = Random.Range(rotationSpeedRange.min, rotationSpeedRange.max);

            // sprite change
            emission.image.sprite = GetRandomSprite();
            emission.image.SetNativeSize();
            emission.image.preserveAspect = true;
            emission.image.rectTransform.pivot =
                new Vector2(emission.image.sprite.pivot.x / emission.image.sprite.rect.width,
                             emission.image.sprite.pivot.y / emission.image.sprite.rect.height);

            // transformation
            emission.image.rectTransform.localPosition = GetRandomPosInBoundry();
            emission.image.rectTransform.localScale = Vector3.one * Random.Range(startScaleRange.min, startScaleRange.max);
            emission.image.rectTransform.localRotation = GetRandomRotation();

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
            emission.image.rectTransform.Rotate(Vector3.forward * emission.rotateSpeed * Time.unscaledDeltaTime * Mathf.Rad2Deg);
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
        public Quaternion GetRandomRotation() => Quaternion.Euler(Vector3.forward * (Random.Range(startRotationRange.min * Mathf.Rad2Deg, startRotationRange.max * Mathf.Rad2Deg)));

        void IOnCanvasDisabled.OnCanvasDisable() => this.enabled = false;
        void IOnCanvasEnabled.OnCanvasEnable() => this.enabled = true;
        private void OnDestroy()
        {
            for (int i = m_emissions.Length - 1; i >= 0; i--)
                Destroy(m_emissions[i].image.gameObject);
        }
    }
}