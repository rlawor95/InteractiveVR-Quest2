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

    private void Start()
    {

        if (photonView.IsMine)
            StartCoroutine(MoveBetweenPoints());
    }

    private void Update()
    {
        if (vfxGraph == null)
            return;
        
        vfxGraph.SetVector3("WavePosition", this.transform.position);
        
    }
    
    [PunRPC]
    void UpdatePosition(Vector3 newPosition)
    {
        // 다른 클라이언트에서 위치 업데이트
        //transform.position = newPosition;
    }
    

    private IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            Transform targetPoint = movingToB ? pointB : pointA; // 목표 위치
            animator.SetBool("Idle", false); // Walk 애니메이션 활성화

            // 목표 위치로 이동
            while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                // 방향을 목표 위치로 맞추기
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);

                // 위치 이동
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

                yield return null; // 다음 프레임까지 대기
            }

            // 목표 위치에 도착하면 Idle 애니메이션 재생
            animator.SetBool("Idle", true); // Walk 애니메이션 비활성화
            yield return new WaitForSeconds(waitTime); // 대기 시간

            // 다음 목표 위치 설정
            movingToB = !movingToB;
        }
    }
}
