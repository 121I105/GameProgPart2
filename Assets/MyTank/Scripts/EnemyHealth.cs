using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;
    public GameObject m_ExplosionPrefab;
    public TextMeshProUGUI m_StatusText;

    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;
    private float m_CurrentHealth;
    private bool m_Dead;
    private Camera m_Camera;

    private void Awake()
    {
        GameObject obj = GameObject.Find("Main Camera");
        m_Camera = obj.GetComponent<Camera>();
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;
        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount;
        SetHealthUI();
        if (m_CurrentHealth <= 0.0f && !m_Dead)
            {
                OnDeath();
            }
    }

    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_StatusText.text = m_CurrentHealth.ToString();
    }

    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        m_StatusText.transform.position = m_Camera.WorldToScreenPoint(
                                            new Vector3(transform.position.x,
                                            transform.position.y + 3.0f,
                                            transform.position.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeDamage(10.0f);
    }
}
