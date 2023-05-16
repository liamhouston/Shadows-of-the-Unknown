using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameFeel
{
    public class VisualPlatformerScriptV2 : BasicPlatformerScriptV2
    {
        [Header("Landing Deformation")]
        [SerializeField] private Vector2 jumpDeformScale;
        [SerializeField] private AnimationCurve jumpDeformCurve;
        
        [Header("Landing Deformation")]
        [SerializeField] private float landingDeformTime;
        [SerializeField] private Vector2 landingDeformScale;
        [SerializeField] private AnimationCurve landingDeformCurve;

        [Header("Camera Triggers"),Tooltip("You'll have to replay to see these changes in effect")] 
        [SerializeField] private CameraControl cameraControl;
        [SerializeField] private bool zoomOnImpact;
        [SerializeField] private bool zoomOnJump;
        [SerializeField] private bool shakeOnImpact;
        [SerializeField] private bool shakeOnJump;
        private readonly  UnityEvent _impactEvent = new UnityEvent();
        private readonly  UnityEvent _jumpEvent = new UnityEvent();
        
        private Vector3 _originalSpritePosition;
        private Vector3 _originalSpriteScale;
        
        private GameObject _spriteObject;
        private Transform _spriteTransform;
        private Coroutine _landingDeformRoutine;
        private Coroutine _jumpDeformRoutine;
        protected override void Start() {
            base.Start();
            
            //Cache sprite data
            _spriteObject = transform.GetChild(0).gameObject;
            _spriteTransform = _spriteObject.transform;
            _originalSpritePosition = _spriteTransform.localPosition;
            _originalSpriteScale = _spriteTransform.localScale;
            
            //Subscribe Event
            if (zoomOnImpact) _impactEvent.AddListener(cameraControl.Zoom);
            if (shakeOnImpact) _impactEvent.AddListener(cameraControl.Shake);
            if (zoomOnJump) _jumpEvent.AddListener(cameraControl.Zoom);
            if (shakeOnJump) _jumpEvent.AddListener(cameraControl.Shake);
        }

        protected override void OnGrounded_Hook()
        {
            if (_jumpDeformRoutine != null)
            {
                StopCoroutine(_jumpDeformRoutine);
                _jumpDeformRoutine = null;
                _spriteTransform.localScale = _originalSpriteScale;
                _spriteTransform.localPosition = _originalSpritePosition;
            }
            _landingDeformRoutine = StartCoroutine(_Deform(landingDeformScale,landingDeformCurve,jumpTime));
            _impactEvent.Invoke();
        }
        protected override void OnFalling_Hook(){}

        protected override void OnJump_Hook()
        {
            if (_landingDeformRoutine != null)
            {
                StopCoroutine(_landingDeformRoutine);
                _landingDeformRoutine = null;
                _spriteTransform.localScale = _originalSpriteScale;
                _spriteTransform.localPosition = _originalSpritePosition;
            }
            else if (_jumpDeformRoutine != null)
            {
                StopCoroutine(_jumpDeformRoutine);
                _jumpDeformRoutine = null;
                _spriteTransform.localScale = _originalSpriteScale;
                _spriteTransform.localPosition = _originalSpritePosition;
            }
            _jumpDeformRoutine = StartCoroutine(_Deform(jumpDeformScale,jumpDeformCurve,jumpTime));
            _jumpEvent.Invoke();
        }
        
        private IEnumerator _Deform(Vector2 deformScale, AnimationCurve curve,float maxTime)
        {
            float timer = 0;
            Vector2 extants = _playerCollider.bounds.extents;
            while (timer <= maxTime)
            {
                //Calculating deformed scale
                float pingPongVal = Mathf.PingPong(timer, (maxTime/2f))/(maxTime/2f);
                float curveValue = curve.Evaluate(pingPongVal);
                Vector2 newScale = new Vector2(
                    Mathf.Lerp(_originalSpriteScale.x, _originalSpriteScale.x * deformScale.x, curveValue),
                    Mathf.Lerp(_originalSpriteScale.y, _originalSpriteScale.y * deformScale.y, curveValue));
                _spriteTransform.localScale = newScale;
                
                //calculate vertical adjustment so that sprite base is the same as collider base?
                timer += Time.deltaTime;
                yield return null;
            }
            _spriteTransform.localScale = _originalSpriteScale;
            _spriteTransform.localPosition = _originalSpritePosition;
        }
    }
    
}
