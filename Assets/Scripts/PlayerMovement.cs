using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float gravity = -9.81f;
    [SerializeField] private float moveSpeed = 6f;
    private Vector3 velocity;
    private float jump;
    private CharacterController controller;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float jumpHeight;
    public Transform ground;
    public float distanceToGround = 0.4f;
    public LayerMask groundLayer;

    [SerializeField] private float shiftMove;
    [SerializeField] private float normalSpeed;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float normalFov;
    [SerializeField] private float SprintFov;

    [Header("Inventory")]
    private Inventory inventory;

    [Header("Sword")]
    [SerializeField] Animator swordAnimator;
    [SerializeField] ParticleSystem swordParticle;
    [SerializeField] LayerMask attackLayer;
    [SerializeField] float swordAttackDistance;
    [SerializeField] AttackTime attackTime;

    [Header("Gun")]
    [SerializeField] Animator gunAnimator;
    [SerializeField] int gunCurrentAmmo;
    [SerializeField] int gunMaxAmmo;
    float coolDownTimeLeft;
    [SerializeField] float coolDownTime;
    bool canShoot;

    [Header("Inventory Items")]
    //Compass
    [SerializeField] Transform compassTargetPositionNorth;
    [SerializeField] Transform needle;
    //SpyGlass
    [SerializeField] GameObject scopeUI;
    [SerializeField] bool scoperEnabled;

    [Header("Player Health")]
    public int currentHealth;
    [SerializeField] int maxHealth = 100;
    public int potions;
    [SerializeField] Animator potionAnimator;

    [Header("Rum")]
    public bool rumEffect;
    [SerializeField] Animator rumAnimator;

    [Header("Quest System")]
    [SerializeField] private Animator questAnimator;

    private void Start()
    {
        inventory = gameObject.GetComponent<Inventory>();
        moveSpeed = normalSpeed;
        controller = gameObject.GetComponent<CharacterController>();
        canShoot = true;
        gunCurrentAmmo = gunMaxAmmo;
        coolDownTimeLeft = coolDownTime;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        PlayerMove();
        Jump();
        Gravity();
        PlayerAttack();
        InventoryItems();      
    }

    void InventoryItems()
    {
        //Compass
        if (inventory.currentItem == 2)
        {
            // get worldspace coordinates of target
            Vector3 target = compassTargetPositionNorth.transform.position;
            // Vector3 target = transform.position + Vector3.forward; // (to make needle point north)

            // convert to local coordinate space of compass body
            Vector3 relativeTarget = needle.transform.parent.InverseTransformPoint(target);

            // determine needle rotation with atan2
            float needleRotation = Mathf.Atan2(relativeTarget.x, relativeTarget.z) * Mathf.Rad2Deg;

            // apply needle rotation
            needle.transform.localRotation = Quaternion.Euler(-needleRotation, 0, 0);
        }

        //Spy Glass
        if (inventory.currentItem == 3)
        {
            if (Input.GetMouseButton(0))
            {
                scoperEnabled = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                scoperEnabled = false;
            }
        }
        if (scoperEnabled)
        {
            scopeUI.SetActive(true);
            cam.fieldOfView = 10;
        }
        else
        {
            scopeUI.SetActive(false);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFov, 10 * Time.deltaTime);
        }

        //Health
        if (Input.GetMouseButtonDown(0) && inventory.currentItem == 4 && !potionAnimator.GetBool("heal"))
        {
            if (potions > 0)
            {
                potions--;
                potionAnimator.SetBool("heal", true);
                StartCoroutine(Heal());
                if (currentHealth < maxHealth)
                {
                    currentHealth += 40;
                }

                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
            }
        }

        //Rum
        if(Input.GetMouseButtonDown(0) && inventory.currentItem == 5 && !rumEffect)
        {
            rumEffect = true;
            rumAnimator.SetBool("drink", true);
            StartCoroutine(Rum());
        }

        //Quest
        if (Input.GetMouseButton(0) && inventory.currentItem == 7)
        {
            questAnimator.SetBool("look", true);
        }

        if (Input.GetMouseButtonUp(0) && inventory.currentItem == 7)
        {
            questAnimator.SetBool("look", false);
        }

        if (Input.GetMouseButton(0) && inventory.currentItem == 8)
        {
            questAnimator.SetBool("look", true);
        }

        if (Input.GetMouseButtonUp(0) && inventory.currentItem == 8)
        {
            questAnimator.SetBool("look", false);
        }
    }

    void PlayerAttack()
    {
        //Sword Attack
        if (Input.GetMouseButtonDown(0) && inventory.currentItem == 1)
        {
            int currentAnim = swordAnimator.GetInteger("Attack");
            if (currentAnim == 0)
            {
                swordAnimator.SetInteger("Attack", UnityEngine.Random.Range(1, 4));
                swordParticle.Stop();
                swordParticle.Play();
            }
        }

        if (attackTime.canAttack)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, swordAttackDistance, attackLayer))
            {
                Destroy(hit.transform.gameObject);  //Add Damage Later
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && canShoot)
        {
            canShoot = false;
            gunAnimator.SetBool("shoot", true);
            StartCoroutine(Shoot());
        }
        if (canShoot == false)
        {
            coolDownTimeLeft -= Time.deltaTime;
            if (coolDownTimeLeft <= 0)
            {
                canShoot = true;
                coolDownTimeLeft = coolDownTime;
            }
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && inventory.currentItem == 0)
        {
            moveSpeed = shiftMove;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, SprintFov, 10 * Time.deltaTime);
        }

        else
        {
            moveSpeed = normalSpeed;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFov, 10 * Time.deltaTime);
        }
    }
    void Gravity()
    {
        isGrounded = Physics.CheckSphere(ground.position, distanceToGround, groundLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -3f;
        }
        if(velocity.y <= -9f)
        {
            velocity.y = -9f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.5f);        
        gunAnimator.SetBool("shoot", false);
    }

    IEnumerator Heal()
    {
        yield return new WaitForSeconds(1f);
        potionAnimator.SetBool("heal", false);
    }

    IEnumerator Rum()
    {
        yield return new WaitForSeconds(1f);
        rumAnimator.SetBool("drink", false);
        yield return new WaitForSeconds(19f);
        rumEffect = false;
    }
}