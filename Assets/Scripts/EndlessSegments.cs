using UnityEngine;

namespace DefaultNamespace
{
    using System.Collections.Generic;
    using UnityEngine;
    
    public class EndlessSegments : MonoBehaviour
    {
        [Header("Segments")]
        public List<Transform> segments;
    
        [Header("Bounds")]
        public float resetZ = -20f; // когда сегмент ушел за эту точку — переносим
        public float segmentLength = 10f; // длина одного сегмента
    
        void Update()
        {
            MoveSegments();
            RecycleSegments();
        }
    
        void MoveSegments()
        {
            foreach (var seg in segments)
            {
                seg.Translate(Vector3.back * Game.I.speed * Time.deltaTime);
            }
        }
    
        void RecycleSegments()
        {
            foreach (var seg in segments)
            {
                if (seg.position.z < resetZ)
                {
                    Transform farthest = GetFarthestSegment();
                    float newZ = farthest.position.z + segmentLength;
    
                    seg.position = new Vector3(seg.position.x, seg.position.y, newZ);
                }
            }
        }
    
        Transform GetFarthestSegment()
        {
            Transform farthest = segments[0];
            float maxZ = farthest.position.z;
    
            foreach (var seg in segments)
            {
                if (seg.position.z > maxZ)
                {
                    maxZ = seg.position.z;
                    farthest = seg;
                }
            }
    
            return farthest;
        }
    }
}