using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoKitStylizedAnimatedDogs
{
public class DemoController : MonoBehaviour
{
    [SerializeField] private List<AnimationButton> _buttons;
    [SerializeField] private List<GameObject> _dogs;
    private List<Animator> _animators = new List<Animator>();

    private void Start()
    {
       foreach(var button in _buttons)
       {
          button.Click += OnAnimationButtonClick;
       }

      foreach(GameObject dog in _dogs)
      {
         Animator animator = dog.GetComponent<Animator>();
         // Check if the Animator is found
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
         _animators.Add(animator);
      }

    }

    private void OnAnimationButtonClick(int id)
    {
       foreach(var animator in _animators)
       {
          animator.SetInteger("AnimationID",id);
       }
    }
}
}