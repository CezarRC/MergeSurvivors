using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNumber
{
    public static void CreateHitNumber(Vector3 worldPosition, float damage, bool crit)
    {
        GameObject go = GameObject.Instantiate((GameObject)Resources.Load("UI/HitNumber"));
        go.transform.position = new Vector3(worldPosition.x + Random.Range(-0.25f, 0.25f), worldPosition.y + Random.Range(1.5f, 2f), worldPosition.y);
        go.GetComponent<TMPro.TMP_Text>().text = "" + (int)damage;
        if (crit)
        {
            go.GetComponent<TMPro.TMP_Text>().color = Color.cyan;
        }
        else
        {
            go.GetComponent<TMPro.TMP_Text>().color = GetHitNumberColor(damage);
        }

        float size = crit ? 1 * (0.1f + (damage * 2) / 1000) : 1 * (0.1f + damage / 1000);
        go.transform.localScale = Vector3.zero;
        go.transform.LeanScale(new Vector3(size, size, 1), 0.2f).setEaseOutElastic().setOnComplete(
            (value) => {
                go.transform.LeanScale(Vector3.zero, 0.8f).setEaseInQuint().setDestroyOnComplete(true);
            });
    }

    static Color GetHitNumberColor(float damage)
    {
        Gradient gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        GradientColorKey[] colorKey = new GradientColorKey[6];
        colorKey[0].color = Color.yellow;
        colorKey[0].time = 0.0f;
        colorKey[1].color = new Color(1, 100f / 255f, 0); //Orange
        colorKey[1].time = 62.5f / 1000f;
        colorKey[2].color = Color.red;
        colorKey[2].time = 125f / 1000f;
        colorKey[3].color = new Color(1, 0, 0.5f); //Semi-Magenta
        colorKey[3].time = 250f / 1000f;
        colorKey[4].color = Color.magenta;
        colorKey[4].time = 500f / 1000f;
        colorKey[5].color = Color.cyan;
        colorKey[5].time = 1000f / 1000f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[6];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 62.5f / 1000f;
        alphaKey[2].alpha = 1.0f;
        alphaKey[2].time = 125f / 1000f;
        alphaKey[3].alpha = 1.0f;
        alphaKey[3].time = 250f / 1000f;
        alphaKey[4].alpha = 1.0f;
        alphaKey[4].time = 500f / 1000f;
        alphaKey[5].alpha = 1.0f;
        alphaKey[5].time = 1000f / 1000f;

        gradient.SetKeys(colorKey, alphaKey);

        // What's the color at the relative time 0.25 (25 %) ?
        return gradient.Evaluate(damage / 1000);
    }
}
