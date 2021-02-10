using System;
using GXPEngine;
using GXPEngine.OpenGL;

namespace GXPEngine.Core
{
    public class Collider
    {
        public Vector2 localPos;
        public Vector2 localBounds;
        public GameObject selfIdentity;
        public bool isTrigger = false;

        public Vector2[] extends
        {
            get
            {
                if (selfIdentity == null)
                    return new Vector2[4] { Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero };
                else
                    return new Vector2[4] {
                selfIdentity.TransformPoint(localPos.x, localPos.y) ,
                selfIdentity.TransformPoint(localPos.x + localBounds.x,localPos.y),
                selfIdentity.TransformPoint(localPos.x + localBounds.x,localPos.y + localBounds.y ),
                selfIdentity.TransformPoint(localPos.x, localPos.y + localBounds.y) };
            }
        }

        public void SetExtends(Vector2 pos, Vector2 offset)
        {
            localPos = pos;
            localBounds = offset;
        }

        public virtual void DrawSelf()
        {
            Vector2[] extends = this.extends;
            if (isTrigger) Gizmos.SetColor(0, 1, 0);
            else Gizmos.SetColor(1, 1, 1);
            Gizmos.DrawLine(extends[0], extends[1], width: 1);
            Gizmos.DrawLine(extends[1], extends[2], width: 1);
            Gizmos.DrawLine(extends[2], extends[3], width: 1);
            Gizmos.DrawLine(extends[3], extends[0], width: 1);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------		
        /// <summary>
        /// Returns <c>true</c> if this collider is currently overlapping with the collider other.
        /// </summary>
        public virtual bool HitTest(Collider other)
        {
            return false;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------		
        /// <summary>
        /// Returns <c>true</c> if this collider is currently overlapping with the point x,y.
        /// </summary>
        public virtual bool HitTestPoint(float x, float y, bool ignoreTrigger = true)
        {
            return false;
        }

        /// <summary>
        /// If this collider would collide with collider other after moving by vx,vy,
        /// then this method returns the time of impact of the collision, which is a number between 
        /// 0 (=immediate collision, or already overlapping) and 1 (=collision after moving exactly by vx,vy).
        /// Otherwise, a number larger than 1 (e.g. float.MaxValue) is returned.
        /// In addition, the collision normal is returned, in case of a collision.
        /// </summary>
        /// <returns>The time of impact.</returns>
        /// <param name="other">Another collider.</param>
        /// <param name="vx">x velocity or translation amount.</param>
        /// <param name="vy">y velocity or translation amount.</param>
        /// <param name="normal">The collision normal.</param>
        public virtual float TimeOfImpact(Collider other, float vx, float vy, out Vector2 normal)
        {
            normal = Vector2.Zero;
            return float.MaxValue;
        }

        /// <summary>
        /// If this collider and the collider other are overlapping, this method returns useful collision info such as
        /// the collision normal, the point of impact, and the penetration depth, 
        /// contained in a Collision object (the time of impact field will always be zero).
        /// 
        /// If they are not overlapping, this method returns null.
        /// </summary>
        public virtual Collision GetCollisionInfo(Collider other)
        {
            return null;
        }
    }
}

