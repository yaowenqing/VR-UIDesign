using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR.InteractionSystem;
using System.IO;
using UnityEngine.SceneManagement;
using Valve.VR;

public class moveball : MonoBehaviour
{
    public enum ExpType
    {
        V,
        A,
        H,
        VA,
        VH,
        AH,
        VAH
    }

    public hit hit;
    public ExpType expType = ExpType.V;

    //public GameObject gameObject;
    public Vector3 targetPosition = new Vector3(0, 1.5f, 0f);

    //[Range(0.1f, 5)]
    public float moveSpeed = 2;

    //[Range(20, 70)]
    public float beginShootAngle = 45;

    private float distanceToTarget;
    private bool isRunning = true;
    private int i = 0;//统计试验次数

    private AudioSource[] m_ArrayMusic;
    private AudioSource m_sound_flying;
    private AudioSource m_sound_hitted;
    private AudioSource m_sound_inRegion;
    private AudioSource m_sound_serve;

    private string user = "user_2";

    public Hand leftHand;
    public Hand rightHand;

    private GrabTypes bestGrab;

    private float[,] speedAngle = new float[,]{
            { 3, 30 },{ 3, 30 }, { 3, 30 }, { 3, 30 }, { 3, 30 }, { 3, 30 }, { 3, 30 }, { 3, 30 }, { 3, 30 }, { 3, 30 }, { 3, 30 }, { 3, 30 },
            { 4, 30 },{ 4, 30 }, { 4, 30 }, { 4, 30 }, { 4, 30 }, { 4, 30 }, { 4, 30 }, { 4, 30 }, { 4, 30 }, { 4, 30 }, { 4, 30 }, { 4, 30 },
            { 5, 30 },{ 5, 30 }, { 5, 30 }, { 5, 30 }, { 5, 30 }, { 5, 30 }, { 5, 30 }, { 5, 30 }, { 5, 30 }, { 5, 30 }, { 5, 30 }, { 5, 30 }
        };

    const int conditionCount = 36;

    private float[,] data = new float[conditionCount, 11];
    private float currentDist; //记录羽毛球到终点的直线距离

    public float timeBraek = 1f;
    private float timer;

    public Camera VRCamera;

    //private float Dt = 1.5f;//飞行时间
    //private float Wt = 0.2f;//击球时间

    private float r1 = 0.6f; //窗口半径，内部的球半径
    private float r2 = 1.5f; // 窗口半径，外部的球半径
    private GameObject SphereW;
    private GameObject SphereD;

    // Use this for initialization
    private void Awake()
    {
        expType = Tool.exptype;
    }

