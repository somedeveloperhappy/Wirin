using UnityEngine;

namespace FlatTheme.Shield
{
    public class ShieldFx : MonoBehaviour
    {
        public Gameplay.Player.Shield shield;


        bool shield_isUp = false;
        [HideInInspector] public float time;

        [System.Serializable]
        public struct LineSettings
        {
            public SpriteRenderer m_spriteRendererWithMaterial;

            [HideInInspector] public int param_mat_id;
            [HideInInspector] public int mainTex_mat_id;
            [HideInInspector] public Vector3 lastSize;
            [HideInInspector] public Vector2 mainTex_scale;

            public AnimationCurve onUpScaleCurve;
            public float scaleSpeedOnDown;
        }
        public LineSettings m_lineSettings;


        [System.Serializable]
        public struct CircleSettings
        {
            public SpriteRenderer spriteRenderer;

            [HideInInspector] public int tile_mat_id;
            [HideInInspector] public float tile;

            public AnimationCurve tileOnShieldUp;
            public float tileSpeedOnShieldDown;
        }
        public CircleSettings m_circleSettings;

        private void Awake()
        {
            m_lineSettings.m_spriteRendererWithMaterial ??= GetComponent<SpriteRenderer>();
            shield.onShieldUp += onShieldUp;
            shield.onShieldDown += onShieldDown;
        }
        private void OnDestroy()
        {
            shield.onShieldUp -= onShieldUp;
            shield.onShieldDown -= onShieldDown;
        }
        private void Start()
        {
            m_lineSettings.param_mat_id = Shader.PropertyToID("_Offset");
            m_lineSettings.mainTex_mat_id = Shader.PropertyToID("_MainTex");
            m_lineSettings.mainTex_scale = m_lineSettings.m_spriteRendererWithMaterial.sharedMaterial.GetTextureScale(m_lineSettings.mainTex_mat_id);

            m_circleSettings.tile_mat_id = Shader.PropertyToID("_Tile");
            m_circleSettings.tile = m_circleSettings.spriteRenderer.sharedMaterial.GetFloat(m_circleSettings.tile_mat_id);

        }
        private void OnEnable()
        {
            time = 0;
        }

        private void onShieldUp()
        {
            this.enabled = true;
            shield_isUp = true;
        }

        private void onShieldDown()
        {
            this.enabled = false;
            shield_isUp = false;
        }

        private void Update()
        {
            time += Time.deltaTime;

            #region line fx
            m_lineSettings.mainTex_scale.y = shield_isUp ?
                m_lineSettings.onUpScaleCurve.Evaluate(time) :
                m_lineSettings.mainTex_scale.y + m_lineSettings.scaleSpeedOnDown * Time.deltaTime;

            m_lineSettings.m_spriteRendererWithMaterial.sharedMaterial.SetTextureScale(
                m_lineSettings.mainTex_mat_id,
                m_lineSettings.mainTex_scale);
            #endregion

            #region circle fx
            m_circleSettings.tile =
                shield_isUp ?
                    m_circleSettings.tileOnShieldUp.Evaluate(time) :
                    m_circleSettings.tile + m_circleSettings.tileSpeedOnShieldDown * Time.deltaTime;
            m_circleSettings.spriteRenderer.sharedMaterial.SetFloat(m_circleSettings.tile_mat_id, m_circleSettings.tile);

            #endregion
        }

    }
}