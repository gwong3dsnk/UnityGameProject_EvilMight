using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeSlash : PlayerAbilities
{
    private Camera mainCamera;

    public override void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene", this);
        }

        base.Awake();
    }

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        Debug.Log("Activating ScreenAOE", this);
        KillVisibleEnemies();
    }

    private void KillVisibleEnemies()
    {
        GameObject[] visibleEnemies = GetVisibleEnemies();

        foreach (GameObject enemy in visibleEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth == null)
            {
                Debug.LogError("Missing reference to EnemyHealth", this);
            }

            enemyHealth.ApplyAOEDamage(damage);
        }
    }

    private GameObject[] GetVisibleEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> visibleEnemies = new List<GameObject>();

        foreach (GameObject enemy in allEnemies)
        {
            if (IsVisible(enemy))
            {
                visibleEnemies.Add(enemy);
            }
        }

        return visibleEnemies.ToArray();
    }

    private bool IsVisible(GameObject enemy)
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemy.transform.position);

        // Check if the object is within the screen's x, y bounds using z as the distance out from the camera.
        return screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;
    }

    public override void DeactivateAbility()
    {
        throw new System.NotImplementedException();
    }

    public override void ActivateUpgrade(Dictionary<string, AbilityUpgrades> upgrade)
    {
        throw new System.NotImplementedException();
    }

    protected override void InitializeAbilityData()
    {
        throw new System.NotImplementedException();
    }    
}