    void Start()
    {
        timer = timeBraek;

        if (expType == ExpType.V ||
            expType == ExpType.VA ||
            expType == ExpType.VH ||
            expType == ExpType.VAH)
        {
            VRCamera.enabled = true;
        }
        else
            VRCamera.enabled = false;

        m_ArrayMusic = this.GetComponents<AudioSource>();
        m_sound_flying = m_ArrayMusic[0];
        m_sound_hitted = m_ArrayMusic[1];
        m_sound_inRegion = m_ArrayMusic[2];
        m_sound_serve = m_ArrayMusic[3];
        SphereW = GameObject.Find("SphereW");
        SphereD = GameObject.Find("SphereD");

        this.transform.position = new Vector3(0f, 1.5f, 6f);

        GetDisorderBytes(speedAngle);//产生随机数组，随机发球

        moveSpeed = speedAngle[i, 0];
        beginShootAngle = speedAngle[i, 1];
        //Tool.GetSphereRadius(moveSpeed, Dt, Wt, out r1, out r2);
        SphereW.transform.localScale = new Vector3(2 * r1, 2 * r1, 2 * r1);
        SphereD.transform.localScale = new Vector3(2 * r2, 2 * r2, 2 * r2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Reset();
        }

        if (!isRunning)
            return;

        GetComponents<AudioSource>()[0].volume = 1 / Vector3.Distance(this.transform.position, targetPosition);

        float amplitude = Mathf.Pow((5 - Vector3.Distance(this.transform.position, targetPosition)) / 6, 2);
        float frequency = 50 * (7 - Vector3.Distance(this.transform.position, targetPosition)) / 6;

        if (expType == ExpType.H ||
            expType == ExpType.VH ||
            expType == ExpType.AH ||
            expType == ExpType.VAH)
        {
            leftHand.TriggerHapticPulse(0.01f, frequency, amplitude);
            rightHand.TriggerHapticPulse(0.01f, frequency, amplitude);
        }

        transform.LookAt(targetPosition, Vector3.up);

        float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPosition) / distanceToTarget) * beginShootAngle;

        transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -beginShootAngle, beginShootAngle), 0, 0);
        currentDist = Vector3.Distance(this.transform.position, targetPosition);

        transform.Translate(Vector3.forward * Mathf.Min(moveSpeed * Time.deltaTime, currentDist));

        //if (Input.GetKeyDown(KeyCode.W))// we're pressing the trigger
        //if (SteamVR_Input.__actions_default_in_Teleport.GetStateDown(rightHand.handType) ||
        //    SteamVR_Input.__actions_default_in_Teleport.GetStateDown(leftHand.handType))
        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.Any))
        {            
            hit.Hit();

            if (currentDist <= r2 && currentDist >= r1)
            {
                if (expType == ExpType.A ||
                    expType == ExpType.VA ||
                    expType == ExpType.AH ||
                    expType == ExpType.VAH)
                {
                    m_sound_flying.Stop();
                    m_sound_inRegion.Stop();
                }

                m_sound_hitted.Play();

                data[i, 4] = (float)1;
                data[i, 6] = (float)Vector3.Distance(this.transform.position, targetPosition);
                Debug.Log("实验" + expType + ";第" + i + "次击球" + ";羽毛球的速度为" + moveSpeed + ";发球的角度为" + beginShootAngle + ";击中与否" + "true" + ";球的位置" + currentDist);
                this.transform.position = new Vector3(0f, 1.5f, 6f);
                isRunning = false;
            }
            else
            {
                if (expType == ExpType.A ||
                    expType == ExpType.VA ||
                    expType == ExpType.AH ||
                    expType == ExpType.VAH)
                {
                    m_sound_flying.Stop();
                    m_sound_inRegion.Stop();
                }

                data[i, 4] = (float)0;
                data[i, 6] = (float)Vector3.Distance(this.transform.position, targetPosition);
                Debug.Log("实验" + expType + ";第" + i + "次击球" + ";羽毛球的速度为" + moveSpeed + ";发球的角度为" + beginShootAngle + ";击中与否" + "false" + ";球的位置" + Vector3.Distance(this.transform.position, targetPosition));
                this.transform.position = new Vector3(0f, 1.5f, 6f);
                isRunning = false;
            }

            data[i, 5] = (float)(2 * Math.PI - 12 * Mathf.Asin(data[i, 6] / 12)) / moveSpeed; //完成时间
            Debug.Log("完成时间" + data[i, 5]);
        }
        else
        {
            if (currentDist <= 0.01f)//到达终点
            {
                if (expType == ExpType.A ||
                    expType == ExpType.VA ||
                    expType == ExpType.AH ||
                    expType == ExpType.VAH)
                {
                    m_sound_flying.Stop();
                }

                // m_music3.Stop();
                data[i, 4] = -1;
                data[i, 6] = 0;
                Debug.Log("实验" + expType + ";第" + i + "次击球" + ";羽毛球的速度为" + moveSpeed + ";发球的角度为" + beginShootAngle + ";击中与否" + "false" + ";球的位置" + "0");
                this.transform.position = new Vector3(0f, 1.5f, 6f);
                data[i, 5] = -1; //完成时间
                isRunning = false;
            }
            else if (currentDist <= r2 && currentDist >= r1) //进入窗口
            {
                //窗口内 3声音播放，1声音停止
                //m_sound_inRegion.Play();
                //m_sound_inRegion.loop = true;
                if (expType == ExpType.A ||
                    expType == ExpType.VA ||
                    expType == ExpType.AH ||
                    expType == ExpType.VAH)
                {
                    m_sound_flying.pitch = 0.92f;
                }

                if (expType == ExpType.H ||
                    expType == ExpType.VH ||
                    expType == ExpType.AH ||
                    expType == ExpType.VAH)
                {
                    leftHand.TriggerHapticPulse(0.01f, 200, 1);
                    rightHand.TriggerHapticPulse(0.01f, 200, 1);
                }
            }
            else if (currentDist < r1)
            {
                if (expType == ExpType.A ||
                    expType == ExpType.VA ||
                    expType == ExpType.AH ||
                    expType == ExpType.VAH)
                {
                    m_sound_inRegion.Stop();//飞出窗口 3声音停止
                }
            }
        }
    }

    private void Reset()
    {
        if (i == conditionCount)
        {
            this.transform.position = new Vector3(100.00f, 100.00f, -600f);
            Debug.Log(data);
            string fileName = "test_data/" +
                Tool.exptype.ToString() + "_" + Tool.username + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
            string sign = " "; //元素之间分隔符号，此处设置为空格，可自行更改设置
            StreamWriter sw = new StreamWriter(fileName, true); //第一个参数是读取到流的文件名，第二个参数是如果文件不存在，能否创建文件，true为创建新文件，false为不创建
            for (int i = 0; i < conditionCount; i++)
            {
                for (int j = 0; j < 11; j++)
                    sw.Write(data[i, j] + sign); //如果不是string数组，可使用.Tostring()转换在进行连接
                sw.WriteLine();
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();

            isRunning = false;

            SceneManager.LoadScene("Menu");
        }

        if (i < conditionCount)
        {
            if (expType == ExpType.A ||
                expType == ExpType.VA ||
                expType == ExpType.AH ||
                expType == ExpType.VAH)
            {
                m_sound_serve.Play();
                m_sound_flying.Play();
                m_sound_flying.pitch = 1f;
                m_sound_flying.loop = true;
            }

            //this.transform.position = new Vector3(0f, 1.5f, 6f);

            moveSpeed = speedAngle[i, 0];
            beginShootAngle = speedAngle[i, 1];

            //Tool.GetSphereRadius(moveSpeed, Dt, Wt, out r1, out r2);

            SphereW.transform.localScale = new Vector3(2 * r1, 2 * r1, 2 * r1);
            SphereD.transform.localScale = new Vector3(2 * r2, 2 * r2, 2 * r2);

            data[i, 0] = (float)expType; //记录实验类别
            data[i, 1] = (float)i;
            data[i, 2] = (float)moveSpeed;
            data[i, 3] = (float)beginShootAngle;
            data[i, 7] = r1;//内部球半径
            data[i, 8] = r2;//外部球半径
            float Dt = 0;
            float Wt = 0;
            Tool.GetDtWt(6, moveSpeed, r1, r2, out Dt, out Wt);
            data[i, 9] = Dt;
            data[i, 10] = Wt;
            distanceToTarget = Vector3.Distance(this.transform.position, targetPosition);
            i++;
            isRunning = true;
        }

        timer = timeBraek;
    }

    private static void GetDisorderBytes(float[,] byt)//乱序数组
    {
        int min = 0;
        int max = conditionCount - 1;
        int inx = 0;
        float b = 0;
        float c = 0;
        System.Random rnd = new System.Random();
        while (min != max)
        {
            int r = rnd.Next(min++, max);
            b = byt[inx, 0];
            c = byt[inx, 1];
            Debug.Log(r);
            byt[inx, 0] = byt[r, 0];
            byt[inx, 1] = byt[r, 1];
            byt[r, 0] = b;
            byt[r, 1] = c;
            inx++;
        }
    }

    public static void Delay(int milliSecond)
    {
        int start = Environment.TickCount;
        while (Math.Abs(Environment.TickCount - start) < milliSecond)
        {
            //Application.DoEvents();
        }
    }
}