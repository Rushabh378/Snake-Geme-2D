using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;
        [SerializeField]
        private Camera cam;
        private Vector3 viewPos;
        private int select = 0;
        private Going going;
        private Animator animator;
        

        private float Xdirection = 0f;
        private float Ydirection = 1f;
        private Vector2 initPos = new Vector2(18f, 11f);

        private float minX = 0.05f;
        private float maxX = 0.82f;
        private float minY = 0.10f;
        private float maxY = 0.83f;

        [SerializeField]
        private TextMeshProUGUI scoreText;
        private int score;
        [SerializeField]
        private GameObject gameover;

        [SerializeField]
        private Spawner spawner;

        private bool shield;
        private int doubler = 1;
        [SerializeField]
        private GameObject doublerObject;
        [SerializeField]
        private GameObject shieldObject;

        [SerializeField]
        private AudioClip[] clips;
        private AudioSource eats;

        [SerializeField]
        private GameObject[] body = new GameObject[2];
        private List<Transform> snakeBody;

        private SpriteRenderer myRenderer;

        

        private void Start()
        {
            animator = GetComponent<Animator>();
            eats = GetComponent<AudioSource>();
            snakeBody = new List<Transform>();
            
            Initialization();
        }
        void Update()
        {
            //player movements
            if (Input.GetKeyDown(KeyCode.D) && going != Going.Left)
            {
                Xdirection = 1;
                Ydirection = 0;
                going = Going.Right;
            }
            else if(Input.GetKeyDown(KeyCode.A) && going != Going.Right)
            {
                Xdirection = -1;
                Ydirection = 0;
                going = Going.Left;
            }
            if(Input.GetKeyDown(KeyCode.W) && going != Going.Bottom)
            {
                Ydirection = 1;
                Xdirection = 0;
                going = Going.Up;
            }
            else if(Input.GetKeyDown(KeyCode.S) && going != Going.Up)
            {
                Ydirection = -1;
                Xdirection = 0;
                going = Going.Bottom;
            }
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Food")
            {
                eatSound(clips[0]);
                bodyGrow();
                increasScore(50);
                collision.gameObject.transform.position = spawner.Randomizer();
            }
            else if (collision.tag == "Body")
            {
                if (!shield)
                {
                    eatSound(clips[3]);
                    gameover.SetActive(true);
                    doublerObject.SetActive(false);
                    shieldObject.SetActive(false);
                    
                    Time.timeScale = 0;
                }
                
            }
            else if (collision.tag == "poisen")
            {
                eatSound(clips[1]);
                bodyUngrow();
                decreasScore(50);
                TimerManagement.cancelTimer("poisenTimer");
                TimerManagement.setTimer(spawner.RandomizePoisen, 4f,"poisenTimer");
                Destroy(collision.gameObject);
            }
            else if(collision.tag == "Shield")
            {
                eatSound(clips[2]);
                shield = true;
                shieldObject.SetActive(true);
                for(int i = 2; i < snakeBody.Count; i++)
                {
                    myRenderer = snakeBody[i].gameObject.GetComponent<SpriteRenderer>();
                    myRenderer.color = Random.ColorHSV();
                }
                TimerManagement.cancelTimer("shieldTimer");
                TimerManagement.setTimer(spawner.RandomizeShield, 10f, "shieldTimer");
                Destroy(collision.gameObject);
                TimerManagement.setTimer(shieldOff, 5f);
            }
            else if(collision.tag == "Doubler")
            {
                eatSound(clips[1]);
                doubler = 2;
                doublerObject.SetActive(true);
                TimerManagement.cancelTimer("doublerTimer");
                TimerManagement.setTimer(spawner.RandomizeDoubler, 15f, "doublerTimer");
                Destroy(collision.gameObject);
                TimerManagement.setTimer(doublerOff, 6f);
            }
        }
        public void Initialization()
        {
            for(int i = 3; i < snakeBody.Count; i++)
            {
                Destroy(snakeBody[i].gameObject);
            }
            if (Time.timeScale == 0)
                Time.timeScale = 1;
            if(gameover.activeSelf == true)
                gameover.SetActive(false);
            snakeBody.Clear();
            snakeBody.Add(transform);
            snakeBody.Add(body[0].transform);
            snakeBody.Add(body[1].transform);
            transform.position = initPos;
            going = Going.Up;
            Ydirection = 1f;
            Xdirection = 0f;
            score = 0;
            scoreText.text = "Score:" + score.ToString();
            shield = false;
            doublerObject.SetActive(false);
            shieldObject.SetActive(false);
        }
        private void bodyGrow()
        {
            Transform bodySegment = Instantiate(body[1].transform);
            bodySegment.position = snakeBody[snakeBody.Count - 1].position;
            snakeBody.Add(bodySegment);
        }
        private void bodyUngrow()
        {
            if(snakeBody.Count > 3)
            {
                Destroy(snakeBody[snakeBody.Count - 1].gameObject);
                snakeBody.RemoveAt(snakeBody.Count - 1);
            }
        }
        private void increasScore(int increment)
        {
            increment *= doubler;
            score += increment;
            scoreText.text = "Score:" + score.ToString();
        }
        private void decreasScore(int decrement)
        {
            score -= decrement;
            scoreText.text = "Score:" + score.ToString();
        }
        public void shieldOff()
        {
            shield = false;
            shieldObject.SetActive(false);
            for (int i = 2; i < snakeBody.Count; i++)
            {
                myRenderer = snakeBody[i].gameObject.GetComponent<SpriteRenderer>();
                myRenderer.color = new Color(0.5f,0.9f,0.6f,1f);
                
            }
        }
        public void doublerOff()
        {
            doubler = 1;
            doublerObject.SetActive(false);
        }
        public void eatSound(AudioClip clip)
        {
            eats.PlayOneShot(clip);
        }
    }
}
