using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class AdjustableCoefficient
{
    public string LabelText;

    private Text _cText;

    public Text CoefficientText
    {
        get => _cText;
        set
        {
            _cText = value;
            _cText.text = _cValue.ToString();
        }
    }

    private float _cValue;

    public float CoefficientValue
    {
        get => _cValue;
        set
        {
            if (CoefficientText != null)
            {
                CoefficientText.text = value.ToString();
            }

            _cValue = value;
        }
    }

    public float CoefficentMod;
}

public class DiscController : MonoBehaviour
{
    private bool flying = false;

    public Bag currentBag;

    public float maxSpeed;
    public float maxRotation;


    public Vector3 halfExtents;

    public GameObject AdjustableFloatUIPrefab;

    public Vector3 startPosition;
    public Quaternion startRotation;

    private MiniGameController m_mgController;

    public Text velocityText;

    #region Coefficients
    
    public float m;
    public float I_a;
    public float I_d;
    public float d;
    public float r, q, p;
    public float area;
    public float rho;

    public float g;

    # endregion

    public float angleOfAttack;

    public Vector3 velocity;

    public GameObject UIParent;
    private Dictionary<string, AdjustableCoefficient> Coefficients = new Dictionary<string, AdjustableCoefficient>();

    private void Start()
    {
        //mgController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();
        startRotation = transform.rotation;

        Coefficients.Add("CLo", new AdjustableCoefficient() {CoefficientValue = 0.188f, CoefficentMod = .01f, LabelText = "Lift"});
        Coefficients.Add("CLa",
            new AdjustableCoefficient() {CoefficientValue = 2.37f, CoefficentMod = .01f, LabelText = "Lift x A of A"});
        Coefficients.Add("CDo", new AdjustableCoefficient() {CoefficientValue = 0.15f, CoefficentMod = .01f, LabelText = "Drag"});
        Coefficients.Add("CDa",
            new AdjustableCoefficient() {CoefficientValue = 1.24f, CoefficentMod = .01f,LabelText = "Drag x A of A"});
        Coefficients.Add("CMo", new AdjustableCoefficient() {CoefficientValue = -.06f, CoefficentMod = .01f, LabelText = "Pitching"});
        Coefficients.Add("CMa",
            new AdjustableCoefficient() {CoefficientValue = 0.38f, CoefficentMod = .01f, LabelText = "Pitching x A of A"});
        Coefficients.Add("CMq",
            new AdjustableCoefficient() {CoefficientValue = 0.0008f, CoefficentMod = .0001f, LabelText = "Pitching x Pitching Velocity"});
        Coefficients.Add("CLr", new AdjustableCoefficient() {CoefficientValue = 0.0004f, CoefficentMod = .01f, LabelText = "Rolling"});
        Coefficients.Add("CLp",
            new AdjustableCoefficient() {CoefficientValue = -0.013f, CoefficentMod = .0001f, LabelText = "Rolling x Rolling Velocity"});
        Coefficients.Add("CNr", new AdjustableCoefficient() {CoefficientValue = -.000028f,  CoefficentMod = .00001f,LabelText = "Spindown"});

        int count = 0;
        foreach (var test in Coefficients)
        {
            GameObject adjustableFloatUI = Instantiate(AdjustableFloatUIPrefab, UIParent.transform, false);
            adjustableFloatUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-396, 166 - 30 * count);
            adjustableFloatUI.GetComponent<RectTransform>().localScale = Vector3.one;
            adjustableFloatUI.GetComponent<RectTransform>().localRotation = quaternion.identity;


            var ui = adjustableFloatUI.GetComponent<AdjustableFloatUI>();
            ui.LabelText.text = test.Value.LabelText;
            test.Value.CoefficientText = ui.ValueText;

            ui.DownButton.onClick.AddListener(() => LowerCoefficientValue(test.Key));
            ui.UpButton.onClick.AddListener(() => RaiseCoefficientValue(test.Key));

            count++;
        }



