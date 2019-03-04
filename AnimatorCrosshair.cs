using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For animating Sprites - alpha version
/// Attach to object you wish to be able to change sprites for
/// Based on location and status of crosshair
/// </summary>
public class AnimatorCrosshair : MonoBehaviour {

    public CrosshairController crosshair;
    public Sprite _sprite1;
    public Sprite _sprite2;
    //private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        /// <summary>
        /// if crosshair active 
        /// </summary> 
        if (crosshair.crosshairActive)
        {
            _spriteRenderer.sprite = _sprite1;
            //_animator.Play(Animator.StringToHash("Aim"));
        }

        /// <summary>
        /// if crosshair inactive and mouse is centered on sprite
        /// </summary> 
        if (crosshair.crosshairActive == false && Input.GetAxis("Horizontal") == 0)
        {
            _spriteRenderer.sprite = _sprite2;
            //_animator.Play(Animator.StringToHash("Idle"));
        }
    }
}
