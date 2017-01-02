using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 
    public float m_ScreenEdgeBuffer = 4f;           
    public float m_MinSize = 6.5f;                  
    /*[HideInInspector]*/ public Transform[] m_Targets; 


    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;                 
    private Vector3 m_DesiredPosition;              


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            averagePos += m_Targets[i].position;//accrue all positions
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;//average out the accrued positions

        averagePos.y = transform.position.y;//change y to current camera's y (don't want average of y's)

        m_DesiredPosition = averagePos;//change the global m_DesiredPosition var to be SmoothDamped in the next Move() line.
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()//zoom to accomodate furthest tank
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);//uses Camera Rig's local space

        float size = 0f;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);//again, relative to the camera's local space

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));//calculate vertical separately

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);//compare horizontal with earlier vertical
        }
        
        size += m_ScreenEdgeBuffer;

        size = Mathf.Max(size, m_MinSize);//Is the minSize bigger than the current biggest?

        return size;
    }


    public void SetStartPositionAndSize()//to be used at the beginning of each fight
    {
        FindAveragePosition();//Just Move w/o the dampening

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();//Just Zoom w/o the dampening
    }
}