using System.Collections.Generic;
using UnityEngine;


namespace PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;
        [SerializeField]
        private Camera cam;
        private Vector3 viewPos;
        [SerializeField]
        private GameObject[] body = new GameObject[2];
        private List<Transform> snakeBody;
        private int select = 0;
        private float deltaX;
        private float deltaY;
        private Going going;
        private Animator animator;
        

        private float Xdirection = 1f;
        private float Ydirection = 0f;

        private float minX = 0.05f;
        private float maxX = 0.82f;
        private float minY = 0.10f;
        private float maxY = 0.83f;

        private void Start()
        {
            animator = GetComponent<Animator>();
            snakeBody = new List<Transform>();
            snakeBody.Add(transform);
            snakeBody.Add(body[0].transform);
            snakeBody.Add(body[1].transform);
        }
        void Update()
        {
            //player movements
            if(going == Going.Up || going == Going.Bottom)
            {
                deltaX = Input.GetAxisRaw("Horizontal");
                Debug.Log("deltaX :" + deltaX);
            }  
            else
            {
                Debug.Log("vertical");
                deltaY = Input.GetAxisRaw("Vertical");
            }
            //deltaX = Input.GetAxisRaw("Horizontal");
            //deltaY = Input.GetAxisRaw("Vertical");

            if (deltaX > 0)
            {
                Xdirection = 1;
                Ydirection = 0;
                going = Going.Right;
            }
            else if(deltaX < 0)
            {
                Xdirection = -1;
                Ydirection = 0;
                going = Going.Left;
            }
            if(deltaY > 0)
            {
                Ydirection = 1;
                Xdirection = 0;
                going = Going.Up;
            }
            else if(deltaY < 0)
            {
                Ydirection = -1;
                Xdirection = 0;
                going = Going.Bottom;
            }

            Debug.Log("Going: " + going);
            animator.SetInteger("Direction", (int)going);

            //warping
            viewPos = cam.WorldToViewportPoint(transform.position);
            //Debug.Log("viewPos x:" + viewPos.x);
            //Debug.Log("vewPos y: " + viewPos.y);
            if(viewPos.x < minX)
            {
                transform.position = cam.ViewportToWorldPoint(new Vector3(maxX, viewPos.y, viewPos.z));
            }
            if(viewPos.y < minY)
            {
                transform.position = cam.ViewportToWorldPoint(new Vector3(viewPos.x, maxY, viewPos.z));
            }
            if(viewPos.x > maxX)
            {
                transform.position = cam.ViewportToWorldPoint(new Vector3(minX+0.01f, viewPos.y, viewPos.z));
            }
            if(viewPos.y > maxY)
            {
                transform.position = cam.ViewportToWorldPoint(new Vector3(viewPos.x, minY+0.01f, viewPos.z));
            }
            
        }
        private void FixedUpdate()
        {
            //classic snake like body movement.
            select = snakeBody.Count - 1; ;
            while (select >=1)
            {
                snakeBody[select].position = snakeBody[select - 1].position;
                select--;
            }
            body[select].transform.position = transform.position;

            float newXPos = transform.position.x + Xdirection * speed * Time.deltaTime;
            float newYPos = transform.position.y + Ydirection * speed * Time.deltaTime;

            transform.position = new Vector2(newXPos, newYPos);
        }

        private void bodyGrow()
        {
            Transform bodySegment = Instantiate(body[1].transform);
            bodySegment.position = snakeBody[snakeBody.Count - 1].position;
            snakeBody.Add(bodySegment);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Food")
            {
                bodyGrow();
                collision.gameObject.transform.position = Spawner.randomizer();
            }
        }
    }
}
