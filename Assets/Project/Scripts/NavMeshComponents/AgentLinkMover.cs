using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.NavMeshComponents
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AgentLinkMover : MonoBehaviour
    {
        [SerializeField] private OffMeshLinkMoveMethod _method = OffMeshLinkMoveMethod.Parabola;
        [SerializeField] private AnimationCurve _curve = new AnimationCurve();

        IEnumerator Start()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.autoTraverseOffMeshLink = false;
            while (true)
            {
                if (agent.isOnOffMeshLink)
                {
                    if (_method == OffMeshLinkMoveMethod.NormalSpeed)
                        yield return StartCoroutine(NormalSpeed(agent));
                    else if (_method == OffMeshLinkMoveMethod.Parabola)
                        yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                    else if (_method == OffMeshLinkMoveMethod.Curve)
                        yield return StartCoroutine(Curve(agent, 0.5f));
                    agent.CompleteOffMeshLink();
                }
                yield return null;
            }
        }

        IEnumerator NormalSpeed(NavMeshAgent agent)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            while (agent.transform.position != endPos)
            {
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                yield return null;
            }
        }

        IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }

        IEnumerator Curve(NavMeshAgent agent, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                float yOffset = _curve.Evaluate(normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }
    }
}
