using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine.AI;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath _patrolPath;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] float waypointDwellTime = 2f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        private Fighter _fighter;
        private GameObject _player;
        private Health _health;
        private Vector3 _guardPosition;
        private Mover _mover;

        private float _timeSinceWeSawThePlayer = Mathf.Infinity;
        private float _timeSinceWeArrivedWaypoint = Mathf.Infinity;

        int currentWayPoint = 0;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _guardPosition = transform.position;
            _mover = GetComponent<Mover>();
        }
        private void Update()
        {
            if (_health.isDead()) return;

            if (InChasingRange() && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            }
            else if (_timeSinceWeSawThePlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceWeSawThePlayer += Time.deltaTime;
            _timeSinceWeArrivedWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition;
            if (_patrolPath != null)
            {
                if (AtWayPoint())
                {
                    _timeSinceWeArrivedWaypoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }
            if (_timeSinceWeArrivedWaypoint > waypointDwellTime)
                _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }
        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWaypoint < waypointTolerance;

        }

        private void CycleWayPoint()
        {
            currentWayPoint = _patrolPath.GetNextIndex(currentWayPoint);
        }

        private Vector3 GetCurrentWayPoint()
        {
            return _patrolPath.GetWaypoint(currentWayPoint);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CurrentActionDisable();
        }

        private bool InChasingRange()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return distanceToPlayer < chaseDistance;
        }
        private void AttackBehaviour()
        {
            _timeSinceWeSawThePlayer = 0;
            _fighter.Attack(_player.gameObject);
        }

        //unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

