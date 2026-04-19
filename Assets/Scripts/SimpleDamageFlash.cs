using System.Collections;
using UnityEngine;

public class SimpleDamageFlash : MonoBehaviour
{
    public Renderer[] renderers;
    public Material flashMaterial;

    public float duration = 0.3f;
    public float blinkInterval = 0.05f;

    private Material[][] originalMaterials;
    private Coroutine routine;

    void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();

        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void Play()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float timer = 0f;
        bool state = false;

        while (timer < duration)
        {
            state = !state;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (state)
                {
                    // ставим flash материал на все слоты
                    Material[] mats = new Material[renderers[i].materials.Length];
                    for (int j = 0; j < mats.Length; j++)
                        mats[j] = flashMaterial;

                    renderers[i].materials = mats;
                }
                else
                {
                    // возвращаем оригинал
                    renderers[i].materials = originalMaterials[i];
                }
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // в конце обязательно вернуть оригинал
        ResetMaterials();
    }

    private void ResetMaterials()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
    }
}