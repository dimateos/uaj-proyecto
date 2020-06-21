using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PolygonCollider2D))]
public class PlayerPhotos : MonoBehaviour
{
    public Player player;
    public GameObject photoUI;

    private PolygonCollider2D _photoCollider;
    private Text _photoUIText;
    private Image _photoUIImage;

    private Fish _fishOnRange;
    private bool _inputPhoto = false;

    // Start is called before the first frame update
    void Start()
    {
        _photoCollider = GetComponent<PolygonCollider2D>();
        _photoUIText = photoUI.GetComponentInChildren<Text>();
        _photoUIImage = photoUI.GetComponentsInChildren<Image>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (_fishOnRange != null && _inputPhoto) {
            photographFish(_fishOnRange);
            _fishOnRange = null;
        }
        _inputPhoto = false;

        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);
        float mouseAngle = -Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, -mouseAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Fish fish = collision.gameObject.GetComponent<Fish>();
        if (fish != null && !Player.GetProgress().getFishPhoto(fish.fishType)) _fishOnRange = fish;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Fish fish = collision.gameObject.GetComponent<Fish>();
        if (fish == _fishOnRange) _fishOnRange = null;
    }

    public void photographFish() {
        _inputPhoto = true;
    }

    private void photographFish(Fish fish)
    {
        player.photographFish(fish.fishType);
        photoUI.SetActive(true);
        _photoUIText.text = Progress.FishName[(int)fish.fishType];
        _photoUIImage.sprite = fish.getSprite();
    }

    public bool canPhotograph() {
        return _fishOnRange != null;
    }
}
