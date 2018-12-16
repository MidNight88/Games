using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 15.0f;
    public float projectileSpeed;
    public float fireRate=0.2f;
    float padding = 0.8f;

    float xmin;
    float xmax;
    public GameObject projectile;
    public float health = 300;
    public AudioClip fireSound;
    public AudioClip deathSound;

    

    // Use this for initialization
    void Start () {
        CameraViewport();
        
    }
	 public void CameraViewport()
    {

        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distance));

        //print("leftmost " + leftmost);
        //print("rightmost " + rightmost);

        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;

        //print("xmin" + xmin);
        //print("xmax " + xmax);
    }


    void Fire()
    {
        GameObject beam = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    // Update is called once per frame
    void Update () {
        //player movement with addjustable speed indipendent from framerate and alternativ control for emotiv emokey
        if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A) || Input.GetMouseButton(0))
        {
            transform.position +=  Vector3.left *speed * Time.deltaTime;

        }else if(Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D)|| Input.GetMouseButton(1))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, fireRate);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }
        

        // restrict the player to the gamespace
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, 0.0f);
	}





    void OnTriggerEnter2D(Collider2D coll)

    {
       
        if (coll)
        {
            Projectile laser = coll.gameObject.GetComponent<Projectile>();
            
            if (laser)
            {
                health -= laser.GetDamage();
                laser.Hit();
                if (health <= 0)
                {
                    Die();
                    

                }

            }
        }
    }

    void Die()
    {
       LevelManager manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        manager.LoadLevel("Win Screen");
    }

}
