using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DiscController : MonoBehaviour
{
    private bool flying = false;

    public Bag currentBag;

    public float maxSpeed;
    public float maxRotation;


    public Vector3 halfExtents;


    public Vector3 startPosition;
    private Quaternion startRotation;

    private MiniGameController m_mgController;

    // public MiniGameController mgController
    // {
    //     get
    //     {
    //         return m_mgController != null
    //             ? m_mgController
    //             : m_mgController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();
    //     }
    //
    //     set { m_mgController = value; }
    // }

    public float C_lo;
    public float C_lalpha;
    public float C_do;
    public float C_dalpha;
    public float C_mo;
    public float C_malpha;
    public float C_mq;
    public float C_lr;
    public float C_lp;
    public float C_nr;

    public float m;
    public float I_a;
    public float I_d;
    public float d;
    public float r, q, p;
    public float area;
    public float rho;

    public float g;

    public float angleOfAttack;

    public Vector3 velocity;

    private void Start()
    {
        //mgController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();
        startRotation = transform.rotation;

        C_lo = 0.188f;
        C_lalpha = 2.37f;
        C_do = 0.15f;
        C_dalpha = 1.24f;
        C_mo = -.06f;
        C_malpha = 0.38f;
        C_mq = 0.0008f;
        C_lr = 0.0004f;
        C_lp = -0.013f;
        C_nr = -.000028f;

        m = 0.175f;
        I_a = 0.00235f;
        I_d = 0.00122f;
        d = 0.269f;
        area = 0.057f;
        rho = 1.23f;
        g = 9.794f;
    }

    private Vector3 throwStart;
    private float maxDistance = 100;
    public void Throw(float speedMod, float spinMod, Vector3 direction)
    {
        //mgController.OnThrow(this);
        Debug.Log("Start throw with speed: " + speedMod);
        // transform.forward = velocity;
        velocity = speedMod * maxSpeed * direction;
        angleOfAttack = CalculateAngleOfAttack();

        r = spinMod * maxRotation;
        flying = true;
        velocityLine.positionCount = 2;
        d1Line.positionCount = 2;
        liftLine.positionCount = 2;
        throwStart = transform.position;
    }

    public LineRenderer velocityLine;
    public float lineSize;

    public LineRenderer d1Line;

    public LineRenderer liftLine;
    public void ShowVelocity()
    {
        velocityLine.SetPositions(new[] {transform.position, transform.position + velocity.normalized * lineSize});
    }

    public void ShowD1(Vector3 d1)
    {
        d1Line.SetPositions(new[] {transform.position, transform.position + d1.normalized * lineSize});
    }

    public void ShowLift(Vector3 lift)
    {
        liftLine.SetPositions(new[] {transform.position, transform.position + lift.normalized * lineSize});
    }

    public void OnGrab()
    {
        flying = false;
        velocity = Vector3.zero;
    }

    // https://www.cuemath.com/geometry/angle-between-a-line-and-a-plane/
    public float CalculateAngleOfAttack()
    {
        float a = transform.up.x;
        float b = transform.up.y;
        float c = transform.up.z;

        float l = velocity.x;
        float m = velocity.y;
        float n = velocity.z;

        float numerator = Mathf.Abs(a * l + b * m + c * n);
        float denominator = Mathf.Sqrt(a * a + b * b + c * c) * Mathf.Sqrt(l * l + m * m + n * n);

        return Mathf.Asin(numerator / denominator);
    }

    private void FixedUpdate()
    {
        if (flying)
        {
            ShowVelocity();
            angleOfAttack = CalculateAngleOfAttack();
            Vector3 d_1 = Vector3.RotateTowards(velocity, transform.up, angleOfAttack, 0);
            ShowD1(d_1);
            Vector3 d_3 = -transform.up;
            Vector3 d_2 = Vector3.Cross(d_3, d_1);

            Vector3 liftForce = CalculateLift(d_1, d_2, d_3);
            ShowLift(liftForce);
            Vector3 dragForce = CalculateDrag();
            Vector3 gravityForce = CalculateGravity();

            float rollingMoment = CalculateRollingMoment();
            float pitchingMoment = CalculatePitchingMoment();
            float spindownMoment = CalculateSpindownMoment();

            Vector3 totalForce = liftForce + dragForce + gravityForce;
            Vector3 acceleration = totalForce / m;

            //apply acceleration to velocity;
            velocity += acceleration * Time.fixedDeltaTime;

            float rollingAcceleration = rollingMoment / I_d;
            float pitchingAcceleration = pitchingMoment / I_d;
            float spindownAcceleration = spindownMoment / I_a;

            p += rollingAcceleration * Time.fixedDeltaTime;
            q += pitchingAcceleration * Time.fixedDeltaTime;
            r += spindownAcceleration * Time.fixedDeltaTime;

            Vector3 movementVector = velocity * Time.fixedDeltaTime;

            RaycastHit[] results = UnityEngine.Physics.BoxCastAll(transform.position, halfExtents, movementVector,
                transform.rotation, .3f);

            foreach (RaycastHit hit in results)
            {
                if (hit.transform.gameObject.CompareTag("ground"))
                {
                    //make this an event
                    //mgController.OnMiss();
                    StartCoroutine(DiscDelay());
                    flying = false;
                }
            }

            if (Vector3.Distance(throwStart, transform.position) > maxDistance)
            {
                StartCoroutine(DiscDelay());
                flying = false;
            }

     //      transform.Rotate(d_1, p * Time.fixedDeltaTime);
      //    transform.Rotate(d_2, q * Time.fixedDeltaTime);
         //   transform.Rotate(d_3, r * Time.fixedDeltaTime);


            transform.position += movementVector;
        }

    }

    Vector3 CalculateLift(Vector3 d1,Vector3 d2, Vector3 d3)
    {
        float liftMagnitude = (C_lo + C_lalpha * angleOfAttack) * rho * area * velocity.sqrMagnitude / 2;
 
        Vector3 direction = -Vector3.Cross(velocity, d2);


        Vector3
            liftDirection = Vector3.up;//Vector3.RotateTowards(velocity, transform.up, Mathf.PI / 2, 0).normalized;
        return liftDirection * liftMagnitude;
    }

    Vector3 CalculateDrag()
    {
        float dragMagnitude = (C_do + C_dalpha * angleOfAttack * angleOfAttack) * rho * area;
        Vector3 dragDirection = -velocity.normalized;
        return dragDirection * dragMagnitude;
    }

    Vector3 CalculateGravity()
    {
        float gravityMagnitude = m * g;
        Vector3 gravityDirection = Vector3.down;
        return gravityDirection * gravityMagnitude;
    }

    float CalculateRollingMoment()
    {
        return (C_lr * r + C_lp * p) * rho * d * area * velocity.sqrMagnitude / 2;
    }

    float CalculatePitchingMoment()
    {
        return (C_mo + C_malpha * angleOfAttack + C_mq * q) * rho * d * area * velocity.sqrMagnitude / 2;
    }

    float CalculateSpindownMoment()
    {
        return C_nr * r * rho * d * area * velocity.sqrMagnitude / 2;
    }

    IEnumerator DiscDelay()
    {
        yield return new WaitForSeconds(1);
        transform.position = startPosition;
        transform.rotation = startRotation;

        velocityLine.positionCount = 0;
        d1Line.positionCount = 0;
        liftLine.positionCount = 0;
    }
}