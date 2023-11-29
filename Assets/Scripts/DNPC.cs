using System;
using UnityEngine;

public class DNPC : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float turnSpeed = 90f;
    [SerializeField] private int startingHp = 100;
    [SerializeField] private UnityEngine.UI.Slider hpBarSlider = null;
    [SerializeField] private ParticleSystem deathParticlePrefab = null;
    private int currentHp;

    private void Start()
    {
        currentHp = startingHp;
        UpdateUI();
    }

    public float CurrentHpPct
    {
        get { return (float)currentHp / (float)startingHp; }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("Invalid Damage amount specified: " + amount);

        currentHp -= amount;

        UpdateUI();

        if (currentHp <= 0)
            Die();
    }

    private void UpdateUI()
    {
        var currentHpPct = CurrentHpPct;
        hpBarSlider.value = currentHpPct;
    }

    private void Die()
    {
        PlayDeathParticle();
        Destroy(gameObject);
    }

    private void PlayDeathParticle()
    {
        var deathparticle = Instantiate(deathParticlePrefab, transform.position, deathParticlePrefab.transform.rotation);
        Destroy(deathparticle, 4f);
    }

    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
        hpBarSlider.transform.LookAt(Camera.main.transform);

        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(currentHp);
        }
    }
}
