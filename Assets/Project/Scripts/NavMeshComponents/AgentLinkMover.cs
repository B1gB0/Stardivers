using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.NavMeshComponents
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentLinkMover : MonoBehaviour
    {
        private const float JumpFactor = 4f;
        private const float Duration = 0.5f;
        private const float Height = 2f;
        private const float NormalizedTimeMinValue = 0f;
        private const float NormalizedTimeMaxValue = 1f;
        
        [SerializeField] private OffMeshLinkMoveMethod _method = OffMeshLinkMoveMethod.Parabola;
        [SerializeField] private AnimationCurve _curve = new AnimationCurve();

        private NavMeshAgent _agent;
        private Coroutine _startBehaviour;
        private Coroutine _changeBehaviour;

        private void OnEnable()
        {
            if(_changeBehaviour != null)
                StopCoroutine(_changeBehaviour);

            _changeBehaviour = StartCoroutine(ChangeBehaviour());
        }

        private void OnDisable()
        {
            if(_changeBehaviour != null)
                StopCoroutine(_changeBehaviour);
            
            if(_startBehaviour != null)
                StopCoroutine(_startBehaviour);
        }

        private IEnumerator ChangeBehaviour()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.Resume();
            _agent.autoTraverseOffMeshLink = false;

            while (gameObject.activeSelf)
            {
                if (_agent.isOnOffMeshLink)
                {
                    if (_method == OffMeshLinkMoveMethod.NormalSpeed)
                        yield return _startBehaviour = StartCoroutine(NormalSpeed(_agent));
                    else if (_method == OffMeshLinkMoveMethod.Parabola)
                        yield return _startBehaviour = StartCoroutine(Parabola(_agent, Height, Duration));
                    else if (_method == OffMeshLinkMoveMethod.Curve)
                        yield return _startBehaviour = StartCoroutine(Curve(_agent, Duration));
                    
                    _agent.CompleteOffMeshLink();
                }

                yield return null;
            }
        }

        private IEnumerator NormalSpeed(NavMeshAgent agent)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            
            while (agent.transform.position != endPos)
            {
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            
            float normalizedTime = NormalizedTimeMinValue;
            
            while (normalizedTime < NormalizedTimeMaxValue)
            {
                float yOffset = height * JumpFactor * (normalizedTime - normalizedTime * normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }

        private IEnumerator Curve(NavMeshAgent agent, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            
            float normalizedTime = NormalizedTimeMinValue;
            
            while (normalizedTime < NormalizedTimeMaxValue)
            {
                float yOffset = _curve.Evaluate(normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }
    }
}
