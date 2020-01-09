using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float healDelay;
    public float currentSpeed;
    public float normalSpeed;
    public float rotationSpeed;
    public float acceleratedSpeed;
    public float accelearateTime;
    public float accelerateCooldown;
    public GameObject tankExplosionEffect;
    public GameObject bustedTank;
    public GameObject healEffect;
    public AudioClip tankExplosionAudio;
    public AudioClip tankIdelAudio;
    public AudioClip tankDrivingAudio;
    public KeyCode accelerateKey;
    public Slider hpSlider;
    public Slider accelerateSlider;
    public Text endingText;

    private float vertical;
    private float horizontal;
    private float healTimer;
    private float accelerateTimer;
    private float accelerateCooldownTimer;
    private bool accelerating;
    private bool isReadyToAccelerate;
    private AudioSource audioSource;
    private Rigidbody tank;

    // Start is called before the first frame update
    void Start()
    {
        switch (this.gameObject.tag)
        {
            case "Player1":
                accelerateKey = KeyCode.LeftShift;
                break;
            case "Player2":
                accelerateKey = KeyCode.RightShift;
                break;
        }
        tank = this.GetComponent<Rigidbody>();     // obtains the rigidbody component for attached object
        currentHealth = maxHealth;
        currentSpeed = normalSpeed;
        healTimer = healDelay;
        accelerateCooldownTimer = accelerateCooldown;
        accelerateTimer = accelearateTime;
        accelerating = false;
        isReadyToAccelerate = true;
        audioSource = this.GetComponent<AudioSource>();
        hpSlider.value = currentHealth;
        accelerateSlider.value = accelerateTimer;
    }

    // Update is called once per frame (fixed time)
    void FixedUpdate()
    {
        // timers
        healTimer -= Time.deltaTime;
        if (accelerating) {
            accelerateTimer -= Time.deltaTime;
            accelerateSlider.value = accelerateTimer / accelearateTime;
        }

        if (healTimer <= 0) {
            healTank();
            healTimer = healDelay;
        }
        if (Input.GetKeyDown(accelerateKey) && isReadyToAccelerate) {
            currentSpeed = acceleratedSpeed;
            accelerating = true;
        }
        if ((Input.GetKeyUp(accelerateKey) || accelerateTimer <= 0) && isReadyToAccelerate) {
            currentSpeed = normalSpeed;
            accelerating = false;
            isReadyToAccelerate = false;
        }
        if (!isReadyToAccelerate) {
            accelerateCooldownTimer -= Time.deltaTime;
        }
        if (accelerateCooldownTimer <= 0) {
            isReadyToAccelerate = true;
            accelerateSlider.value = accelearateTime;
            accelerateTimer = accelearateTime;
            accelerateCooldownTimer = accelerateCooldown;
        }

        // Movement control
        switch (this.gameObject.tag) {
            case "Player1":
                vertical = Input.GetAxis("Vertical1");     // range (-1, 1); W for 1, S for -1
                horizontal = Input.GetAxis("Horizontal1");
                break;
            case "Player2":
                vertical = Input.GetAxis("Vertical2");     // range (-1, 1); Up for 1, Down for -1
                horizontal = Input.GetAxis("Horizontal2");
                break;
        }
        if (vertical < 0) {
            tank.angularVelocity = transform.up * horizontal * rotationSpeed * -1;     // provide inversed angular velocity
        } else {
            tank.angularVelocity = transform.up * horizontal * rotationSpeed;      // provide angular velocity
        }
        if (transform.rotation.eulerAngles.x > 0.5 || transform.rotation.eulerAngles.x < -0.5) {
            transform.rotation.Normalize();     // keep rotation = 0
        }

        // Audio control
        tank.velocity = transform.forward * vertical * currentSpeed;      // provide forward/backward velocity
        if (vertical.Equals(0) && horizontal.Equals(0)) {
            // play engine driving audio
            audioSource.clip = tankDrivingAudio;
        } else {
            // play engine idel audio
            audioSource.clip = tankIdelAudio;
        }
        if (!audioSource.isPlaying) audioSource.Play();
        
        // health detection
        if (currentHealth <= 0)
        {
            tankExplosionEffect = GameObject.Instantiate(tankExplosionEffect, transform.position + Vector3.up, transform.rotation) as GameObject;      // instantiate explosion effect
            AudioSource.PlayClipAtPoint(tankExplosionAudio, transform.position);
            GameObject.Instantiate(bustedTank, transform.position, transform.rotation);
            GameObject.Destroy(this.gameObject);
            GameObject.Destroy(tankExplosionEffect, 5);     // destroy explosion effect after 5 sec
            endingText.text = this.gameObject.tag.ToString() + " win!";
            endingText.gameObject.SetActive(true);
        }
    }

    void RecieveDamage()
    {
        if (healEffect.activeSelf) healEffect.SetActive(false);
        currentHealth -= Random.Range(20, 40);     // damage random health (20-40hp)
        hpSlider.value = (float) currentHealth / maxHealth;
        healTimer = healDelay;
    }

    void healTank()
    {
        if (currentHealth <= maxHealth - 10) {
            currentHealth += 10;
            healEffect.SetActive(true);
            hpSlider.value = (float)currentHealth / maxHealth;
        } else {
            healEffect.SetActive(false);
        }
    }
}
