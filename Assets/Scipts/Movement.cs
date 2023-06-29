using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    CustomInputSystem inputSystem;
    Vector2 movementInput;
    float speed = 1f;
    float jumpForce = 3f;
    bool doublejumped = false;
    bool running = false;
    bool rolling = false;
    float timeStampRun = -1;
    float timeStampRoll = -1;

    float range = 1f;

    public GameObject door;


    public GameObject lightColorSwitch;
    public GameObject directionalLight;
    int index = 0;

    public GameObject key;
    bool hasKey = false;

    public GameObject flowers;
    public GameObject grass;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //grass.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();

        inputSystem = new CustomInputSystem();
        inputSystem.Enable();
        inputSystem.Player.Move.performed += moving =>
        {
            movementInput = moving.ReadValue<Vector2>();
        };
        inputSystem.Player.Move.canceled += moving =>
        {
            movementInput = new(0, 0);
        };

    }

    // Update is called once per frame
    void Update()
    {
        if (!rolling)
            Move();
        if (inputSystem.Player.Jump.triggered)
        {
            Jump();
        }
        if (inputSystem.Player.Run.triggered)
        {
            Run();
        }
        if (inputSystem.Player.Roll.triggered)
        {
            Roll();
        }
        if (inputSystem.Player.Interact.triggered)
        {
            Interact();
        }
        if (rolling && timeStampRoll - 3f <= Time.time) { rolling = false; }
        if (running && timeStampRun <= Time.time)
        {
            speed = 1f;
            running = false;
            Debug.Log("Running expired");
        }
        if (Vector3.Distance(transform.position, key.transform.position) < range + 1)
        {
            hasKey = true;
            key.SetActive(false);
        }

    }

    void Move()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        if (movementInput.x > 0)
        {
            rb.AddForce(speed * Vector3.right, ForceMode.Impulse);
            animator.Play("WalkRight");
        }
        else if (movementInput.x < 0)
        {
            rb.AddForce(-speed * Vector3.right, ForceMode.Impulse);
            animator.Play("WalkLeft");

        }
        else if (movementInput.y > 0)
        {
            rb.AddForce(speed * Vector3.forward, ForceMode.Impulse);
            animator.Play("WalkForward");

        }
        else if (movementInput.y < 0)
        {
            rb.AddForce(-speed * Vector3.forward, ForceMode.Impulse);
            animator.Play("WalkBackward");

        }

    }

    private void Jump()
    {
        Debug.Log(transform.position.y);
        Debug.Log(transform.position.y <= 0.11f);
        if (transform.position.y <= 0.11f)
        {
            rb.velocity = Vector3.zero;
            if (animator != null)
            {
                animator.SetTrigger("JumpTrigger");
            }
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            doublejumped = false;

        }
        else if (!doublejumped)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            doublejumped = true;

        }
    }

    private void Run()
    {
        if (timeStampRun == -1 || timeStampRun <= Time.time)
        {
            speed = 3f;
            timeStampRun = Time.time + 5;
            running = true;
            Debug.Log("Running Active");
        }
        else
        {
            Debug.Log("Running Unactive");
        }
    }

    private void Roll()
    {
        if (timeStampRoll == -1 || timeStampRoll <= Time.time && (movementInput.x != 0 || movementInput.y != 0))
        {

            rolling = true;
            if (movementInput.x > 0)
            {
                rb.AddForce(speed * 5f * Vector3.right, ForceMode.Impulse);
                animator.Play("RollRight");
            }
            else if (movementInput.x < 0)
            {
                rb.AddForce(-speed * 5f * Vector3.right, ForceMode.Impulse);
                animator.Play("RollLeft");

            }
            else if (movementInput.y > 0)
            {
                rb.AddForce(speed * 5f * Vector3.forward, ForceMode.Impulse);
                animator.Play("RollForward");

            }
            else if (movementInput.y < 0)
            {
                rb.AddForce(-speed * 5f * Vector3.forward, ForceMode.Impulse);
                animator.Play("RollBackward");

            }

            timeStampRoll = Time.time + 5f;
            Debug.Log("Rolling Active");
        }
        else
        {
            Debug.Log("Rolling Unactive");
        }
    }

    private void Interact()
    {
        if (hasKey && Vector3.Distance(transform.position, door.transform.position) < range)
        {
            StartCoroutine(OpenDoor());
        }
        else if (Vector3.Distance(transform.position, lightColorSwitch.transform.position) < range + 2)
        {
            Light light = directionalLight.GetComponent<Light>();
            switch (index % 5)
            {
                case 0:
                    light.color = Color.green; break;
                case 1:
                    light.color = Color.yellow; break;
                case 2:
                    light.color = Color.red; break;
                case 3:
                    light.color = Color.magenta; break;
                case 4:
                    light.color = Color.white; break;
            }
            index++;
        }
        else if (Vector3.Distance(transform.position, flowers.transform.position) < range)
        {
            StartCoroutine(GatherFlower());
        }
    }

    private IEnumerator OpenDoor()
    {
        door.GetComponent<Animator>().Play("DoorOpen");
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(5f);
        door.GetComponent<Animator>().Play("New State");
    }

    private IEnumerator GatherFlower()
    {
        animator.Play("Gather");
        yield return new WaitForSeconds(0.05f);
        yield return new WaitForSeconds(1.5f);
        flowers.SetActive(false);
        grass.SetActive(true);
    }
}
