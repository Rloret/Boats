using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This script has been partially modified so that it accepts the suitable changes for our Game.
/// The original code is in the Dossier.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FlotabilityController : MonoBehaviour
{
    public GameObject[] m_hoverPoints;
    public List<GameObject> m_twinHoverPoints;


    public GameObject m_twinHover;

    public float m_hoverForce = 9.0f;
    public float m_hoverHeight = 2.0f;

    private int Gravityatenuator = 8;
    private int m_layerMask;
    private float offsetX;
    private float offsetZ;
    private float centerY;

    public GameObject stabilizer;
    public GameObject twinHoverParent;
    private Rigidbody m_body;


    void Start()
    {
        m_body = GetComponent<Rigidbody>();

        m_layerMask = 1 << LayerMask.NameToLayer("Characters");
        m_layerMask = ~m_layerMask;
        Vector3 masscenter = GetComponent<Rigidbody>().worldCenterOfMass;
        offsetX = GetComponent<BoxCollider>().bounds.extents.x;
        offsetZ = GetComponent<BoxCollider>().bounds.extents.z * 0.75f;
        centerY = (transform.TransformPoint(GetComponent<BoxCollider>().center - GetComponent<BoxCollider>().size / 2)).y;


        m_hoverPoints[0].transform.position = new Vector3(masscenter.x + offsetX, centerY, masscenter.z + offsetZ);
        m_hoverPoints[1].transform.position = new Vector3(masscenter.x - offsetX, centerY, masscenter.z + offsetZ);
        m_hoverPoints[2].transform.position = new Vector3(masscenter.x - offsetX, centerY, masscenter.z - offsetZ);
        m_hoverPoints[3].transform.position = new Vector3(masscenter.x + offsetX, centerY, masscenter.z - offsetZ);

        twinHoverParent = new GameObject();
        twinHoverParent.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        twinHoverParent.name = "TwinHoverParent from " + this.name;


        twinHoverParent.AddComponent<Rigidbody>().useGravity = false;
        twinHoverParent.GetComponent<Rigidbody>().mass = m_body.mass;
        twinHoverParent.GetComponent<Rigidbody>().drag = m_body.drag;



        for (int i = 0; i < m_hoverPoints.Length; i++)
        {

            m_twinHoverPoints.Add(new GameObject("twin from " + m_hoverPoints[i].name));
            m_twinHoverPoints[i].AddComponent<rePositionHover>();
            m_twinHoverPoints[i].transform.parent = twinHoverParent.transform;
            Vector3 offsetfromparent = m_hoverPoints[i].transform.position - this.transform.position;
            m_twinHoverPoints[i].transform.position = twinHoverParent.transform.position + offsetfromparent;
            m_twinHoverPoints[i].GetComponent<rePositionHover>().sibling = m_hoverPoints[i];


        }

        if (stabilizer == null)
        {
            stabilizer = new GameObject();
            stabilizer.name = this.name + " Stabilizer";

            stabilizer.transform.position = new Vector3(this.transform.position.x, centerY - 1f, this.transform.position.z);
            stabilizer.transform.parent = this.transform;
        }

    }
    void moveTwins()
    {
        for (int i = 0; i < m_hoverPoints.Length; i++)
        {
            Vector3 offsetfromparent = m_hoverPoints[i].transform.position - this.transform.position;
            m_twinHoverPoints[i].GetComponent<rePositionHover>().posFromParent(offsetfromparent, twinHoverParent.transform.position);
            m_twinHoverPoints[i].GetComponent<rePositionHover>().checkPosInGrid();
        }
    }

/*   void OnDrawGizmos()
    {

        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_hoverPoints.Length; i++)
        {
            Gizmos.DrawSphere(m_twinHoverPoints[i].transform.position, 0.5f);
            var hoverPoint = m_hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position,
                -Vector3.up, out hit,
                m_hoverHeight,
                m_layerMask))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, 0.5f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hoverPoint.transform.position,
                    hoverPoint.transform.position - Vector3.up * m_hoverHeight);
            }
        }
    }*/

    void FixedUpdate()
    {
        RaycastHit hit;


        for (int i = 0; i < m_hoverPoints.Length; i++)
        {

            var hoverPoint = m_hoverPoints[i];
            var hoverTwin = m_twinHoverPoints[i];
            if (hoverTwin !=null && Physics.Raycast(new Vector3(hoverTwin.transform.position.x, hoverTwin.transform.position.y, hoverTwin.transform.position.z), -Vector3.up, out hit, m_hoverHeight, m_layerMask))
            {
                m_body.AddForceAtPosition(new Vector3(0, 1, 0) * m_hoverForce * (1.0f - (hit.distance / m_hoverHeight)), hoverPoint.transform.position);
            }

            else
            {
                if (stabilizer.transform.position.y > hoverPoint.transform.position.y)
                {
                    m_body.AddForceAtPosition(hoverPoint.transform.up * m_hoverForce / Gravityatenuator, hoverPoint.transform.position);
                }
                else
                {
                    m_body.AddForceAtPosition(hoverPoint.transform.up * -m_hoverForce / Gravityatenuator / 2, hoverPoint.transform.position);
                }
            }
        }



       if(twinHoverParent!=null) twinHoverParent.transform.position = this.transform.position;
        moveTwins();


    }
}