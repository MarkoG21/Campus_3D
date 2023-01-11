using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayAnimation : MonoBehaviour {
    public UnityEvent animationFinished;
    public UnityEvent animationFinished2;
    private AnimationClip currentClip;

    private Animation animationComponent;
    // Start is called before the first frame update
    void Start() {
        animationComponent = gameObject.GetComponent<Animation>();
    }

    public void InvoceAfterAnimationDelay() {
        StartCoroutine(ExecuteAfterTime(currentClip.length + 0.5F));
    }

    public void InvoceDirectly() {
        animationFinished2.Invoke();
    }

    public void ExecuteAfterAnimation() {

    }

    IEnumerator ExecuteAfterTime(float delay) {
        yield return new WaitForSeconds(delay);
        animationFinished.Invoke();
    }

    public void PlayAnimationClip(AnimationClip a) {
        animationComponent.Play(a.name);
        currentClip = a;


    }

    // Update is called once per frame
    void Update() {

    }
}
