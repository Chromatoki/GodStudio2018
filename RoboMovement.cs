using UnityEngine;
using System.Collections;
using Prime31;


public class RoboMovement : MonoBehaviour
{
	// movement config
    public float fl_move_speed = 20f;
    public float fl_acceleration = 5f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;

	//[HideInInspector]
	//private float normalizedHorizontalSpeed = 0;
	//private float normalizedVerticalSpeed = 0;

	private RoboController _controller;
    //private Animator _animator;
    private Rigidbody2D RB_PC;
    private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

	//private  UnityEngine.SceneManagement.Scene _scene;

	private static int currentLevel;
    public bool effected = false;

	private bool firingTrigger = false;
	private bool firingDead = false;

	private AudioSource jumpSound;
    public GameObject EngineSound;
    private AudioSource _as;

	public bool wantJump;

	void Awake()
	{
        //_animator = GetComponent<Animator>();
        _as = EngineSound.GetComponent<AudioSource>();
        _controller = GetComponent<RoboController>();
        RB_PC = GetComponent<Rigidbody2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

		//_scene  = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		currentLevel = 0;//_scene.buildIndex;

		jumpSound = GetComponent<AudioSource>();
	}

	void Start(){
		StartCoroutine(CharacterUpdate());
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.W))
        {
            theVacuum.isVacuumCharging = true;
        }
	}

	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
        //bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent( Collider2D col )
	{
		string tag = col.gameObject.tag;
		//Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
		if(tag == "deadTrigger"){
			firingDead = true;
		}else if(tag == "onlyTrigger"){
			firingTrigger = true;
		}
	}


	void onTriggerExitEvent( Collider2D col )
	{
		//firingTrigger = false;
	}

	#endregion

	// the Update loop contains a very simple example of moving the character around and controlling the animation
	IEnumerator CharacterUpdate()
	{
        while (true){

            if (!effected)
            {
                if (Input.GetAxis("Horizontal") != 0)
                {
                    if (!_as.isPlaying)
                    {
                        _as.volume = 1;
                        _as.Play();
                    }

                    if ((RB_PC.velocity.x > 0 && Input.GetAxis("Horizontal") < 0) || (RB_PC.velocity.x < 0 && Input.GetAxis("Horizontal") > 0))
                    {
                        RB_PC.velocity = new Vector2(0, RB_PC.velocity.y);
                        EngineSound.SetActive(true);
                    }
                }
                else
                {
                    if (_as.isPlaying)
                    {
                        if (_as.volume != 0)
                        {
                            _as.volume = _as.volume - 0.05f;
                        }
                        else
                        {
                            _as.Stop();
                        }
                    }
                }


                if (Mathf.Abs(RB_PC.velocity.x) < fl_move_speed)
                {
                    RB_PC.AddForce(new Vector2(Input.GetAxis("Horizontal") * fl_acceleration, 0));
                }
            }


            // we can only jump whilst grounded
            if (_controller.isGrounded && wantJump && vacuum_attached)
            {
                wantJump = false;
                if (!firingTrigger)
                {

                    if (theVacuum.vacuumRaycastHit)
                    {
                        RB_PC.velocity = _velocity;

                        //if (theVacuum.contactHit.x > gameObject.transform.position.x)
                        //{
                        //    _velocity.x = -Mathf.Sqrt(2f * jumpHeight);
                        //    //_animator.Play( Animator.StringToHash( "Jump" ) );
                        //    if (jumpSound)
                        //        jumpSound.Play();
                        //}

                        //if (theVacuum.contactHit.x < gameObject.transform.position.x)
                        //{
                        //    _velocity.x = Mathf.Sqrt(2f * jumpHeight);
                        //    //_animator.Play( Animator.StringToHash( "Jump" ) );
                        //    if (jumpSound)
                        //        jumpSound.Play();
                        //}

                        if (theVacuum.contactHit.y > gameObject.transform.position.y)
                        {
                            _velocity.y = -Mathf.Sqrt(2f * jumpHeight);
                            //_animator.Play( Animator.StringToHash( "Jump" ) );
                            if (jumpSound)
                                jumpSound.Play();
                        }

                        if (theVacuum.contactHit.y < gameObject.transform.position.y)
                        {
                            _velocity.y = Mathf.Sqrt(2f * jumpHeight);
                            //_animator.Play( Animator.StringToHash( "Jump" ) );
                            if (jumpSound)
                                jumpSound.Play();
                        }
                    }

                    yield return new WaitForEndOfFrame();

                }
                else
                {
                    // Switch level 
                    Debug.Log("Load next Level");
                    loadNextLevel();
                }

            }

            if (firingDead)
            {
                reloadLevel();
            }

            //var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
            //_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

            // if holding down bump up our movement amount and turn off one way platform detection for a frame.
            // this lets uf jump down through one way platforms
            if (_controller.isGrounded && Input.GetKey(KeyCode.DownArrow))
            {
                _controller.ignoreOneWayPlatformsThisFrame = true;
            }

            yield return new WaitForEndOfFrame();
            _controller.move(RB_PC.velocity * Time.deltaTime);

            // grab our current _velocity to use as a base for all calculations
            RB_PC.velocity = _controller.velocity;
        }
	}

	internal void reloadLevel(){
		firingDead = false;
		Debug.Log (currentLevel);
		#if UNITY_5_3_OR_NEWER
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(currentLevel);
		#else
		Application.LoadLevelAsync(Application.loadedLevel);
		#endif
		
		//
		
	}
	
	internal void loadNextLevel(){
		firingTrigger = false;
		currentLevel ++;
		
		#if UNITY_5_3_OR_NEWER
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
		#else
		Application.LoadLevelAsync(Application.loadedLevel + 1);
		#endif
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PointEffector2D>() != null)
        {
            effected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PointEffector2D>() != null)
        {
            effected = false;
        }
    }

    #region Vacuum Controller State

    /// <summary>
    /// checks if vacuum is attached to robot
    /// </summary>
    public bool vacuum_attached;

    /// <summary>
    /// gets vacuum controller
    /// </summary>
    public VacuumControllerDemo theVacuum;
    #endregion
}
