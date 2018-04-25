using UnityEngine;
using System.Collections.Generic;

public class CharacterControl : MonoBehaviour {

    private enum ControlMode
    {
        Tank,
		Key,
		Compass,
    }

	[SerializeField] private Vector3 pos = Vector3.zero;
	[SerializeField] private float angle = 0;
    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 4;
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;
	[SerializeField] private CollisionLevel1 collision;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Key;

	public bool canmove = true;
	public bool switchmove = false;
	public int move = 0;

	private int eventx;
	private int eventy;
	private bool calleventnext;

    private float m_currentV = 0;
    private float m_currentH = 0;
	private float moving = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for(int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider)) {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if(validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        } else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

	void Update () {
        m_animator.SetBool("Grounded", m_isGrounded);

        switch(m_controlMode)
        {
            case ControlMode.Key:
                KeyUpdate();
                break;

            case ControlMode.Compass:
                CompassUpdate();
                break;

			case ControlMode.Tank:
				TankUpdate();
				break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }

        m_wasGrounded = m_isGrounded;
    }

    private void TankUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        bool walk = Input.GetKey(KeyCode.LeftShift);

        if (v < 0) {
            if (walk) { v *= m_backwardsWalkScale; }
            else { v *= m_backwardRunScale; }
        } else if(walk)
        {
            v *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);

        JumpingAndLanding();
    }

	private void KeyUpdate() {
		if (switchmove) {
			switchmove = false;
			canmove = true;
			collision.endEvent (eventx, eventy);
			calleventnext = false;
			moving = 1;
		}
		if (!canmove) {
			moving = 0;
			m_animator.SetFloat("MoveSpeed", m_moveSpeed * moving);
			return;
		}
		if(Input.GetKey(KeyCode.A) && transform.position == pos && Mathf.Approximately(angle, transform.eulerAngles.y)) {        // Left
			angle = 270;
			moving = 1;
			if (CheckMovement (pos + Vector3.left * 5)) {
				pos += Vector3.left * 5;
			}
		}
		if(Input.GetKey(KeyCode.D) && transform.position == pos && Mathf.Approximately(angle, transform.eulerAngles.y)) {        // Right
			angle = 90;
			moving = 1;
			if (CheckMovement (pos + Vector3.right * 5)) {
				pos += Vector3.right * 5;
			}
		}
		if(Input.GetKey(KeyCode.W) && transform.position == pos && Mathf.Approximately(angle, transform.eulerAngles.y)) {        // Up
			angle = 0;
			moving = 1;
			if (CheckMovement (pos + Vector3.forward * 5)) {
				pos += Vector3.forward * 5;
			}
		}
		if(Input.GetKey(KeyCode.S) && transform.position == pos && Mathf.Approximately(angle, transform.eulerAngles.y)) {        // Down
			angle = 180;
			moving = 1;
			if (CheckMovement (pos + Vector3.back * 5)) {
				pos += Vector3.back * 5;
			}
		}
		if (!Mathf.Approximately (angle, transform.eulerAngles.y))
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, angle, 0), Time.deltaTime * m_turnSpeed);
		else
			transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * m_moveSpeed);
		if (Mathf.Approximately (angle, transform.eulerAngles.y) && transform.position == pos) {
			moving = 0;
			if (calleventnext) {
				collision.callEvent (eventx, eventy);
				calleventnext = false;
			}
		}
		m_animator.SetFloat("MoveSpeed", m_moveSpeed * moving);
		JumpingAndLanding();
	}

	private void CompassUpdate() {
		if (switchmove) {
			switchmove = false;
			canmove = true;
			collision.endEvent (eventx, eventy);
			calleventnext = false;
			moving = 1;
		}
		if (!canmove) {
			moving = 0;
			m_animator.SetFloat("MoveSpeed", m_moveSpeed * moving);
			return;
		}
		if(move == 1 && transform.position == pos && Mathf.Approximately(angle, transform.eulerAngles.y)) {        // Left
			angle = 270;
			moving = 1;
			if (CheckMovement (pos + Vector3.left * 5)) {
				pos += Vector3.left * 5;
			} else
				move = 0;
		}
		if(move == 2 && transform.position == pos && Mathf.Approximately(angle, transform.eulerAngles.y)) {        // Right
			angle = 90;
			moving = 1;
			if (CheckMovement (pos + Vector3.right * 5)) {
				pos += Vector3.right * 5;
			} else
				move = 0;
		}
		if(move == 3 && transform.position == pos && Mathf.Approximately(angle, transform.eulerAngles.y)) {        // Up
			angle = 0;
			moving = 1;
			if (CheckMovement (pos + Vector3.forward * 5)) {
				pos += Vector3.forward * 5;
			} else
				move = 0;
		}
		if(move == 4 && transform.position == pos && Mathf.Approximately(angle, transform.eulerAngles.y)) {        // Down
			angle = 180;
			moving = 1;
			if (CheckMovement (pos + Vector3.back * 5)) {
				pos += Vector3.back * 5;
			} else
				move = 0;
		}
		if (!Mathf.Approximately (angle, transform.eulerAngles.y))
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.Euler (0, angle, 0), Time.deltaTime * m_turnSpeed);
		else
			transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * m_moveSpeed);
		if (Mathf.Approximately (angle, transform.eulerAngles.y) && transform.position == pos) {
			//move = 0;
			if (move == 0)
				moving = 0;
			if (calleventnext) {
				collision.callEvent (eventx, eventy);
				calleventnext = false;
			}
		}
		m_animator.SetFloat("MoveSpeed", m_moveSpeed * moving);
		JumpingAndLanding();
	}

	public void GridMove(int i) {
		if (move != 0)
			move = 0;
		else {
			move = i;
			CompassUpdate ();
		}
	}

	private bool CheckMovement(Vector3 position) {
		int x = (int)position.x / 5;
		int y = (int)position.z / 5;
		calleventnext = false;
		switch (collision.objects [x, y]) {
			case -1:
				return false;
			case 0:
				return true;
			default:
				eventx = x;
				eventy = y;
				calleventnext = true;
				return false;
		}
	}

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

		if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.LeftControl))
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }
}
