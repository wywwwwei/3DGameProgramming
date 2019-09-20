using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此类挂载到Main Camera上
public class RoundSun : MonoBehaviour
{
    public float distanceUnit;
    //距离单位，通过修改可使整体距离太阳的长度变化
    public float scaleUnit;
    //缩放单位，可使整体变大/变小
    public float rotationUnit;
    //自转速度单位，改变整体自转速度
    public float revolutionUnit;
    //公转速度单位，改变整体公转速度
    protected GameObject sun;
    protected GameObject mercury;
    protected GameObject venus;
    protected GameObject earth;
    protected GameObject mars;
    protected GameObject jupiter;
    protected GameObject saturn;
    protected GameObject uranus;
    protected GameObject neptune;
    protected GameObject moon;
    // Start is called before the first frame update
    void Start()
    {
        loadResource();
        setOriginPos();
        setZoom();
        drawTrack();
    }

    // Update is called once per frame
    void Update()
    {
        setRotation();
        setRevolution();
    }

    void loadResource()
    {
        sun = (GameObject)Instantiate(Resources.Load("Prefabs/Sun"));
        mercury = (GameObject)Instantiate(Resources.Load("Prefabs/Mercury"));
        venus = (GameObject)Instantiate(Resources.Load("Prefabs/Venus"));
        earth = (GameObject)Instantiate(Resources.Load("Prefabs/Earth"));
        mars = (GameObject)Instantiate(Resources.Load("Prefabs/Mars"));
        jupiter = (GameObject)Instantiate(Resources.Load("Prefabs/Jupiter"));
        saturn = (GameObject)Instantiate(Resources.Load("Prefabs/Saturn"));
        uranus = (GameObject)Instantiate(Resources.Load("Prefabs/Uranus"));
        neptune = (GameObject)Instantiate(Resources.Load("Prefabs/Neptune"));
        moon = (GameObject)Instantiate(Resources.Load("Prefabs/Moon"));
    }

    void setOriginPos()
    {
        this.transform.position = new Vector3(0,0,0);
        distanceUnit = 1f; 
        sun.transform.position = new Vector3 (0, -2, 10);
        mercury.transform.position = new Vector3 (2.5f * distanceUnit, -2, 10);
        venus.transform.position = new Vector3 (3.1f * distanceUnit, -2, 10);
        earth.transform.position = new Vector3 (4f * distanceUnit, -2, 10);
        mars.transform.position = new Vector3 (5f * distanceUnit, -2, 10);
        jupiter.transform.position = new Vector3 (6.8f * distanceUnit, -2, 10);
        saturn.transform.position = new Vector3 (9f * distanceUnit, -2, 10);
        uranus.transform.position = new Vector3 (10.7f * distanceUnit, -2, 10);
        neptune.transform.position = new Vector3 (12f * distanceUnit, -2, 10);
        moon.transform.position =   new Vector3 (4.4f * distanceUnit, -2, 10);
    }

    void setZoom()
    {
        scaleUnit = 0.4f;
        float sunScale = 10f * scaleUnit;
        float mercuryScale = 0.8f * scaleUnit;
        float venusScale = 0.9f * scaleUnit;
        float earthScale = 1 * scaleUnit;
        float marsScale = 0.85f * scaleUnit;
        float jupiterScale = 4f * scaleUnit;
        float saturnScale = 3f * scaleUnit;
        float uranusScale = 2f * scaleUnit;
        float neptuneScale =  1.5f * scaleUnit;
        float moonScale = 0.5f * scaleUnit;
        sun.transform.localScale = new Vector3 (sunScale, sunScale, sunScale);
        mercury.transform.localScale = new Vector3 (mercuryScale, mercuryScale, mercuryScale);
        venus.transform.localScale = new Vector3 (venusScale, venusScale, venusScale);
        earth.transform.localScale = new Vector3 (earthScale, earthScale, earthScale);
        mars.transform.localScale = new Vector3 (marsScale, marsScale, marsScale);
        jupiter.transform.localScale = new Vector3 (jupiterScale, jupiterScale, jupiterScale);
        saturn.transform.localScale = new Vector3 (saturnScale, saturnScale, saturnScale);
        uranus.transform.localScale = new Vector3 (uranusScale, uranusScale, uranusScale);
        neptune.transform.localScale = new Vector3 (neptuneScale, neptuneScale, neptuneScale);
        moon.transform.localScale =   new Vector3 (moonScale, moonScale, moonScale);
    }

