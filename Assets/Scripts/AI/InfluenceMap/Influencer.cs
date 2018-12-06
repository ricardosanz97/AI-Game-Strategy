using UnityEngine;

namespace InfluenceMap
{
    public class Influencer : MonoBehaviour
    {
        public InfluenceMap.Originator Originator;

        private void Update()
        {
            Originator.WorldPosition = transform.position;
        }
    }
}