        m = 0.175f;
        I_a = 0.00235f;
        I_d = 0.00122f;
        d = 0.269f;
        area = 0.057f;
        rho = 1.23f;
        g = 9.794f;
    }


    public void  RaiseCoefficientValue(string memberName)
    {
        Coefficients[memberName].CoefficientValue += Coefficients[memberName].CoefficentMod;
    }

    public void LowerCoefficientValue(string memberName)
    {
        Coefficients[memberName].CoefficientValue -= Coefficients[memberName].CoefficentMod;

    }

    public Canvas DiscUICanvas;

    public void ShowCanvas(GameObject centerEye)
    {
        Debug.Log("Show Canvas");
        DiscUICanvas.enabled = true;
        DiscUICanvas.worldCamera = centerEye.GetComponent<Camera>();
        Vector3 target = DiscUICanvas.transform.position + DiscUICanvas.transform.position -
                         centerEye.transform.position;
        DiscUICanvas.transform.LookAt(target);
    }

    public void HideCanvas()
    {
        DiscUICanvas.enabled = false;
    }

    private Vector3 throwStart;
    private float maxDistance = 100;

    public void Throw(float speedMod, float spinMod, Vector3 direction)
    {
        flying = true;

        transform.parent = null;

        Vector3 d_1 = Vector3.RotateTowards(velocity, Vector3.up, angleOfAttack, 0);
        Vector3 d_3 = -transform.up;
        Vector3 d_2 = Vector3.Cross(d_3, d_1);


        velocity = speedMod * maxSpeed * direction;


        angleOfAttack = CalculateAngleOfAttack();
        q = angleOfAttack;
        p = CalculateRollingAngle(d_2);
        r = spinMod * maxRotation;

        foreach (var line in VectorLines.Values)
        {
            line.positionCount = 2;
        }

        throwStart = transform.position;
    }

    public float lineSize;


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

    // https://www.cuemath.com/geometry/angle-between-a-line-and-a-plane/
    public float CalculateRollingAngle(Vector3 d2)
    {
        float a = transform.up.x;
        float b = transform.up.y;
        float c = transform.up.z;

        float l = d2.x;
        float m = d2.y;
        float n = d2.z;

        float numerator = Mathf.Abs(a * l + b * m + c * n);
        float denominator = Mathf.Sqrt(a * a + b * b + c * c) * Mathf.Sqrt(l * l + m * m + n * n);

        return Mathf.Asin(numerator / denominator);
    }

    private Dictionary<Color, LineRenderer> VectorLines = new Dictionary<Color, LineRenderer>();


    private void ShowVector(Vector3 vector, Color color)
    {
        if (!VectorLines.ContainsKey(color))
        {
            GameObject lineObj = new GameObject();
            LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.material.color = color;
            lineRenderer.startWidth = .05f;
            lineRenderer.positionCount = 2;

            VectorLines.Add(color, lineRenderer);

            lineObj.transform.parent = transform;
        }


        VectorLines[color].SetPositions(new[] {transform.position, transform.position + vector.normalized * lineSize});
    }

    private void FixedUpdate()
    {
        if (flying)
        {
            // ShowVelocity();
            angleOfAttack = CalculateAngleOfAttack();
            Vector3 d_1 = Vector3.RotateTowards(velocity, Vector3.up, angleOfAttack, 0);
            ShowVector(velocity, Color.green);

            // ShowD1(d_1);
            Vector3 d_3 = -transform.up;
            Vector3 d_2 = Vector3.Cross(d_3, d_1);
            ShowVector(d_3, Color.black);
            Vector3 liftForce = CalculateLift(d_1, d_2, d_3);
            //  ShowLift(liftForce);
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


            //Ground check raycast
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

            //return to start if farther than max distance
            if (Vector3.Distance(throwStart, transform.position) > maxDistance)
            {
                transform.parent = currentBag.transform;

                transform.localPosition = startPosition;
                transform.localRotation = startRotation;

                foreach (var line in VectorLines.Values)
                {
                    line.positionCount = 0;
                }

                flying = false;
            }

            transform.Rotate(d_1, p * Time.fixedDeltaTime, Space.World);

            transform.Rotate(d_3, r * Time.fixedDeltaTime, Space.World);
            transform.Rotate(d_2, -q * Time.fixedDeltaTime, Space.World);
            //transform.rotation = Quaternion.Euler(new Vector3(p, q, r));


            transform.position += movementVector;
        }
    }

    Vector3 CalculateLift(Vector3 d1, Vector3 d2, Vector3 d3)
    {
        float liftMagnitude = (Coefficients["CLo"].CoefficientValue + Coefficients["CLa"].CoefficientValue * angleOfAttack) * rho * area * velocity.sqrMagnitude / 2;

        Vector3 direction = -Vector3.Cross(velocity, d2).normalized;

        return direction * liftMagnitude;
    }

    Vector3 CalculateDrag()
    {
        float dragMagnitude = (Coefficients["CDo"].CoefficientValue + Coefficients["CDa"].CoefficientValue * angleOfAttack * angleOfAttack) * rho * area;
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
        return (Coefficients["CLr"].CoefficientValue * r + Coefficients["CLp"].CoefficientValue * p) * rho * d * area * velocity.sqrMagnitude / 2;
    }

    float CalculatePitchingMoment()
    {
        return (Coefficients["CMo"].CoefficientValue + Coefficients["CMa"].CoefficientValue * angleOfAttack + Coefficients["CMq"].CoefficientValue * q) * rho * d * area * velocity.sqrMagnitude / 2;
    }

    float CalculateSpindownMoment()
    {
        return Coefficients["CNr"].CoefficientValue * r * rho * d * area * velocity.sqrMagnitude / 2;
    }

    IEnumerator DiscDelay()
    {
        yield return new WaitForSeconds(1);
        currentBag.DiscLanded(transform.position);

        transform.parent = currentBag.transform;

        transform.localPosition = startPosition;
        transform.localRotation = startRotation;

        foreach (var line in VectorLines.Values)
        {
            line.positionCount = 0;
        }
    }
}