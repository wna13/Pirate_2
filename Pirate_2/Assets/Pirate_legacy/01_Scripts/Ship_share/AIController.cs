using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] float detectionRadius = 50f; // AI가 다른 배를 감지할 수 있는 반경
    [SerializeField] float distanceToMyTarget = 50f; // Target이 멀어지면 새로 배를 찾기 위한 거리
    [SerializeField] float attackDistance = 20f;  // AI가 공격을 시작할 거리
    [SerializeField] float minDistanceToChangeDirection = 5f;  // 방향을 변경할 최소 거리
    [SerializeField] float rotationSmoothing = 0.1f;  // 방향 전환의 부드러움
    [SerializeField] private float maxDistanceFromPlayer = 65f; // 플레이어로부터의 최대 거리
    [SerializeField] private float directionChangeInterval = 2f; // 방향을 바꾸는 시간 간격
    [SerializeField] private float turnDuration = 0.5f; // 회전이 완료되는데 걸리는 시간

    ShipMover shipMover;
    Transform targetShip;
    Transform playerTransform;
    Vector3 smoothedDirection;
    bool hasEnemy;
    private float directionChangeTimer = 0f;
    private Quaternion targetRotation;
    private bool isTurning = false;
    private float turnTimer = 0f;

    private void Start()
    {
        shipMover = GetComponent<ShipMover>();
        shipMover.isPlayer = false;
        smoothedDirection = transform.forward;
        SetRandomValue();
        targetShip = null;
        targetRotation = transform.rotation; // 초기 회전값 설정
    }

    void SetRandomValue()
    {
        // 필요한 랜덤 값 설정
    }

    private void Update()
    {
        if (!shipMover.isLive) return;
        if ( GameFlowManager.Instance != null )
        { 
            if (!GameFlowManager.Instance.isGameStart) return;
        }

        if ( playerTransform == null )
        {
            if ( PlayerMover.Instance != null ) playerTransform = PlayerMover.Instance.transform; // 플레이어의 Transform 가져오기
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > maxDistanceFromPlayer)
            {
                // 플레이어로부터 너무 멀면 비활성화 및 ShipManager에서 제거
                ShipManager.Instance.RemoveShip(transform);
                gameObject.SetActive(false);
                return;
            }
        }

        if (targetShip != null)
        {
            float _distanceToTarget = Vector3.Distance(transform.position, targetShip.position);

            // 일정 거리 내에서는 방향을 조정하지 않음
            if (_distanceToTarget > minDistanceToChangeDirection)
            {
                // 타겟 배를 향해 이동
                Vector3 direction = (targetShip.position - transform.position).normalized;

                // 방향 변경 및 회피 로직
                PerformAvoidanceMovement();

                if (direction != Vector3.zero)
                {
                    smoothedDirection = Vector3.Lerp(smoothedDirection, direction, rotationSmoothing);
                    shipMover.PlayerMove(new Vector3(smoothedDirection.x, 0, smoothedDirection.z));
                }
            }

            // 공격 거리 내에 있으면 공격
            if (_distanceToTarget <= attackDistance)
            {
                Attack();
            }

            // 타겟이 멀어진 경우 새로운 타겟을 찾음
            if (_distanceToTarget > distanceToMyTarget)
            {
                targetShip = null;
            }
        }
        else
        {
            // 가장 가까운 배를 찾음
            FindClosestShip();
            shipMover.PlayerStop();
        }

        // 회전이 진행 중이면 부드럽게 회전
        if (isTurning)
        {
            turnTimer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnTimer / turnDuration);

            if (turnTimer >= turnDuration)
            {
                isTurning = false;
                turnTimer = 0f;
            }
        }
    }

    void PerformAvoidanceMovement()
    {
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= directionChangeInterval)
        {
            directionChangeTimer = 0f;

            // 무작위 회전 각도 생성 (왼쪽 또는 오른쪽으로 회전)
            float randomAngle = Random.Range(-45f, 45f);
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomAngle, 0);

            // 회전 시작
            isTurning = true;
            turnTimer = 0f;
        }
    }

    void FindClosestShip()
    {
        targetShip = PlayerMover.Instance.transform;
        return;

        float closestDistance = Mathf.Infinity;
        Transform closestShip = null;

        foreach (Transform ship in ShipManager.Instance.ships)
        {
            if (ship == transform) continue; // 자기 자신은 제외

            float distance = Vector3.Distance(transform.position, ship.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestShip = ship;
            }
        }

        if (closestDistance <= detectionRadius)
        {
            targetShip = closestShip;
        }
        else
        {
            targetShip = null;
        }
    }

    void Attack()
    {
        // 공격 로직 구현 (캐논 발사 등)
        // Debug.Log("Attack the target ship!");
    }
}
