using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : EnemyStats
{

  [Header("Stats")]
  [SerializeField] float speed;
  private float playerDetectTime;
  public float playerDetectRate;
  public float chaseRange;
  bool lookRight;

  [Header("Attack")]
  [SerializeField] float attackRange;
  [SerializeField] float attackRate;
  private float lastAttackTime;
  public Transform attackPoint;
  public LayerMask playerLayerMask;

  [Header("Component")]
  Rigidbody2D rb;
  private PlayerControler targetPlayer;
  Animator anim;

  [Header("Pathfinding")]
  public float nextWaypointDistance = 2f;
  Path path;
  int currentWayPoint = 0;
  bool reachEndPath = false;
  Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
      seeker = GetComponent<Seeker>();
      rb = GetComponent<Rigidbody2D>();
      anim = GetComponent<Animator>();
      InitilizeBar();
      InvokeRepeating("UpdatePath",0f, .5f);
    }

    void UpdatePath()
    {
      if(seeker.IsDone() && targetPlayer != null)
      {
        seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);

      }
    }
    private void Update()
    {
      if(rb.velocity.x > 0 && lookRight|| rb.velocity.x < 0 && !lookRight)
      {
        Flip();
      }
    }

    void OnPathComplete(Path p)
    {
      if (!p.error)
      {
        path = p;
        currentWayPoint = 0;
      }
    }



    private void FixedUpdate()
    {
      if(targetPlayer != null)
      {
        float dist = Vector2.Distance(transform.position, targetPlayer.transform.position);
        if(dist < attackRange && Time.time - lastAttackTime >= attackRate)
        {
          StartCoroutine(Delay());

          IEnumerator Delay()
          {
            yield return new WaitForSeconds(Random.Range(0.1f, 1f));

            if(dist < attackRange)
            {
              anim.SetTrigger("Attack");
              rb.velocity = Vector2.zero;
            }

          }

          lastAttackTime = Time.time;
        }
        else if(dist > attackRange)
        {
          if(path == null)
            return;
          if(currentWayPoint >= path.vectorPath.Count)
          {
            reachEndPath = true;
            return;
          }
          else
          {
            reachEndPath = false;
          }
          Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
          Vector2 force = direction*speed*Time.fixedDeltaTime;

          rb.velocity = force;

          float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

          if(distance < nextWaypointDistance)
          {
            currentWayPoint++;
          }
        }
        else
        {
          rb.velocity = Vector2.zero;
        }
      }




      DetectPlayer();
    }


    void DetectPlayer()
    {
      if(Time.time - playerDetectTime > playerDetectRate)
      {
        playerDetectTime = Time.time;
        foreach(PlayerControler player in FindObjectsOfType<PlayerControler>())
        {
          if(player != null)
          {
            float dist = Vector2.Distance(transform.position, player.transform.position);

            if(player == targetPlayer)
            {
              if(dist > chaseRange)
              {
                targetPlayer = null;
                rb.velocity = Vector2.zero;
                anim.SetBool("onMove", false);

              }
            }else if(dist < chaseRange)
              {
                if(targetPlayer == null)
                  targetPlayer = player;
                anim.SetBool("onMove", true);

              }
            }
          }
        }
      }
      void Flip()
      {
        lookRight = !lookRight;
        transform.Rotate(0,180f,0);
      }
      void Attack()
      {
        Collider2D player = Physics2D.OverlapCircle(attackPoint.transform.position,0.5f, playerLayerMask);
        if(player != null && player.tag == "Player")
        {
          player.GetComponent<PlayerControler>().TakeDamage(damage);
        }
      }

}
