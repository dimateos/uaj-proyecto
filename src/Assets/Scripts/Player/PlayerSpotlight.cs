using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Light))]
public class PlayerSpotlight : PlayerLight {
    public Oxygen oxygen;

    void Update()
    {
        if (!player.isSpotlightActive()) return;

        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
        float mouseAngle = -Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(mouseAngle, 90, 0);
    }

    public void TurnOnOff() {
        player.setSpotlightActive(!player.isSpotlightActive());
        _light.enabled = player.isSpotlightActive();
    }
}
