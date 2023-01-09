using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Reflection;
using System;
using UnityEngine.UI;
//[ExecuteInEditMode]
public class ObjectFade : MonoBehaviour {

    public float animationDuration = 1.0F;
    public float targetAlpha = 1F;
    public bool loopAnimation = false;
    public string animationDirection;
    public bool autoPlay = false;
    public float animationDelay;

    private StringParser stringParser;
    private Image buttonImage;
    private RawImage imageObject;
    private TextMeshProUGUI textMeshProObject;
    private bool increaseAlpha = false;
    private float time = 2;

    private void Start() {
        if (autoPlay) time = 0;
        CheckObjectType(this.gameObject);
        stringParser = gameObject.AddComponent<StringParser>();
    }

    void CheckObjectType(GameObject g) {
        switch (g.tag) {
            case "TextMeshPro":
                textMeshProObject = g.GetComponent<TextMeshProUGUI>();
                break;
            case "Image":
                imageObject = g.GetComponent<RawImage>();
                break;
            case "Button":
                buttonImage = g.GetComponent<Image>();
                break;
            default:
                Debug.LogWarning("Warning, [" + g.name + "] has no TAG attached to it!");
                break;
        }
    }

    void Update() {
        //Loop the Fade
        if (loopAnimation) {
            // This "if-else" decides if the alpha should be turned down or pulled up.
            if (increaseAlpha) {
                ProcessAnimation(0F, targetAlpha);
            }
            else {
                ProcessAnimation(targetAlpha, 0F);
            }

            // When the Timer is bigger than 1, it means that the "fade-out" Animation is finished, so we reset the time and lerp alpha back to 1
            if (time >= 1 && !increaseAlpha) {
                time = 0;
                increaseAlpha = true;
            }

            // When increase alpha is set and the time is bigge than 1, it means we now finished a whole cycle of blending down and up again. We loop the Animation.
            if (time >= 1 && increaseAlpha) {
                time = 0;
                increaseAlpha = false;
            }
        }

        //Only plays the animation once
        else {
            if (time <= 1) {
                if (animationDirection == "in") {
                    ProcessAnimation(0F, targetAlpha);
                }
                else if (animationDirection == "out") {
                    ProcessAnimation(targetAlpha, 0F);
                }
            }
        }
    }

    IEnumerator ExecuteAfterTime() {
        yield return new WaitForSeconds(animationDelay);
        time = 0;
    }

    public void FadeObjectUI(GameObject g, string direction) {
        if (direction == "in") {
            loopAnimation = false;
            time = 0;
            animationDirection = "in";
        }
        if (direction == "out") {
            loopAnimation = false;
            time = 0;
            animationDirection = "out";
        }
    }

    // FORMAT OF THE INPUT STRING: Animation_Time (float), AnimationDirection-[in/out] (String), Animation_Delay (float), Target_Alpha [0-1] (Float)
    public void FadeObject(string s) {
        animationDuration = stringParser.ConvertStringToFloat(stringParser.ConvertStringsToArray(s)[0]);
        animationDirection = stringParser.ConvertStringsToArray(s)[1];
        animationDelay = stringParser.ConvertStringToFloat(stringParser.ConvertStringsToArray(s)[2]);
        Debug.Log(animationDelay);
        //targetAlpha = stringParser.ConvertStringToFloat(stringParser.ConvertStringsToArray(s)[3]);
        loopAnimation = false;
        StartCoroutine(ExecuteAfterTime());
    }

    // This function takes two values and interpolates them, which sets the alpha of the Object this script is applied to.
    private void ProcessAnimation(float from, float to) {
        if (textMeshProObject != null) {
            Color textMeshColor = textMeshProObject.color; //Create a Color Object, which we will set to the textMeshObject in the End, we can't directly affect the Alpha of the textMeshObject.
            time += Time.deltaTime / animationDuration;
            textMeshColor.a = Mathf.SmoothStep(from, to, time);
            textMeshProObject.color = textMeshColor;
        }
        else if (imageObject != null) {
            Color imageColor = imageObject.color;
            time += Time.deltaTime / animationDuration;
            imageColor.a = Mathf.SmoothStep(from, to, time);
            imageObject.color = imageColor;
        }
        else if (buttonImage != null) {
            Color imageColor = buttonImage.color;
            time += Time.deltaTime / animationDuration;
            imageColor.a = Mathf.SmoothStep(from, to, time);
            buttonImage.color = imageColor;
        }

        //Reset Attributes after Animation
        if (time >= 1) {
            animationDirection = "";
            time = 2;
        }
    }
}
