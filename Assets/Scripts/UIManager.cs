using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private float delay;
    public GameObject latestDrawnUIComponent;

    public void ShowUI(Transform g) {

        //Iterate over the child objects of the parent gameObject.
        foreach (Transform child in latestDrawnUIComponent.transform) {
            //Prevent the user from pressing a button while the animation is running.
            if (child.gameObject.GetComponent<Button>() != null) {
                child.gameObject.GetComponent<Button>().interactable = false;
            }

            //This takes the delay+duration from the last gameObject in the Hierachy and waits the needed time before finishing the fadeOut
            delay = child.gameObject.GetComponent<ObjectFade>().animationDelay + child.gameObject.GetComponent<ObjectFade>().animationDuration;
            //Fades the UI component out.

            child.gameObject.GetComponent<ObjectFade>().FadeObjectUI(child.gameObject, "out");
        }

        //Wait for the fadeOut animation to complete.
        StartCoroutine(ExecuteAfterTime(delay, g));
    }

    //This function waits for the fadeOut animation to complete and fades the other UI object in.
    IEnumerator ExecuteAfterTime(float delay, Transform g) {
        //We also need to add the additional delay from the fadeIn object.
        float fadeInDelay = 0;
        foreach (Transform child in g) {
            fadeInDelay = child.gameObject.GetComponent<ObjectFade>().animationDelay;
        }
        yield return new WaitForSeconds(delay+ fadeInDelay);

        //Deactivate the UI component that has been faded out.
        latestDrawnUIComponent.SetActive(false);
        //The UI that will be faded in, is now the latestDrawnUI component.
        latestDrawnUIComponent = g.gameObject;

        //We have to make sure the User can't interact with the buttons while the fade animation is being played.
        foreach (Transform child in g) {
            if (child.gameObject.GetComponent<Button>() != null) {
                child.gameObject.GetComponent<Button>().interactable = false;
            }

            //Activate the UI component in order to fade it in.
            g.gameObject.SetActive(true);

            //Fades the UI component in.
            child.gameObject.GetComponent<ObjectFade>().FadeObjectUI(child.gameObject, "in");
        }
        StartCoroutine(ExecuteAfterTimeIn(g));
    }

    //We wait for the animation to finish, before we can make the buttons interactable.
    IEnumerator ExecuteAfterTimeIn(Transform g) {
        float delay = 0;
        foreach (Transform child in g) {
            delay = child.gameObject.GetComponent<ObjectFade>().animationDelay + child.gameObject.GetComponent<ObjectFade>().animationDuration;
        }

        yield return new WaitForSeconds(delay);

        //After the animation has been finished, the buttons are interactable.
        foreach (Transform child in g) {
            if (child.gameObject.GetComponent<Button>() != null) {
                child.gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }

}
