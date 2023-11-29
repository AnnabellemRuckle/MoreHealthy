using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float turnSpeed = 90f;
    [SerializeField] private int startingHp = 100;
    [SerializeField] private UnityEngine.UI.Slider hpBarSlider = null;
    [SerializeField] private ParticleSystem deathParticlePrefab = null;

    private int currentHp;
    private float animationDuration = 1f; 
    private bool isAnimating = false;

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
            throw new System.ArgumentOutOfRangeException("Invalid Damage amount specified: " + amount);

        if (!isAnimating)
        {
            StartCoroutine(AnimateDamage());
        }
    }

    private void UpdateUI()
    {
        var currentHpPct = CurrentHpPct;
        hpBarSlider.value = currentHpPct;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(startingHp / 10);
        }
    }

    private System.Collections.IEnumerator AnimateDamage()
    {
        isAnimating = true;

        float initialHpPct = CurrentHpPct;
        float targetHpPct = Mathf.Clamp01(initialHpPct - 0.5f); 

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            var currentHpPct = Mathf.Lerp(initialHpPct, targetHpPct, elapsedTime / animationDuration);
            hpBarSlider.value = currentHpPct;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isAnimating = false;
        UpdateUI();
    }
}
