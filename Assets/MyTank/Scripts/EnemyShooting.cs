using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] GameObject m_Target;
    [SerializeField] GameObject m_Turret;
    [SerializeField] Rigidbody m_Shell;
    [SerializeField] float m_LaunchForce;
    [SerializeField] float m_Range;
    [SerializeField] float m_FireRate;
    [SerializeField] float m_ReloadTime;
    [SerializeField] Transform m_FireTransform;
    [SerializeField] AudioSource m_ShootingAudio;
    [SerializeField] AudioClip m_FireClip;

    private bool m_Fired;
    private float m_NextShoot;
    // Start is called before the first frame update
    void Start()
    {
        m_Fired = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(m_Target.transform.position, m_Turret.transform.position);
        if( distance < m_Range )
        {
            Vector3 aim = new Vector3(
                            m_Target.transform.position.x,
                            m_Turret.transform.position.y,
                            m_Target.transform.position.z) - m_Turret.transform.position;
            Quaternion angle = Quaternion.LookRotation(aim, Vector3.up);
            m_Turret.transform.rotation = Quaternion.Lerp(m_Turret.transform.rotation,
                                                            angle, 2.0f * Time.deltaTime);
            if (Random.Range(0.0f, 1.0f) < m_FireRate * Time.deltaTime && !m_Fired)
            {
                Fire();
            }
            if(m_Fired)
            {
                m_NextShoot -= Time.deltaTime;
                if( m_NextShoot < 0.0f)
                {
                    m_Fired = false;
                }
            }
        }
    }

    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;
        m_NextShoot = m_ReloadTime;
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position,
        m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
    }
}
