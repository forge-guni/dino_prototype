using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class DinoController : MonoBehaviour
    {
        private bool _obstacleCollision;
        public bool obstacleCollision
        {
            get => _obstacleCollision;
            set
            {
                _obstacleCollision = value;
                DinoIsDead?.Invoke();
            }
        }

        private int _score;
        public int score
        {
            get => _score;
            set
            {
                _score = value;
                ScoreUpdated?.Invoke(_score);
            }
        }

        public event Action DinoIsDead;
        public event Action<int> ScoreUpdated;

        public void StartDinoing()
        {
            obstacleCollision = false;
            StartCoroutine(DinoLoop());
        }

        private IEnumerator DinoLoop()
        {
            while (!_obstacleCollision)
            {
                transform.Translate(Vector3.right * Time.deltaTime);
                if (Input.GetKey(KeyCode.Space))
                {
                    Debug.Log("Jump");
                    //jump
                }
                
                yield return null;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Obstacle>())
            {
                obstacleCollision = true;
            }
        }
    }
}