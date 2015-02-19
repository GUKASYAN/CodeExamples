using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour
{
    public GameObject areas;
    public Camera MainCam;
    public Material mater1;
    public Material mater0;
    
     
    private bool _bResizing = false;
    private float distance = 12f;
    private float distanceBegin = 0;
    private float distanceEnd = 0;
  
    private UIViewport ViewportMaincam;
    private UIDraggableCamera DraggableCamera;
    
    public float StartScale;
    private float value = 0; 
    private float lastsize = 0;
     
    float _oneTapTimeD;

    private Vector3[] masPolygonVert;
    private Vector3 v3curCamPos;
    private Vector3 v3newCamPos;
    private Renderer curareaRenderer = null;
    private String _selectedareaName;


    public GameObject LabelName;
    public GameObject vLabelIndex;
    public GameObject vLabelShore;
    public GameObject vLabelContinentID;
    public GameObject vLabelNeighbCount;
    public GameObject vLabelTheName;

    private static PlanetDatabase _Instance;

    private static PlanetDatabase WDI
    {
        get
        {
            if (_Instance == null)
                _Instance = Resources.Load("Planet", typeof(PlanetDatabase)) as PlanetDatabase;
            
            return _Instance;
        }

    }

	void Start ()
	{ 
 
        DraggableCamera = MainCam.GetComponent<UIDraggableCamera>();
        ViewportMaincam = MainCam.GetComponent<UIViewport>();
	    StartScale = ViewportMaincam.fullSize;
     
    }
    private void Update()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////START ZOOM
        if (Input.touchCount == 2)
        {
            if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began)
            {
                distanceBegin = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                v3curCamPos = MainCam.transform.position;
                v3newCamPos =MainCam.ScreenToPlanetPoint((Input.touches[0].position + Input.touches[1].position) / 2);
              
                lastsize = ViewportMaincam.fullSize;
                _bResizing = true;
                
                if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Ended)
                {
                    DraggableCamera.scale.x = 1;
                    DraggableCamera.scale.y = 1;
                    _bResizing = false;
           
                }



            }

        }
        ////////////////////////////////////////////////////////////////////////////////////////////END ZOOM
        /// 
        ////////////////////////////////////////////////////////////////////////////////////////////START SCROLL
          if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
          {
              MainCam.GetComponent<UIViewport>().fullSize += 0.1f;

          }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
        {
            MainCam.GetComponent<UIViewport>().fullSize -= 0.1f;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////END SCROLL
   
        if (Input.GetMouseButtonDown(0))
        {
            _oneTapTimeD = Time.time;
           }
        
        if (Input.GetMouseButtonUp(0))
            {
                if ((Time.time - _oneTapTimeD)<0.2)  
                {
                    
                    Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit myhit;

                    if (Physics.Raycast(ray, out myhit, 1000.0f))
                    {
                        foreach (Transform areaTrasform in myhit.transform.GetComponentsInChildren<Transform>())
                        {
                            MeshFilter Fil = areaTrasform.GetComponent<MeshFilter>();
                            if (Fil != null)
                            {
                                masPolygonVert = Fil.mesh.vertices;
                           
                              
                                if (IsPointInPolygon(masPolygonVert, (myhit.point - areaTrasform.position)*400))
                                {
                                    int WatchID = Convert.ToInt32(Regex.Match(areaTrasform.name, @"\d+").Value);

                               

                                    LabelName.GetComponent<UILabel>().text = Convert.ToString(areaTrasform.name);
                                    vLabelIndex.GetComponent<UILabel>().text = "[00c800]" + Convert.ToString(WDI.areas[WatchID].Index) + "[-]";
                                    vLabelShore.GetComponent<UILabel>().text = "[00c800]" + Convert.ToString(WDI.areas[WatchID].shore) + "[-]";
                                    vLabelNeighbCount.GetComponent<UILabel>().text = "[00c800]" + Convert.ToString(WDI.areas[WatchID].neighbors.Count) + "[-]";
                                    if (curareaRenderer == null)
                                    {
                                        areaTrasform.renderer.material = mater1;
                                        curareaRenderer = areaTrasform.renderer;
                                        _selectedareaName = areaTrasform.name;
                                    }
                                    else
                                    {
                                        if (_selectedareaName.Equals(areaTrasform.name))
                                        {
                                            curareaRenderer = null;
                                            _selectedareaName = string.Empty;
                                            areaTrasform.renderer.material = mater0;
                                        }
                                        else
                                        {
                                            curareaRenderer.material = mater0;
                                            areaTrasform.renderer.material = mater1;
                                            curareaRenderer = areaTrasform.renderer;
                                            _selectedareaName = areaTrasform.name;
                                        }
                                    }

                                }

                            }
                        }
                    }
                }
            }
             
    }
    private void LateUpdate()
    {
        if (_bResizing)
        {


            distanceEnd = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            distance = ((distanceBegin - distanceEnd));
            ViewportMaincam.fullSize = Mathf.Clamp(ViewportMaincam.fullSize + distance / 300, 0.1f, StartScale);
            distance = 0;
            distanceBegin = distanceEnd;
            value = lastsize <= 0.01f ? 0 : (lastsize - ViewportMaincam.fullSize) / (lastsize - 0.01f);
            MainCam.transform.position = v3curCamPos + value * (v3newCamPos - v3curCamPos);
        }
        
    }

    public static bool IsPointInPolygon(Vector3[] polygon, Vector3 testPoint)
    {
        bool result = false;
        int j = polygon.Count() - 1;
        for (int i = 0; i < polygon.Count(); i++)
        {
            if (polygon[i].y < testPoint.y && polygon[j].y >= testPoint.y || polygon[j].y < testPoint.y && polygon[i].y >= testPoint.y)
            {
                if (polygon[i].x + (testPoint.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x- polygon[i].x) < testPoint.x)
                {
                    result = !result;
                }
            }
            j = i;
        }
        return result;
    }

    
}
     
     







