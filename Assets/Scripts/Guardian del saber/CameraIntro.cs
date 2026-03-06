    using UnityEngine;

    public class CameraIntro : MonoBehaviour
    {
        public Transform[] points;
        public CameraFollow cameraFollow;

        public Transform player;
        public Animator animator;

        public float speed = 2.0f;
        public float waitTime = 1.0f;

        int index = 0;
        float t = 0f;
        bool waiting = false;
        bool spinDone = false;
        float spinTimer = 0f;
        float spinDuration = 2f; // duración de 2 loops aprox
        bool spinningNow = false;

        void Start()
        {
            transform.position = points[0].position;
            transform.rotation = points[0].rotation;

            cameraFollow.enabled = false;

            animator.SetBool("greeting", true);
        }

        void Update()
        {
            Recorrido();
            if (spinningNow)
            {
                spinTimer += Time.deltaTime;

                if (spinTimer >= spinDuration)
                {
                    animator.SetBool("spinning", false);
                    spinningNow = false;
                }
            }
        }

        void Recorrido()
        {
            if (index >= points.Length - 1 || waiting) return;

            t += Time.deltaTime * speed;
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(points[index].position, points[index + 1].position, smooth);

            transform.rotation = Quaternion.Slerp(points[index].rotation, points[index + 1].rotation, smooth);

            if (t >= 1f)
            {
                index++;
                t = 0f;
                waiting = true;

                // 🔥 Cuando llega al penúltimo punto
                if (index == points.Length - 2 && !spinDone)
                {
                    SpinCharacter();
                }

                Invoke(nameof(Next), waitTime);
            }
        }

        void SpinCharacter()
        {
            spinDone = true;
            spinningNow = true;

            animator.SetBool("greeting", false);
            animator.SetBool("spinning", true);
            SpinCharacter2();
        }
        void SpinCharacter2()
        {
            animator.SetBool("spinning", false);
            animator.SetBool("spinning2", true);
        }


        void Next()
        {
            waiting = false;

            if (index >= points.Length - 1)
            {
                EndIntro();
            }
        }

        void EndIntro()
        {
            animator.SetBool("spinning2", false);
            animator.SetBool("idle", true);

            cameraFollow.enabled = true;
            this.enabled = false;
        }
    }