using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class RaycastFromMouse : MonoBehaviour {
    public GameObject cameraRig;
    public Camera realCamera;
    public Camera fakeCamera;
    public bool activeFocus;

    public GameObject trackerA;
    public GameObject trackerB1;
    public GameObject trackerB2;
    public GameObject trackerC;
    public GameObject trackerD;
    public GameObject trackerE;
    public GameObject trackerMensa;
    public GameObject trackerPoi;
    public GameObject trackerRiz;
    public GameObject trackerSteinbeis;

    public GameObject a;
    public GameObject b1;
    public GameObject b2;
    public GameObject c;
    public GameObject d;
    public GameObject e;
    public GameObject mensa;
    public GameObject poi;
    public GameObject riz;
    public GameObject steinbeis;
    public bool rotating;
    private GameObject rotateTarget;
    public UnityEvent rizEvent;
    public UnityEvent showInfo;
    public UnityEvent hideInfo;

    public GameObject currentRotateTarget;

    private Transform lastCameraTransform;
    // Start is called before the first frame update
    void Start() {

    }

    void enableGhostCam() {
        lastCameraTransform = realCamera.transform;
        fakeCamera.transform.position = lastCameraTransform.position;
        fakeCamera.transform.rotation = lastCameraTransform.rotation;
        fakeCamera.transform.localScale = lastCameraTransform.localScale;
        cameraRig.gameObject.SetActive(false);
        fakeCamera.gameObject.SetActive(true);

    }

    public void returnToView() {
        rotating = false;
        activeFocus = false;
        hideInfo.Invoke();
        Debug.Log("functio ncalled");
        StartCoroutine(LerpPositionGoBack(realCamera.gameObject, 1f, fakeCamera, cameraRig));
    }

    IEnumerator LerpPosition(GameObject target, float duration, Camera fakeCamera) {
        float time = 0;
        Vector3 startPosition = fakeCamera.transform.position;
        Quaternion startRotate = fakeCamera.transform.rotation;
        Debug.Log("Lerping to:" + target.transform.position);
        while (time < duration) {
            //fakeCamera.transform.position = Vector3.Lerp(startPosition, target.transform.position, time / duration);
            fakeCamera.transform.position = new Vector3(Mathf.SmoothStep(startPosition.x, target.transform.position.x, time / duration), Mathf.SmoothStep(startPosition.y, target.transform.position.y, time / duration), Mathf.SmoothStep(startPosition.z, target.transform.position.z, time / duration));
            fakeCamera.transform.rotation = Quaternion.Lerp(startRotate, target.transform.rotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        fakeCamera.transform.position = target.transform.position;
        rotating = true;

    }

    IEnumerator LerpPositionGoBack(GameObject target, float duration, Camera fakeCamera, GameObject rig ) {

        float time = 0;
        Vector3 startPosition = fakeCamera.transform.position;
        Quaternion startRotate = fakeCamera.transform.rotation;
        Debug.Log("Lerping to:" + target.transform.position);
        while (time < duration) {
            //fakeCamera.transform.position = Vector3.Lerp(startPosition, target.transform.position, time / duration);
            fakeCamera.transform.position = new Vector3(Mathf.SmoothStep(startPosition.x, target.transform.position.x, time / duration), Mathf.SmoothStep(startPosition.y, target.transform.position.y, time / duration), Mathf.SmoothStep(startPosition.z, target.transform.position.z, time / duration));
            fakeCamera.transform.rotation = Quaternion.Lerp(startRotate, target.transform.rotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        fakeCamera.transform.position = target.transform.position;
        rotating = false;
        rig.gameObject.SetActive(true);
        fakeCamera.gameObject.SetActive(false);


    }

    IEnumerator RotateAround( GameObject rotateTarget) {
            float time = 0;
            Vector3 startPosition = transform.position;
            while (rotating) {
            transform.RotateAround(rotateTarget.transform.position, Vector3.up, 20 * Time.deltaTime);
            time += Time.deltaTime;
                yield return null;
            }
        
        }
        // Update is called once per frame
        void Update() {
        if (rotating) { 
        fakeCamera.transform.RotateAround(riz.transform.position, Vector3.up, 20 * Time.deltaTime);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hitinfo;

            if (Physics.Raycast(rayOrigin, out hitinfo)) {
                var collidedObject = hitinfo.collider;
                if (collidedObject.tag == "Label") {
                    if (!activeFocus) {
                        activeFocus = true;
                        showInfo.Invoke();
                        enableGhostCam();
                        switch (collidedObject.gameObject.name) {
                            case ("ui_label_Riz"):
                                currentRotateTarget = riz;
                                Debug.Log("lerping");
                                StartCoroutine(LerpPosition(trackerRiz, 1, fakeCamera));

                                break;
                            default: break;
                        }
                    }
                   
                }
            }
        }
    }
}
