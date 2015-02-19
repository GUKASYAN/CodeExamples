 
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/KeykNavigation")]
public class myKeyController : MonoBehaviour
{
    public static BetterList<myKeyController> list = new BetterList<myKeyController>();
    private bool pressed;
    public enum Constraint
    {
        None,
        Vertical,
        Horizontal,
        Explicit,
    } 
    public Constraint constraint = Constraint.None;
    public GameObject onUp;
    public GameObject onDown;
    public GameObject onLeft;
    public GameObject onRight;
    public GameObject onClick;
    public bool startsSelected = false;

    protected virtual void OnEnable()
    {
        list.Add(this);

        if (startsSelected)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject))
            {
                UICamera.currentScheme = UICamera.ControlScheme.Controller;
                UICamera.selectedObject = gameObject;
            }
        }
    } 
    protected virtual void OnDisable()
    {
        list.Remove(this);
    } 
    protected GameObject GetLeft()
    {
        if (NGUITools.GetActive(onLeft)) return onLeft;
        if (constraint == Constraint.Vertical || constraint == Constraint.Explicit) return null;
        return Get(Vector3.left, true);
    }
    protected GameObject GetRight()
    {
        if (NGUITools.GetActive(onRight))
        {
            return onRight;
        }
        if (constraint == Constraint.Vertical || constraint == Constraint.Explicit)
        {
            return null;
        }
        return Get(Vector3.right, true);
    }

    protected GameObject GetUp()
    {
        if (NGUITools.GetActive(onUp)) return onUp;
        if (constraint == Constraint.Horizontal || constraint == Constraint.Explicit) return null;
        return Get(Vector3.up, false);
    } 
    protected GameObject GetDown()
    {
        if (NGUITools.GetActive(onDown)) return onDown;
        if (constraint == Constraint.Horizontal || constraint == Constraint.Explicit) return null;
        return Get(Vector3.down, false);
    }  
    protected GameObject Get(Vector3 myDir, bool horizontal)
    {
        Transform t = transform;
        myDir = t.TransformDirection(myDir);
        Vector3 myCenter = GetCenter(gameObject);
        float min = float.MaxValue;
        GameObject go = null;

        for (int i = 0; i < list.size; ++i)
        {
            myKeyController nav = list[i];
            if (nav == this) continue;
            Vector3 dir = GetCenter(nav.gameObject) - myCenter;
            float dot = Vector3.Dot(myDir, dir.normalized);
            if (dot < 0.707f) continue;
            dir = t.InverseTransformDirection(dir);
            if (horizontal) dir.y *= 2f;
            else dir.x *= 2f;
            float mag = dir.sqrMagnitude;
            if (mag > min) continue;
            go = nav.gameObject;
            min = mag;
        }
        return go;
    }
    protected static Vector3 GetCenter(GameObject go)
    {
        var w = go.GetComponent<UIWidget>();

        if (w != null)
        {
            Vector3[] corners = w.worldCorners;
            return (corners[0] + corners[2])*0.5f;
        }
        return go.transform.position;
    }  
    private void Update()
    {
        var key = new KeyCode();
        if (!pressed)
        {
            if (Input.GetButtonUp("Up") && gameObject == UICamera.selectedObject)
            {
                key = KeyCode.UpArrow;
                pressed = true;
            }
            if (Input.GetButtonUp("Down") && gameObject == UICamera.selectedObject)
            {
                key = KeyCode.DownArrow;
                pressed = true;
            }
            if (Input.GetButtonUp("Left") && gameObject == UICamera.selectedObject)
            {
                key = KeyCode.LeftArrow;
                pressed = true;
            }
            if (Input.GetButtonUp("Right") && gameObject == UICamera.selectedObject)
            {
                key = KeyCode.RightArrow;
                pressed = true;
            }
        }
        if (pressed)
        {
            if (!NGUITools.GetActive(this)) return;
            GameObject go = null;
            switch (key)
            {
                case KeyCode.LeftArrow:
                    go = GetLeft();
                    break;
                case KeyCode.RightArrow:
                    go = GetRight();
                    break;
                case KeyCode.UpArrow:

                    go = GetUp();

                    break;
                case KeyCode.DownArrow:
                    go = GetDown();
                    break;
                case KeyCode.Tab:
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        go = GetLeft();
                        if (go == null) go = GetUp();
                        if (go == null) go = GetDown();
                        if (go == null) go = GetRight();
                    }
                    else
                    {
                        go = GetRight();
                        if (go == null) go = GetDown();
                        if (go == null) go = GetUp();
                        if (go == null) go = GetLeft();
                    }
                    break;
            }
            if (go != null)
            {
                UICamera.currentScheme = UICamera.ControlScheme.Controller;
                UICamera.selectedObject = go;
                pressed = false;
            }
        }
    }
    protected virtual void OnClick()
    {
        if (NGUITools.GetActive(this) && NGUITools.GetActive(onClick))
            UICamera.selectedObject = onClick;
    }
}
