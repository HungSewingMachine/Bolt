using System;
using System.Collections;
using DG.Tweening;
using Main.Scripts.State;
using Main.Scripts.Utils;
using UnityEngine;

namespace Main.Scripts.Entity
{
    public enum EntityState
    {
        Stable,
        Moving,
        MoveDone,
        AtDestination,
    }

    public class Hexagon : MonoBehaviour
    {
        public EntityState state;
        //private bool canAddToBoxLine = false;
        private bool isMoveFromWaitLine;

        [SerializeField] private GameData gameData;

        // === Only Test change color
        public Renderer[] renderers;

        public Collider[] colliders;
        // === Only Test change color

        public Grid grid;

        public Grid     Coordinate   { get; private set; }
        public HexColor ElementColor { get; private set; }

        public float height  = 0.2f / 2 + 0.1f;
        public float radius  = 0.58f - 0.08f;
        public bool  canMove = false;

        private Box parentBox;
        private GameManager gameManager;

        private const float UNIT_ANGLE         = Mathf.PI / 3;
        private const float MAX_CHECK_DISTANCE = 0.2f;
        private const int   HEXAGON_LAYER      = 1 << 7;

        private static readonly float[] Angles = new[]
        {
            0f, UNIT_ANGLE, 2 * UNIT_ANGLE, 3 * UNIT_ANGLE, 4 * UNIT_ANGLE, 5 * UNIT_ANGLE,
        };

        // if using custom shader, replace it with appropriate name not _Color
        private static readonly int ColorField = Shader.PropertyToID("_Color");

        public void RegisterBox(Box box)
        {
            parentBox = box;
        }

        private Vector3 GetStartOriginPoint(Transform t)
        {
            return t.position;
        }

        private void TurnOffCollider()
        {
            foreach (var c in colliders)
            {
                c.enabled = false;
            }
        }

        public void CheckMovable()
        {
            // Debug.Log($"RedFlag check movable!");
            if (canMove) return;

            foreach (var angle in Angles)
            {
                var checkPoint = GetPositionFromAngle(transform, angle - Mathf.PI / 6);
                // Debug.Log($"RedFlag check point {checkPoint}!");
                if (Physics.Raycast(checkPoint, Vector3.up, 10, HEXAGON_LAYER))
                {
                    canMove = false;
                    LogCanNotMove();
                    return;
                }
            }

            // Turn on anim, ...
            canMove = true;
            // ChangeColor(Color.white);
        }

        private Vector3 GetPositionFromAngle(Transform t, float angle)
        {
            var origin = GetStartOriginPoint(t);
            return new Vector3(origin.x + radius * Mathf.Cos(angle), origin.y, origin.z + radius * Mathf.Sin(angle));
        }

        protected virtual void LogCanNotMove()
        {
            //Debug.Log($"RedFlag Detect False");
        }

        public void ChangeColor(HexColor color)
        {
            Debug.Log($"RedFlag color change! {color}");
            ElementColor = color;

            var proBlock = new MaterialPropertyBlock();
            proBlock.SetColor(ColorField, GetConvertColor(color));
            foreach (var r in renderers)
            {
                r.SetPropertyBlock(proBlock);
            }

            Color GetConvertColor(HexColor c)
            {
                return c switch
                {
                    HexColor.Blue  => Color.blue,
                    HexColor.Green => Color.green,
                    HexColor.Red   => Color.red,
                    HexColor.Pink  => Color.magenta,
                    _              => Color.white,
                };
            }
        }

        public void Initialize(Grid grid, GameManager manager)
        {
            this.grid  = grid;
            Coordinate = grid;
            gameManager = manager;
            state = EntityState.Stable;
        }

        /// <summary>
        /// Wait so that collider if turn off, then request GameManager a position and recalculate possible move.
        /// </summary>
        public void FindTargetThenMove(bool fromWaitLine = false)
        {
            if (!canMove) return;

            TurnOffCollider();
            this.DelayExecute(() => GetTargetPosition(fromWaitLine), 0.1f);
        }

        /// <summary>
        /// If hexagon is moving and call this function again, that mean it called from waitLine.
        /// </summary>
        public void GetTargetPosition(bool fromWaitLine = false)
        {
            if (state == EntityState.Moving)
            {
                //StartCoroutine(GetToBox(fromWaitLine));
                return;
            }

            var target = gameManager.RequestLanding(this, fromWaitLine);
            MoveTo(target);
        }

        public void MoveTo(Vector3 target)
        {
            state = EntityState.Moving;
            Debug.Log($"RedFlag {grid} move to {target}");

            const float duration = 1f;
            Move(target, duration, () =>
            {
                if (parentBox != null)
                {
                    transform.SetParentAndReset(parentBox.boxTransform);
                    parentBox.OnMoveToBoxComplete();
                }

                state = EntityState.MoveDone;
            });
        }

        /// <summary>
        /// Play move and rotate object to destination value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        /// <param name="onComplete"></param>
        private void Move(Vector3 target, float duration, Action onComplete = null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(target, duration));
            sequence.Join(transform.DOLocalRotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360));
            sequence.OnComplete(() => onComplete?.Invoke());

            sequence.Play();
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (canMove) return;
            Gizmos.color = Color.red;
            for (int i = 0; i < Angles.Length; i++)
            {
                var checkPoint = GetPositionFromAngle(transform, Angles[i] - Mathf.PI / 6);
                Gizmos.DrawSphere(checkPoint, 0.01f);
                Gizmos.DrawLine(checkPoint, checkPoint + Vector3.up * MAX_CHECK_DISTANCE);
            }
        }
#endif
    }
}