    void drawTrack()
    {
        GameObject[] store = {mercury,venus,earth,mars,jupiter,saturn,uranus,neptune};
        for(int i = 0; i < 8;i++)
        {
            store[i].AddComponent<TrailRenderer>();
            TrailRenderer temp = store[i].GetComponent<TrailRenderer>();
            temp.startWidth = 0.05f;
            temp.endWidth = 0.05f;
            temp.time = 60;
        }
    }

    void setRotation()
    {
        rotationUnit = 10f;
        //任意设置自转轴
        Vector3 sunAxis = Vector3.up;
        Vector3 mercuryAxis = new Vector3(1,1,0);
        Vector3 venusAxis = new Vector3(1,1,1);
        Vector3 earthAxis = new Vector3(1,1,0.5f);
        Vector3 marsAxis = new Vector3(1,0.5f,0);
        Vector3 jupiterAxis = new Vector3(1,0,0.5f);
        Vector3 saturnAxis = new Vector3(0.5f,1,1);
        Vector3 uranusAxis = new Vector3(0.5f,1,0);
        Vector3 neptuneAxis = new Vector3(1,0.5f,0.5f);
        Vector3 moonAxis = new Vector3(0.5f,1,0.5f);
        //不同的系数代表不同的自转速度,由于各行星速度差太远，为了方便观察，在保持各行星速度快慢排名不变的情况，缩小其比例
        sun.transform.Rotate(- sunAxis * Time.deltaTime * rotationUnit / 2.5f );
        mercury.transform.Rotate( mercuryAxis * Time.deltaTime * rotationUnit / 5.8f );
        venus.transform.Rotate(- venusAxis * Time.deltaTime * rotationUnit / 10);
        earth.transform.Rotate(- earthAxis * Time.deltaTime * rotationUnit );
        mars.transform.Rotate(- marsAxis * Time.deltaTime * rotationUnit  );
        jupiter.transform.Rotate(- jupiterAxis * Time.deltaTime * rotationUnit / 0.5f );
        saturn.transform.Rotate(- saturnAxis * Time.deltaTime * rotationUnit / 0.5f );
        uranus.transform.Rotate(- uranusAxis * Time.deltaTime * rotationUnit / 0.7f);
        neptune.transform.Rotate(- neptuneAxis * Time.deltaTime * rotationUnit / 0.7f);
        moon.transform.Rotate(- sunAxis * Time.deltaTime * rotationUnit / 10);
    }

    void setRevolution()
    {
        revolutionUnit = 100f;
        Vector3 Origin = sun.transform.position;
        Vector3 mercuryAxis = new Vector3(0,-1,0.4f);
        Vector3 venusAxis = new Vector3(0,4,0.8f);
        Vector3 earthAxis = Vector3.down;
        Vector3 marsAxis = new Vector3(0,-0.5f,0);
        Vector3 jupiterAxis = new Vector3(0,-4f,0.5f);
        Vector3 saturnAxis = new Vector3(0f,-9,1);
        Vector3 uranusAxis = new Vector3(0f,-1,0.2f);
        Vector3 neptuneAxis = new Vector3(0,-2f,0.7f);
        Vector3 moonAxis = Vector3.down;
        mercury.transform.RotateAround( Origin, mercuryAxis, revolutionUnit * Time.deltaTime / 0.2f);
        venus.transform.RotateAround(Origin, venusAxis, revolutionUnit * Time.deltaTime / 0.6f );
        earth.transform.RotateAround(Origin, earthAxis, revolutionUnit * Time.deltaTime );
        mars.transform.RotateAround(Origin, marsAxis, revolutionUnit * Time.deltaTime / 2);
        jupiter.transform.RotateAround(Origin, jupiterAxis, revolutionUnit * Time.deltaTime / 4);
        saturn.transform.RotateAround(Origin, saturnAxis, revolutionUnit * Time.deltaTime / 6);
        uranus.transform.RotateAround(Origin, uranusAxis, revolutionUnit * Time.deltaTime / 10);
        neptune.transform.RotateAround(Origin, neptuneAxis, revolutionUnit * Time.deltaTime / 12);
        moon.transform.RotateAround(earth.transform.position, moonAxis, revolutionUnit * Time.deltaTime / 0.01f);
    }
}
