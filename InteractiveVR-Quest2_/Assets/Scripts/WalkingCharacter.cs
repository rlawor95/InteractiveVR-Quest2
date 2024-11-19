using UnityEngine;
using System.Collections;
using UnityEngine.VFX;
using Photon.Pun;

public class WalkingCharacter : MonoBehaviourPun
{
    public Transform pointA; // A 좌표
    public Transform pointB; // B 좌표
    public float speed = 2.0f; // 이동 속도
    public Animator animator; // Animator 컴포넌트
    
    private bool movingToB = true; // 현재 이동 방향
    private float waitTime = 10.0f; // 대기 시간
    
    public VisualEffect vfxGraph;
    public VisualEffect windGraph;

    private Vector3 targetPoint;
    
    private void Start()
    {
       // if (photonView.IsMine)
            StartCoroutine(MoveBetweenPoints());
    }

    private void Update()
    {
        if (vfxGraph == null)
            return;
        
        vfxGraph.SetVector3("WavePosition", this.transform.position);
        windGraph.SetVector3("SensingPosition", this.transform.position);
    }
    
    [PunRPC]
    void UpdatePositionRPC(Vector3 newPosition, Vector3 _targetPoint)
    {
        // 다른 클라이언트에서 위치 업데이트
        Debug.Log("UpdatePositionRPC  " + newPosition + "   ,   " + _targetPoint);
        transform.position = newPosition;
        this.targetPoint = _targetPoint;
    }
    

    private IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            targetPoint = movingToB ? pointB.position : pointA.position; // 목표 위치
            animator.SetBool("Idle", false); // Walk 애니메이션 활성화

            // 목표 위치로 이동
            while (Vector3.Distance(transform.position, targetPoint) > 0.1f)
            {
                // 방향을 목표 위치로 맞추기
                Vector3 direction = (targetPoint - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

                // 위치 이동
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

                yield return null; // 다음 프레임까지 대기
            }

            RPCCall();
            
            
            // 목표 위치에 도착하면 Idle 애니메이션 재생
            animator.SetBool("Idle", true); // Walk 애니메이션 비활성화
            yield return new WaitForSeconds(waitTime); // 대기 시간

            // 다음 목표 위치 설정
            movingToB = !movingToB;
        }
    }

    void RPCCall()
    {
        Debug.Log("RPC CAll");
        photonView.RPC("UpdatePositionRPC", RpcTarget.Others, transform.position, targetPoint);
    }
}
